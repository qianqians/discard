import io
import sys

sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

import random
import zipfile

import torch
import torch.nn as nn
from torch import optim

import dictionary
import utils


class PretrainLSTM(nn.Module):
    def __init__(self, input_size, hidden_size, layer_num, embedding, lstm):
        super(PretrainLSTM, self).__init__()

        self.hidden_size = hidden_size
        self.layer_num = layer_num
        self.embedding = embedding
        self.out = nn.Linear(hidden_size, input_size)
        self.lstm = lstm

    def forward(self, input, hidden):
        embedded = self.embedding(input)
        output, hidden = self.lstm(embedded, hidden)
        output = self.out(output.view(-1, output.shape[-1]))
        return output, hidden

    def initHidden(self):
        return (torch.zeros(self.layer_num, 1, self.hidden_size), torch.zeros(self.layer_num, 1, self.hidden_size))


def indexesFromSentence(_dict, sentence):
    return [_dict.word2index[word] for word in sentence]


def tensorFromSentence(_dict, sentence):
    indexes = indexesFromSentence(_dict, sentence)
    return torch.tensor(indexes, dtype=torch.long).view(-1, 1)


def data_iter_consecutive(_data, batch_size):
    data_len = len(_data)
    batch_len = data_len // batch_size
    for i in range(batch_len):
        i = i * batch_size
        X = _data[i: i + batch_size]
        Y = _data[i + 1: i + batch_size + 1]
        yield X, Y


def pretrain(model, _dict, _data, n_iters=2000, batch_size=256, learning_rate=0.01, clipping_theta=1e-2):
    loss = nn.CrossEntropyLoss()
    optimizer = optim.SGD(model.parameters(), lr=learning_rate)

    training_datas = [random.choice(_data) for i in range(n_iters)]
    for iter in range(n_iters):
        _train_data = training_datas[iter]
        _data_iter = data_iter_consecutive(_train_data, batch_size)

        hidden = model.initHidden()
        for X, Y in _data_iter:
            X = tensorFromSentence(_dict, X)
            Y = tensorFromSentence(_dict, Y)

            hidden = (hidden[0].detach(), hidden[1].detach())
            output, hidden = model(X, hidden)

            y = torch.transpose(Y, 0, 1).contiguous().view(-1)
            l = loss(output, y.long())

            optimizer.zero_grad()
            l.backward()
            # 梯度裁剪
            utils.grad_clipping(model.parameters(), clipping_theta)
            optimizer.step()


def load_data(file_name):
    _data = []
    with zipfile.ZipFile(file_name) as zin:
        for f in zin.namelist():
            with zin.open(f) as f:
                corpus_chars = f.read().decode('utf-8')
                f.close()
                corpus_chars = corpus_chars.replace('\n', '').replace('\r', '').replace(' ', '')
                _data.append(corpus_chars)
    return _data


#_dict = dictionary.load_dict()
#_data = load_data()
#hidden_size, layer_num = 1024, 12
#lstm = nn.LSTM(hidden_size, hidden_size, layer_num)
#embedding = nn.Embedding(_dict.n_words, hidden_size)
#model = PretrainLSTM(_dict.n_words, hidden_size, layer_num, embedding, lstm)
#pretrain(model, _dict, _data)
#torch.save(embedding, 'pretrain_embedding.pkl')
#torch.save(lstm, 'pretrain_lstm.pkl')
