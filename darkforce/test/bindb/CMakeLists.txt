PROJECT(bindb)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 


set(MONGOC_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//..//third_party//libmongoc//include//libmongoc-1.0)

INCLUDE_DIRECTORIES(${MONGOC_INCLUDE_PATH})


set(BSON_INCLUDE_PATH ${PROJECT_SOURCE_DIR}//..//..//third_party//libbson//include//libbson-1.0)

INCLUDE_DIRECTORIES(${BSON_INCLUDE_PATH})

file(GLOB mongoproxy_head_file ./ ./*.h)



link_directories(C:\Users\qianqians\Documents\workspace\DarkForce\darkforce\third_party\libbson\lib)
link_directories(C:\Users\qianqians\Documents\workspace\DarkForce\darkforce\third_party\libmongoc\lib)

file(GLOB logic_head_file ./ ./*.h)

set(logic_src
    ./main.cpp
)
add_executable(bindb ${logic_src} ${logic_head_file})
target_link_libraries(bindb dbproxy)
target_link_libraries(bindb mongoproxy)
target_link_libraries(bindb remoteq_tcp)
target_link_libraries(bindb acceptor)
target_link_libraries(bindb achieve)
target_link_libraries(bindb pool)
target_link_libraries(bindb timer)
target_link_libraries(bindb juggle)
target_link_libraries(bindb context)
target_link_libraries(bindb jsonconfig)
target_link_libraries(bindb fitle)
target_link_libraries(bindb bson-static-1.0)
target_link_libraries(bindb mongoc-static-1.0)