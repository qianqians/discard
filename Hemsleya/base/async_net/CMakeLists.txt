PROJECT(async_net)


AUX_SOURCE_DIRECTORY(. DIR_SRC)
AUX_SOURCE_DIRECTORY(windows DIR_SRC_WINDOWS)
AUX_SOURCE_DIRECTORY(exception DIR_SRC_exception)

SET(header_files
    async_net.h
    async_service.h
    address.h
    Adapter.h
    socket.h
    socket_base_impl.h
    socket_tcp_impl.h
    socket_udp_impl.h
    writebuff.h
    readbuff.h
    buffstruct.h
    enumdef.h
    callbackdef.h
    netbuff.h
)

SET(header_files_exception
    exception/AcceptException.h
    exception/AddresException.h
    exception/bindException.h
    exception/ConnectException.h
    exception/SendException.h
    exception/RecvException.h
    exception/SendtoException.h
    exception/RecvfromException.h
    exception/DisConnectException.h
)

SET(windows_header_files
    windows/winhdef.h
    windows/overlapped.h
    windows/overlappedaccept.h
    windows/overlappedsend.h
    windows/overlappedrecv.h
    windows/overlappedrecvfrom.h
    windows/overlappedconnect.h
    windows/overlappeddisconnect.h 
    windows/work_impl.h	
    windows/socket_function.h
    windows/wsabuff.h
)

SET(SRC_LIST
    ${DIR_SRC}
    ${DIR_SRC_WINDOWS}
    ${DIR_SRC_exception}
)

ADD_LIBRARY(async_net ${SRC_LIST} ${header_files} ${windows_header_files} ${header_files_exception} )
target_link_libraries(async_net abstract_factory)
target_link_libraries(async_net exception)