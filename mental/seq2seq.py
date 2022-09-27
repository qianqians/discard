import math
import time
import json
import random

import torch
import torch.nn as nn
from torch import optim
import torch.nn.functional as F

from snownlp import SnowNLP

import dictionary
import utils
import pretrain

def read_story():
    # Read the file and split into lines
    #lines = open('data.txt', encoding='utf-8').read()

    import data
    # 数据是JSON串
    json_data = data._data

    # Split every line into pairs and normalize
    pairs = [[s.replace('\n', '').replace('\r', '').replace(' ', '') for s in l] for l in json_data]

    return pairs

def prepareData():
    pairs = read_story()
    #print("Read %s sentence pairs" % len(pairs))

    #print("Trimmed to %s sentence pairs" % len(pairs))
    #print("Counting words...")
    #for pair in pairs:
    #    _dict.addSentence(pair[0])
    #    _dict.addSentence(pair[1])
    #print("Counted words:", _dict.n_words)

    return pairs

class EncoderRNN(nn.Module):
    def __init__(self, input_size, hidden_size, layer_num):
        super(EncoderRNN, self).__init__()
        self.hidden_size = hidden_size
        self.layer_num = layer_num

        self.embedding = nn.Embedding(input_size, hidden_size).cuda()
        self.model = nn.LSTM(hidden_size, hidden_size, layer_num, bidirectional=True).cuda()

    def forward(self, input, hidden):
        embedded = self.embedding(input).view(-1, 1, self.hidden_size)
        output = embedded
        output, hidden = self.model(output, hidden)
        return output, hidden

    def initHidden(self):
        return (torch.zeros(self.layer_num*2, 1, self.hidden_size, device=torch.device('cuda')), torch.zeros(self.layer_num*2, 1, self.hidden_size, device=torch.device('cuda')))

class DecoderRNN(nn.Module):
    def __init__(self, hidden_size, output_size, layer_num):
        super(DecoderRNN, self).__init__()
        self.hidden_size = hidden_size

        self.embedding = nn.Embedding(output_size, hidden_size).cuda()
        self.model = nn.LSTM(hidden_size, hidden_size, layer_num, bidirectional=True).cuda()
        self.out = nn.Linear(hidden_size*2, output_size).cuda()
        self.softmax = nn.LogSoftmax(dim=1).cuda()

    def forward(self, input, hidden):
        output = self.embedding(input).view(-1, 1, self.hidden_size)
        output = F.relu(output)
        output, hidden = self.model(output, hidden)
        output = self.out(output.view(-1, output.shape[-1]))
        output = self.softmax(output)
        return output, hidden

def indexesFromSentence(_dict, sentence):
    return [_dict.word2index[word] for word in sentence]

def tensorFromSentence(_dict, sentence):
    indexes = indexesFromSentence(_dict, sentence)
    indexes.append(dictionary.Dictionary.EOS_token)
    return torch.tensor(indexes, dtype=torch.long, device=torch.device('cuda')).view(-1, 1)

def tensorsFromPair(_dict, pair):
    input_tensor = tensorFromSentence(_dict, pair[0])
    target_tensor = tensorFromSentence(_dict, pair[1])
    return (input_tensor, target_tensor)

def train(_dict, input_tensor, target_tensor, encoder, decoder, encoder_hidden, encoder_optimizer, decoder_optimizer, criterion, clipping_theta):
    encoder_output, encoder_hidden = encoder(input_tensor, encoder_hidden)

    loss = 0
    decoded_words = []
    target_length = target_tensor.size(0)
    decoder_input = torch.tensor([[dictionary.Dictionary.SOS_token]], device=torch.device('cuda'))
    for i in range(target_length):
        decoder_output, decoder_hidden = decoder(decoder_input, encoder_hidden)
        topv, topi = decoder_output.topk(1)

        loss += criterion(decoder_output, target_tensor[i])
        if target_tensor[i].item() == dictionary.Dictionary.EOS_token:
            break

        decoded_words.append(_dict.index2word[topi.item()])
        decoder_input = target_tensor[i]

    encoder_optimizer.zero_grad()
    decoder_optimizer.zero_grad()
    loss /= target_length
    loss.backward()
    torch.nn.utils.clip_grad_norm_(encoder.parameters(), clipping_theta)
    torch.nn.utils.clip_grad_norm_(decoder.parameters(), clipping_theta)
    encoder_optimizer.step()
    decoder_optimizer.step()

    print('loss:%f target_length:%d decoded_words_len:%d' % (loss, target_length, len(decoded_words)))
    print(''.join([i for i in decoded_words]))

