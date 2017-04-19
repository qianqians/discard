PROJECT(uuid)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

AUX_SOURCE_DIRECTORY(. DIR_SRC)
AUX_SOURCE_DIRECTORY(./windows windows_DIR_SRC)

SET(header_files
    uuid.h
)

SET(SRC_LIST 
    ${DIR_SRC}
    ${windows_DIR_SRC}
)

ADD_LIBRARY(uuid ${SRC_LIST} ${header_files})