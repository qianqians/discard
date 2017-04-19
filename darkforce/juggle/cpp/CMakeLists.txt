PROJECT(juggle)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

set(BOOST_INCLUDE_PATH  ${PROJECT_SOURCE_DIR}//..//..//third_party//boost CACHE PATH "boost include path")
INCLUDE_DIRECTORIES(${BOOST_INCLUDE_PATH})

set(BOOST_LIBS_PATH ${PROJECT_SOURCE_DIR}//..//..//third_party//boost//lib CACHE PATH "boost include path")
link_directories(${BOOST_LIBS_PATH})

set(Fossilizid_include_path ${PROJECT_SOURCE_DIR}//..//..//fossilizid CACHE PATH "Fossilizid include path")
INCLUDE_DIRECTORIES(${Fossilizid_include_path})
MESSAGE(STATUS "This is include dir " ${Fossilizid_include_path}) 

file(GLOB interface_head_file ./interface ./interface/*.h)
file(GLOB service_head_file ./service ./service/*.h)

AUX_SOURCE_DIRECTORY(./service service_SRC)

SET(header_files
    ${interface_head_file}
    ${service_head_file}
)


SET(SRC_LIST 
    ${service_SRC}
)

ADD_LIBRARY(juggle ${SRC_LIST} ${header_files})
target_link_libraries(juggle uuid)