PROJECT(gate)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB gate_head_file ./ ./*.h)

set(gate_src
    ./gate.cpp
    ./rpcproxy.cpp
)
ADD_LIBRARY(gate ${gate_src} ${gate_head_file})