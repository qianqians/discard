import math
import time
import zipfile
import numpy as np

import torch
from torch import nn

def _one(shape):
    ts = torch.tensor(np.random.normal(0, 0.01, size=shape), dtype=torch.float32)
    return torch.nn.Parameter(ts, requires_grad=True)

def _three(num_inputs, num_hiddens):
    return (_one((num_inputs, num_hiddens)),
            _one((num_hiddens, num_hiddens)),
            torch.nn.Parameter(torch.zeros(num_hiddens, dtype=torch.float32), requires_grad=True))

def get_supplement_params(num_inputs, num_hiddens, num_outputs):
    W_xi, W_hi, b_i = _three(num_inputs, num_hiddens)
    W_xf, W_hf, b_f = _three(num_inputs, num_hiddens)
    W_xo, W_ho, b_o = _three(num_inputs, num_hiddens)
    W_xc, W_hc, b_c = _three(num_inputs, num_hiddens)
    return nn.ParameterList([W_xi, W_hi, b_i, W_xf, W_hf, b_f, W_xo, W_ho, b_o, W_xc, W_hc, b_c])

def get_alchemy_params(num_inputs, num_hiddens, num_outputs):
    W_xi, W_hi, b_i = _three(num_inputs, num_hiddens)
    W_xf, W_hf, b_f = _three(num_inputs, num_hiddens)
    W_xo, W_ho, b_o = _three(num_inputs, num_hiddens)
    W_xc, W_hc, b_c = _three(num_inputs, num_hiddens)
    W_hq = _one((num_hiddens, num_outputs))
    b_q = torch.nn.Parameter(torch.zeros(num_outputs, dtype=torch.float32), requires_grad=True)
    return nn.ParameterList([W_xi, W_hi, b_i, W_xf, W_hf, b_f, W_xo, W_ho, b_o, W_xc, W_hc, b_c, W_hq, b_q])

def init_alchemy_state(batch_size, num_hiddens):
    return (torch.zeros((batch_size, num_hiddens)),
            torch.zeros((batch_size, num_hiddens)))

def Supplement(num_hiddens, inputs, supplement_params):
    [W_xi, W_hi, b_i, W_xf, W_hf, b_f, W_xo, W_ho, b_o, W_xc, W_hc, b_c] = supplement_params
    (H, C) = init_alchemy_state(inputs.shape[1], num_hiddens)
    for X in inputs:
        I = torch.sigmoid(torch.matmul(X, W_xi) + torch.matmul(H, W_hi) + b_i)
        F = torch.sigmoid(torch.matmul(X, W_xf) + torch.matmul(H, W_hf) + b_f)
        O = torch.sigmoid(torch.matmul(X, W_xo) + torch.matmul(H, W_ho) + b_o)
        C_tilda = torch.tanh(torch.matmul(X, W_xc) + torch.matmul(H, W_hc) + b_c)
        C = F * C + I * C_tilda
        H = O * C.tanh()
    return (H, C)

def Determine():

    pass

def Alchemy(inputs, supplement_params, alchemy_params):



    pass