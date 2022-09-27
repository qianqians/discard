import zipfile

import data

class Dictionary:
    SOS_token = 0
    EOS_token = 1

    def __init__(self):
        self.word2index = {"S": Dictionary.SOS_token, "E": Dictionary.EOS_token}
        self.word2count = {"S": 1, "E": 1}
        self.index2word = {Dictionary.SOS_token: "S", Dictionary.EOS_token: "E"}
        self.n_words = 2  # Count SOS and EOS

        self.words = 'SE'

    def addSentence(self, sentence):
        for word in sentence:
            self.addWord(word)

    def addWord(self, word):
        if word not in self.word2index:
            self.word2index[word] = self.n_words
            self.word2count[word] = 1
            self.index2word[self.n_words] = word
            self.n_words += 1
            self.words += word
        else:
            self.word2count[word] += 1

def init_dict():
    _dict = Dictionary()
    with zipfile.ZipFile('wuxia.txt.zip') as zin:
        for f in zin.namelist():
            with zin.open(f) as f:
                corpus_chars = f.read().decode('utf-8')
                corpus_chars = corpus_chars.replace('\n', ' ').replace('\r', ' ')
                _dict.addSentence(corpus_chars)

    # 数据是JSON串
    json_data = data._data
    # Split every line into pairs and normalize
    pairs = [[s for s in l] for l in json_data]
    for pair in pairs:
        _dict.addSentence(pair[0])
        _dict.addSentence(pair[1])

    f = open("dict.txt", mode='w', encoding='utf-8')
    f.write(_dict.words)
    f.close()

def load_dict():
    f = open("dict.txt", encoding='utf-8')
    dict_str = f.read()
    f.close()

    _dict = Dictionary()
    _dict.addSentence(dict_str)

    return _dict

#init_dict()


