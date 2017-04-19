PROJECT(test)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

set(jsonparse_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//jsonparse//cpp CACHE PATH "jsonparseinclude path")
INCLUDE_DIRECTORIES(${jsonparse_INCLUDE_PATH})

set(fossilizid_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//fossilizid CACHE PATH "Fossilizid include path")
INCLUDE_DIRECTORIES(${fossilizid_INCLUDE_PATH})

set(jugglescript_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//framework//jugglescript//cpp CACHE PATH "jugglescript INCLUDE path")
INCLUDE_DIRECTORIES(${jugglescript_INCLUDE_PATH})

set(juggle_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//juggle//cpp//interface CACHE PATH "juggle include path")
INCLUDE_DIRECTORIES(${juggle_INCLUDE_PATH})

set(remoteq_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//remoteq CACHE PATH "remoteq include path")
INCLUDE_DIRECTORIES(${remoteq_INCLUDE_PATH})

set(common_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//common CACHE PATH "common INCLUDE path")
INCLUDE_DIRECTORIES(${common_INCLUDE_PATH})

set(BOOST_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//third_party//boost CACHE PATH "boost include path")
INCLUDE_DIRECTORIES(${BOOST_INCLUDE_PATH})

link_directories(${PROJECT_SOURCE_DIR}//..//third_party//boost//lib)
link_directories(C:\Users\qianqians\Documents\workspace\DarkForce\build\darkforce\bin\Debug)

IF(WIN32)
	SET(CMAKE_EXE_LINKER_FLAGS "/force")
endif()

ADD_SUBDIRECTORY(bincenter)
ADD_SUBDIRECTORY(bindb)
ADD_SUBDIRECTORY(bingate)
ADD_SUBDIRECTORY(binlogic)