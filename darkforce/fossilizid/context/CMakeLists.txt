PROJECT(context)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

AUX_SOURCE_DIRECTORY(. DIR_SRC)

SET(header_files 
    context.h
)

SET(SRC_LIST 
    ${DIR_SRC}
)

ADD_LIBRARY(context ${SRC_LIST} ${header_files})