PROJECT(bingate)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB logic_head_file ./ ./*.h)

set(logic_src
    ./main.cpp
)
add_executable(bingate ${logic_src} ${logic_head_file})
target_link_libraries(bingate gate)
target_link_libraries(bingate remoteq_tcp)
target_link_libraries(bingate acceptor)
target_link_libraries(bingate achieve)
target_link_libraries(bingate pool)
target_link_libraries(bingate timer)
target_link_libraries(bingate juggle)
target_link_libraries(bingate context)
target_link_libraries(bingate jsonconfig)
target_link_libraries(bingate fitle)