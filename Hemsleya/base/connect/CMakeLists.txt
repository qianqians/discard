PROJECT(connect)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB source_files 
     async_net/*.cpp
     async_net/*.h
     async_net/windows/*.cpp
     async_net/windows/*.h
     async_net/exception/*.cpp
     async_net/exception/*.h  
)

source_group(async_net FILES ${source_files})

AUX_SOURCE_DIRECTORY(. DIR_SRC)

SET(header_files
    session.h
    connservice.h
    adapter.h
    address.h
)

SET(SRC_LIST 
    ${DIR_SRC}
)

ADD_LIBRARY(connect ${SRC_LIST} ${header_files})