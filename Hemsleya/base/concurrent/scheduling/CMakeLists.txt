PROJECT(scheduling)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

AUX_SOURCE_DIRECTORY(. DIR_SRC)
AUX_SOURCE_DIRECTORY(.//detail detail_DIR_SRC)
AUX_SOURCE_DIRECTORY(.//exception exception_DIR_SRC)

SET(header_files
    wait_any.h
)

SET(SRC_LIST 
    ${DIR_SRC}
    ${detail_DIR_SRC}
    ${exception_DIR_SRC}
)

ADD_LIBRARY(scheduling ${SRC_LIST} ${header_files})
target_link_libraries(scheduling exception)
target_link_libraries(scheduling tools)
target_link_libraries(scheduling timer)
