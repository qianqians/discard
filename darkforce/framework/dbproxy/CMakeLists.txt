PROJECT(dbproxy)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 


set(MONGOC_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//..//third_party//libmongoc//include//libmongoc-1.0)

INCLUDE_DIRECTORIES(${MONGOC_INCLUDE_PATH})


set(BSON_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//..//third_party//libbson//include//libbson-1.0)

INCLUDE_DIRECTORIES(${BSON_INCLUDE_PATH})

file(GLOB mongoproxy_head_file ./ ./*.h)


file(GLOB db_head_file ./ ./*.h)

set(db_src
    ./dbproxy.cpp
)
ADD_LIBRARY(dbproxy ${db_src} ${db_head_file})