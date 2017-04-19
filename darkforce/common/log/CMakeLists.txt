PROJECT(log)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB log_head_file ./ ./*.h)

set(consolelog_src
    ./consolelog.cpp
)
ADD_LIBRARY(consolelog ${consolelog_src} ${log_head_file})

set(filelog_src
    ./filelog.cpp
)
ADD_LIBRARY(fileloglog ${filelog_src} ${log_head_file})