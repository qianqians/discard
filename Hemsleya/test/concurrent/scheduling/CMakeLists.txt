PROJECT(test_scheduling)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

AUX_SOURCE_DIRECTORY(. DIR_SRC)

SET(header_files
)

ADD_executable(test_scheduling ${DIR_SRC} ${header_files})
target_link_libraries(test_scheduling scheduling)