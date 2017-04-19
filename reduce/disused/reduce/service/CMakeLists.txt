PROJECT(reduce)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

AUX_SOURCE_DIRECTORY(. DIR_SRC)

SET(header_files
    ./acceptservice.h
    ./connectservice.h
    ./service.h
    ./session.h
    ./tempsession.h
    ./rpcsession.h
    ./remote_obj.h
    ./locale_obj.h
    ./obj.h
    ./uuid.h
    ./protodescription.h
    ./mutex.h
)

SET(SRC_LIST 
    ${DIR_SRC}
)

ADD_LIBRARY(reduce ${SRC_LIST} ${header_files})