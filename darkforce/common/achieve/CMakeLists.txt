PROJECT(chieve)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6) 

file(GLOB head_file ./ ./*.h)

set(head_file
    ${head_file}
)

AUX_SOURCE_DIRECTORY(./ mixachieve_SRC)

ADD_LIBRARY(achieve ${mixachieve_SRC} ${head_file})