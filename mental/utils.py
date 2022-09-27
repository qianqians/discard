import torch

def grad_clipping(params, theta):
    norm = torch.tensor([0.0])
    for param in params:
        if param.grad is None:
            continue
        norm += (param.grad.data ** 2).sum()
    norm = norm.sqrt().item()
    if norm > theta:
        for param in params:
            if param.grad is None:
                continue
            param.grad.data *= (theta / norm)