def trainIters(n_iters, encoder, decoder, _dict, pairs, learning_rate=0.01, clipping_theta = 1e-2):
    encoder_optimizer = optim.Adam(encoder.parameters(), lr=learning_rate)
    decoder_optimizer = optim.Adam(decoder.parameters(), lr=learning_rate)

    training_pairs = [tensorsFromPair(_dict, random.choice(pairs)) for i in range(n_iters)]

    criterion = nn.NLLLoss()

    for iter in range(n_iters):
        training_pair = training_pairs[iter]
        input_tensor = training_pair[0]
        target_tensor = training_pair[1]

        train(input_tensor, target_tensor, encoder, decoder, encoder_optimizer, decoder_optimizer, criterion, clipping_theta)

    torch.save(encoder, 'encoder.pkl')
    torch.save(decoder, 'decoder.pkl')

def evaluate(encoder, decoder, _dict, sentence):
    with torch.no_grad():
        input_tensor = tensorFromSentence(_dict, sentence)
        encoder_hidden = encoder.initHidden()
        encoder_output, encoder_hidden = encoder(input_tensor, encoder_hidden)
        decoder_hidden = encoder_hidden

        decoded_words = []
        decoder_input = torch.tensor([[dictionary.Dictionary.SOS_token]], device=torch.device('cuda'))
        for i in range(32):
            decoder_output, decoder_hidden = decoder(decoder_input, decoder_hidden)
            topv, topi = decoder_output.topk(1)
            if topi.item() != dictionary.Dictionary.EOS_token:
                decoded_words.append(_dict.index2word[topi.item()])
            decoder_input = topi

        return ''.join([i for i in decoded_words])

def data_iter_consecutive_sentence(_data, batch_size):
    data_len = len(_data)
    start = 0
    while start < data_len:
        end = start + batch_size
        for i in range(start, data_len):
            if _data[i] == "。" or _data[i] == "！":
                end = i
                break
        X = _data[start: end + 1]
        start = end + 1
        for i in range(start, data_len):
            if _data[i] == "。" or _data[i] == "！":
                end = i
                break
        Y = _data[start: end + 1]
        yield X, Y

def data_iter_consecutive_world(_data, batch_size):
    data_len = len(_data)
    epoch_size = (data_len - 1) // batch_size
    for i in range(epoch_size):
        i *= batch_size
        X = _data[i: i + batch_size]
        Y = _data[i + 1: i + batch_size + 1]
        yield X, Y

def _pretrain(encoder, decoder, _dict, _data, data_iter_consecutive, n_iters=240, batch_size=32, learning_rate=0.00001, clipping_theta=0.01):
    encoder_optimizer = optim.Adam(encoder.parameters(), lr=learning_rate)
    decoder_optimizer = optim.Adam(decoder.parameters(), lr=learning_rate)

    training_datas = []
    for data in _data:
        training_datas.append(data)

    criterion = nn.CrossEntropyLoss()
    for i in range(n_iters):
        for _train_data in training_datas:
            encoder_hidden = encoder.initHidden()
            _data_iter = data_iter_consecutive(_train_data, batch_size)
            for X, Y in _data_iter:
                (input_tensor, target_tensor) = tensorsFromPair(_dict, [X, Y])
                train(_dict, input_tensor, target_tensor, encoder, decoder, encoder_hidden, encoder_optimizer, decoder_optimizer, criterion, clipping_theta)

        if (i + 1) % 10 == 0:
            torch.save(encoder, 'pretrain_encoder.pkl')
            torch.save(decoder, 'pretrain_decoder.pkl')

    torch.save(encoder, 'pretrain_encoder.pkl')
    torch.save(decoder, 'pretrain_decoder.pkl')

_dict = dictionary.load_dict()
_data = pretrain.load_data('wuxia.duanpian.zip')

#_hidden_size = 256
#_encoder = EncoderRNN(_dict.n_words, _hidden_size, 6).cuda()
#_decoder = DecoderRNN(_hidden_size, _dict.n_words, 6).cuda()
_encoder = torch.load('pretrain_encoder.pkl')
_decoder = torch.load('pretrain_decoder.pkl')
_pretrain(_encoder, _decoder, _dict, _data, data_iter_consecutive_sentence)

#_pairs = prepareData(_dict)
#_encoder = torch.load('pretrain_encoder.pkl')
#_decoder = torch.load('pretrain_decoder.pkl')
#trainIters(100, _encoder, _decoder, _dict, _pairs)

#encoder = torch.load('pretrain_encoder.pkl')
#decoder = torch.load('pretrain_decoder.pkl')
#print(evaluate(encoder, decoder, _dict, "小千千手持长剑倒立于天河尽头吃翔。"))

