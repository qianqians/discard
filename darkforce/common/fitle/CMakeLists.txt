PROJECT(fitle)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB fitle_head_file ./ ./*.h)
AUX_SOURCE_DIRECTORY(./ fitle_src)

ADD_LIBRARY(fitle ${fitle_src} ${fitle_head_file})