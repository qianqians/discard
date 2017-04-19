PROJECT(logic)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB logic_head_file ./ ./*.h)

set(logic_src
    ./logic.cpp
)
ADD_LIBRARY(logic ${logic_src} ${logic_head_file})