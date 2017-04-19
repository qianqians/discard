PROJECT(pool)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

AUX_SOURCE_DIRECTORY(. DIR_SRC)

SET(header_files
    angmalloc.h
    chunk.h
    mempage_heap.h
    mirco_mempage_heap.h
)

SET(SRC_LIST 
    ${DIR_SRC}
)

add_executable(pool ${SRC_LIST} ${header_files})