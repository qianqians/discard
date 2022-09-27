import random
import math
import time

import torch
import torch.nn as nn
from torch import optim
import torch.nn.functional as F

from snownlp import SnowNLP

import dictionary
import utils
import pretrain
import seq2seq

class PositionalEncoding(nn.Module):
    def __init__(self, d_model, dropout=0.1, max_len=5000):
        super(PositionalEncoding, self).__init__()
        self.dropout = nn.Dropout(p=dropout)

        pe = torch.zeros(max_len, d_model)
        position = torch.arange(0, max_len, dtype=torch.float).unsqueeze(1)
        div_term = torch.exp(torch.arange(0, d_model, 2).float() * (-math.log(10000.0) / d_model))
        pe[:, 0::2] = torch.sin(position * div_term)
        pe[:, 1::2] = torch.cos(position * div_term)
        pe = pe.unsqueeze(0).transpose(0, 1)
        self.register_buffer('pe', pe)

    def forward(self, x):
        x = x + self.pe[:x.size(0), :]
        return self.dropout(x)

class LearnedPositionEncoding(nn.Embedding):
    def __init__(self, d_model, dropout=0.1, max_len=5000):
        super().__init__(max_len, d_model)
        self.dropout = nn.Dropout(p=dropout)

    def forward(self, x):
        weight = self.weight.data.unsqueeze(1)
        x = x + weight[:x.size(0), :]
        return self.dropout(x)

class TransformerEncoderModel(nn.Module):
    def __init__(self, ntoken, ninp, nhead, nlayers):
        super(TransformerEncoderModel, self).__init__()

        self.src_mask = None
        self.ninp = ninp

        self.embedding = nn.Embedding(ntoken, ninp)
        self.pos_encoder = LearnedPositionEncoding(ninp)
        encoder_layers = nn.TransformerEncoderLayer(ninp, nhead)
        self.transformer_encoder = nn.TransformerEncoder(encoder_layers, nlayers)

    def _generate_square_subsequent_mask(self, sz):
        mask = (torch.triu(torch.ones(sz, sz)) == 1).transpose(0, 1).cuda()
        mask = mask.float().masked_fill(mask == 0, float('-inf')).masked_fill(mask == 1, float(0.0))
        return mask

    def forward(self, src, has_mask=True):
        if has_mask:
            if self.src_mask is None or self.src_mask.size(0) != len(src):
                mask = self._generate_square_subsequent_mask(len(src))
                self.src_mask = mask
        else:
            self.src_mask = None

        src = self.embedding(src) * math.sqrt(self.ninp)
        src = self.pos_encoder(src)
        output = self.transformer_encoder(src, mask=self.src_mask)

        return output

class TransformerDecoderModel(nn.Module):
    def __init__(self, ntoken, ninp, nhead, nlayers):
        super(TransformerDecoderModel, self).__init__()

        self.tgt_mask = None

        self.embedding = nn.Embedding(ntoken, ninp)
        self.pos_encoder = LearnedPositionEncoding(ninp)
        decoder_layer = nn.TransformerDecoderLayer(ninp, nhead)
        self.transformer_decoder = nn.TransformerDecoder(decoder_layer, num_layers=nlayers)
        self.output_layer = nn.Linear(ninp, ntoken)

    def _generate_square_subsequent_mask(self, sz):
        mask = (torch.triu(torch.ones(sz, sz)) == 1).transpose(0, 1).cuda()
        mask = mask.float().masked_fill(mask == 0, float('-inf')).masked_fill(mask == 1, float(0.0))
        return mask

    def forward(self, tgt, mem, has_mask=True):
        if has_mask:
            if self.tgt_mask is None or self.tgt_mask.size(0) != len(tgt):
                mask = self._generate_square_subsequent_mask(len(tgt))
                self.tgt_mask = mask
        else:
            self.tgt_mask = None

        tgt = self.embedding(tgt)
        tgt = self.pos_encoder(tgt)
        output = self.transformer_decoder(tgt, mem, tgt_mask=self.tgt_mask)
        output = self.output_layer(output)
        return F.softmax(output, dim=2)

