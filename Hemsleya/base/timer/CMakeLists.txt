PROJECT(timer)
SET(timer_src
    timer.cpp
    clock.cpp
)
SET(timer_h
    timer.h
    clock.h
)
ADD_LIBRARY(timer ${timer_src} ${timer_h})
