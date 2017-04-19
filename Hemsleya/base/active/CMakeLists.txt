PROJECT(active)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

AUX_SOURCE_DIRECTORY(. DIR_SRC)

SET(header_files
    actuator.h
    active.h
    active_service.h
    scheduling.h
    signal.h
    current_scheduling.h
    current_context.h
    condition.h
    context2actuator.h
    pending_task.h
    wait.h
    yield.h
    task.h
    time_wait_context.h
    exit.h
)

SET(SRC_LIST 
    ${DIR_SRC}
)

ADD_LIBRARY(active ${SRC_LIST} ${header_files})