def train(iter, input_tensor, target_tensor, encoder, decoder, encoder_optimizer, decoder_optimizer, criterion, clipping_theta):
    mem = encoder(input_tensor)
    decoder_input = torch.tensor([[dictionary.Dictionary.SOS_token]], device=torch.device('cuda'))

    decoded_words = []
    loss = 0
    target_length = target_tensor.size(0)
    for di in range(target_length):
        decoder_output = decoder(decoder_input, mem).view(1, -1)
        topv, topi = decoder_output.topk(1)

        loss += criterion(decoder_output, target_tensor[di])
        if target_tensor[di].item() == dictionary.Dictionary.EOS_token:
            break

        decoded_words.append(_dict.index2word[topi.item()])
        decoder_input = target_tensor[di]

    encoder_optimizer.zero_grad()
    decoder_optimizer.zero_grad()
    loss /= target_length
    loss.backward()
    torch.nn.utils.clip_grad_norm_(encoder.parameters(), clipping_theta)
    torch.nn.utils.clip_grad_norm_(decoder.parameters(), clipping_theta)
    encoder_optimizer.step()
    decoder_optimizer.step()

    if (iter + 1) % 10 == 0:
        print('loss:%f' % loss)
        print(''.join([i for i in decoded_words]))

def _pretrain(encoder, decoder, _dict, _data, n_iters=200, batch_size=32, learning_rate=0.00001, clipping_theta=0.001):
    loss = nn.CrossEntropyLoss()

    optimizer_encoder = optim.Adam(encoder.parameters(), lr=learning_rate)
    optimizer_decoder = optim.Adam(decoder.parameters(), lr=learning_rate)

    training_datas = []
    for data in _data:
        training_datas.append(data)

    for iter in range(n_iters):
        for _train_data in training_datas:
            _data_iter = seq2seq.data_iter_consecutive_sentence(_train_data, batch_size)
            for X, Y in _data_iter:
                (input_tensor, target_tensor) = seq2seq.tensorsFromPair(_dict, [X, Y])
                train(iter, input_tensor, target_tensor, encoder, decoder, optimizer_encoder, optimizer_decoder, loss, clipping_theta)

        if (iter + 1) % 10 == 0:
            torch.save(encoder, 'pretrain_transformer_encoder.pkl')
            torch.save(decoder, 'pretrain_transformer_decoder.pkl')

    torch.save(encoder, 'pretrain_transformer_encoder.pkl')
    torch.save(decoder, 'pretrain_transformer_decoder.pkl')

def trainIters(_dict, pairs, n_iters=20, learning_rate=0.01, clipping_theta=1e-2):
    encoder = torch.load('pretrain_transformer_encoder.pkl')
    decoder = torch.load('pretrain_transformer_decoder.pkl')

    encoder_optimizer = optim.SGD(encoder.parameters(), lr=learning_rate)
    decoder_optimizer = optim.SGD(decoder.parameters(), lr=learning_rate)

    training_pairs = [seq2seq.tensorsFromPair(_dict, random.choice(pairs)) for i in range(n_iters)]

    criterion = nn.CrossEntropyLoss()

    for iter in range(n_iters):
        training_pair = training_pairs[iter]
        input_tensor = training_pair[0]
        target_tensor = training_pair[1]

        train(input_tensor, target_tensor, encoder, decoder, encoder_optimizer, decoder_optimizer, criterion, clipping_theta)

    torch.save(encoder, 'transformer_encoder.pkl')
    torch.save(decoder, 'transformer_decoder.pkl')

def evaluate(encoder, decoder, _dict, sentence):
    with torch.no_grad():
        input_tensor = seq2seq.tensorFromSentence(_dict, sentence)

        decoded_words = []
        mem = encoder(input_tensor)
        decoder_input = torch.tensor([[dictionary.Dictionary.SOS_token]])
        for i in range(63):
            decoder_output = decoder(decoder_input, mem).view(1, -1)
            topv, topi = decoder_output.topk(1)
            if topi.item() == dictionary.Dictionary.EOS_token:
                break
            else:
                decoded_words.append(_dict.index2word[topi.item()])
            decoder_input = topi

        return ''.join([i for i in decoded_words])

_dict = dictionary.load_dict()
ntoken, ninp, nhead, nlayers = _dict.n_words, 256, 8, 8
_encoder = TransformerEncoderModel(ntoken, ninp, nhead, nlayers).cuda()
_decoder = TransformerDecoderModel(ntoken, ninp, nhead, nlayers).cuda()
#_encoder = torch.load('pretrain_transformer_encoder.pkl')
#_decoder = torch.load('pretrain_transformer_decoder.pkl')
_data = pretrain.load_data('wuxia.duanpian.zip')
_pretrain(_encoder, _decoder, _dict, _data, n_iters=200)

#_pairs = seq2seq.prepareData()
#trainIters(_dict, _pairs)

#encoder = torch.load('pretrain_transformer_encoder.pkl')
#decoder = torch.load('pretrain_transformer_decoder.pkl')
#print(evaluate(encoder, decoder, _dict, "小千千手持宝剑倒立于天河尽头吃翔。"))
