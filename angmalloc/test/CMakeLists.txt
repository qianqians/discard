PROJECT(ang_profile_test)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

set(angmalloc_include_path ${PROJECT_SOURCE_DIR}//..// CACHE PATH "angmalloc path")
INCLUDE_DIRECTORIES(${angmalloc_include_path})

add_executable(ang_profile_test main.cpp)
target_link_libraries(ang_profile_test angmalloc)