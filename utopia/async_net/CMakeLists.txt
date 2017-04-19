PROJECT(async_net)


AUX_SOURCE_DIRECTORY(. DIR_SRC)


AUX_SOURCE_DIRECTORY(windows DIR_SRC_WINDOWS)


SET(header_files
    async_net.h
    async_service.h
    error_code.h
    sock_addr.h
    sock_buff.h
    socket.h
    socket_pool.h
    buff_pool.h
    write_buff_pool.h

)


SET(windows_header_files
    windows/Overlapped.h
    windows/winhdef.h
    windows/socket_base_WINDOWS.h
)


SET(SRC_LIST
    ${DIR_SRC}
    
    ${DIR_SRC_WINDOWS}

)


ADD_LIBRARY(async_net ${SRC_LIST} ${header_files} ${windows_header_files})