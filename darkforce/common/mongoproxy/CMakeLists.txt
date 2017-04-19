PROJECT(mongoproxy)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 



set(MONGOC_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//..//third_party//libmongoc//include//libmongoc-1.0)

INCLUDE_DIRECTORIES(${MONGOC_INCLUDE_PATH})


set(BSON_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//..//third_party//libbson//include//libbson-1.0)

INCLUDE_DIRECTORIES(${BSON_INCLUDE_PATH})

file(GLOB mongoproxy_head_file ./ ./*.h)


AUX_SOURCE_DIRECTORY(./ mongoproxy_src)



ADD_LIBRARY(mongoproxy ${mongoproxy_src} ${mongoproxy_head_file})