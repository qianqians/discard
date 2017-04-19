PROJECT(Fossilizid)
CMAKE_MINIMUM_REQUIRED(VERSION 2.8) 
 
ADD_DEFINITIONS(-D_CRT_SECURE_NO_WARNINGS)
ADD_DEFINITIONS(-D_WINSOCK_DEPRECATED_NO_WARNINGS)
ADD_DEFINITIONS(-D___mempool___)

set(BOOST_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//third_party//boost CACHE PATH "Fossilizid include path")
INCLUDE_DIRECTORIES(${BOOST_INCLUDE_PATH})

set(BOOST_LIBS_PATH ${PROJECT_SOURCE_DIR}//../third_party//boost//lib CACHE PATH "Fossilizid include path")
link_directories(${BOOST_LIBS_PATH})

set(Fossilizid_include_path ${PROJECT_SOURCE_DIR} CACHE PATH "Fossilizid include path")
INCLUDE_DIRECTORIES(${Fossilizid_include_path})
MESSAGE(STATUS "This is include dir " ${Fossilizid_include_path}) 

set(Fossilizid_lib_path ${PROJECT_BINARY_DIR}/bin/Debug CACHE PATH "Fossilizid lib path")
link_directories(${Fossilizid_lib_path})
MESSAGE(STATUS "This is lib dir " ${Fossilizid_lib_path}) 

ADD_SUBDIRECTORY(pool ${Fossilizid_bin_path}/pool)
ADD_SUBDIRECTORY(container ${Fossilizid_bin_path}/container)
ADD_SUBDIRECTORY(context ${Fossilizid_bin_path}/context)
ADD_SUBDIRECTORY(uuid ${Fossilizid_bin_path}/uuid)