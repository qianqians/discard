PROJECT(config)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB config_head_file ./ ./*.h)

set(jsonconfig_src
    ./jsonconfig.cpp
)
ADD_LIBRARY(jsonconfig ${jsonconfig_src} ${config_head_file})

#set(xmlconfig_src
#    ./xmlconfig.cpp
#)
#ADD_LIBRARY(xmlconfig_src ${jsonconfig_src} ${config_head_file})