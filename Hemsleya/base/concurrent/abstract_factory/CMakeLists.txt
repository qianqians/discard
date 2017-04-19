PROJECT(abstract_factory)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

AUX_SOURCE_DIRECTORY(. DIR_SRC)
AUX_SOURCE_DIRECTORY(detail DIR_SRC_detail)

SET(header_files
    abstract_factory.h
)

SET(SRC_LIST 
    ${DIR_SRC}
    ${DIR_SRC_detail}
)

ADD_LIBRARY(abstract_factory ${SRC_LIST} ${header_files})
target_link_libraries(abstract_factory tools)