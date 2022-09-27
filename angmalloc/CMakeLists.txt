PROJECT(angmalloc)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB def_header_files ./ ./*.h)

AUX_SOURCE_DIRECTORY(. DIR_SRC)

SET(SRC_LIST 
    ${DIR_SRC}
)

add_library(angmalloc STATIC ${def_header_files} ${SRC_LIST})

add_subdirectory(test)