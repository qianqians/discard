PROJECT(center)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB logic_head_file ./ ./*.h)

set(logic_src
    ./center.cpp
)
ADD_LIBRARY(center ${logic_src} ${logic_head_file})