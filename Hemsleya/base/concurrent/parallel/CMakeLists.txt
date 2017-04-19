PROJECT(parallel)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

AUX_SOURCE_DIRECTORY(. DIR_SRC)

SET(header_files
    parallel_sort.h
)

SET(SRC_LIST 
    ${DIR_SRC}
)

ADD_LIBRARY(parallel ${SRC_LIST} ${header_files})
target_link_libraries(parallel tools)