PROJECT(binlogic)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB logic_head_file ./ ./*.h)

set(logic_src
    ./main.cpp
)
add_executable(binlogic ${logic_src} ${logic_head_file})
target_link_libraries(binlogic logic)
target_link_libraries(binlogic remoteq_tcp)
target_link_libraries(binlogic acceptor)
target_link_libraries(binlogic achieve)
target_link_libraries(binlogic pool)
target_link_libraries(binlogic timer)
target_link_libraries(binlogic juggle)
target_link_libraries(binlogic context)
target_link_libraries(binlogic jsonconfig)
target_link_libraries(binlogic fitle)