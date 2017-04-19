PROJECT(bincenter)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB logic_head_file ./ ./*.h)

set(logic_src
    ./main.cpp
)

add_executable(bincenter ${logic_src} ${logic_head_file})
target_link_libraries(bincenter center)
target_link_libraries(bincenter remoteq_tcp)
target_link_libraries(bincenter acceptor)
target_link_libraries(bincenter achieve)
target_link_libraries(bincenter pool)
target_link_libraries(bincenter timer)
target_link_libraries(bincenter juggle)
target_link_libraries(bincenter context)
target_link_libraries(bincenter jsonconfig)
target_link_libraries(bincenter fitle)
