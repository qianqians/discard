PROJECT(logicservice)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB logicservice_head_file ./ ./*.h)

set(logicservice_src
    ./logicservice.cpp
)

ADD_LIBRARY(logicservice ${logicservice_src} ${logicservice_head_file})