PROJECT(tools)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

AUX_SOURCE_DIRECTORY(. DIR_SRC)

SET(header_files
    strtools.h
    tools.h
)

SET(SRC_LIST 
    ${DIR_SRC}
)

ADD_LIBRARY(tools ${SRC_LIST} ${header_files})