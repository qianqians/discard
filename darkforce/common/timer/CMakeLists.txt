PROJECT(timer)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB timer_head_file ./ ./*.h)

set(timer_src
    ./timerservice.cpp
)

ADD_LIBRARY(timer ${timer_src} ${timer_head_file})