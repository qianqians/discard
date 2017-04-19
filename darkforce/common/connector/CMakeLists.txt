PROJECT(connector)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB connector_head_file ./ ./*.h)

set(connector_src
    ./connector.cpp
)

ADD_LIBRARY(connector ${connector_src} ${connector_head_file})