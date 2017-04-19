PROJECT(Hemsleya)
CMAKE_MINIMUM_REQUIRED(VERSION 2.8) 

find_package(Boost)
if (Boost_FOUND)
	link_directories(${Boost_INCLUDE_DIR}//stage//lib)
	INCLUDE_DIRECTORIES(${Boost_INCLUDE_DIR})
endif (Boost_FOUND)

MESSAGE(STATUS "This is BOOST lib dir " ${Boost_LIBRARIES})  
MESSAGE(STATUS "This is BOOST include dir " ${Boost_INCLUDE_DIR})  

MESSAGE(STATUS "This is BINARY dir " ${PROJECT_BINARY_DIR})  
MESSAGE(STATUS "This is SOURCE dir " ${PROJECT_SOURCE_DIR})  

set(Hemsleya_include_path ${PROJECT_SOURCE_DIR}//.. CACHE PATH "Hemsleya include path")
INCLUDE_DIRECTORIES(${Hemsleya_include_path})
MESSAGE(STATUS "This is include dir " ${Hemsleya_include_path}) 

set(Hemsleya_lib_path ${PROJECT_BINARY_DIR}/bin/Debug CACHE PATH "Hemsleya lib path")
link_directories(${Hemsleya_lib_path})
MESSAGE(STATUS "This is lib dir " ${Hemsleya_lib_path}) 

set(Hemsleya_bin_path ${PROJECT_BINARY_DIR}/bin CACHE PATH "Hemsleya bin path")

SET(EXECUTABLE_OUTPUT_PATH ${PROJECT_BINARY_DIR}/bin)
SET(LIBRARY_OUTPUT_PATH ${PROJECT_BINARY_DIR}/bin)

ADD_SUBDIRECTORY(base ${Hemsleya_bin_path})
ADD_SUBDIRECTORY(test ${Hemsleya_bin_path}//test)