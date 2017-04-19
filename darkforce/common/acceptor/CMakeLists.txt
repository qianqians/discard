PROJECT(acceptor)

CMAKE_MINIMUM_REQUIRED(VERSION 2.6)



file(GLOB acceptor_head_file ./ ./*.h)

AUX_SOURCE_DIRECTORY(./ acceptor_src)



ADD_LIBRARY(acceptor ${acceptor_src} ${acceptor_head_file})