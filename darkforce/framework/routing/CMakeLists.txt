PROJECT(routing)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB routing_head_file ./ ./*.h)

set(routing_src
    ./routing.cpp
)
ADD_LIBRARY(routing ${routing_src} ${routing_head_file})