PROJECT(triangle)
CMAKE_MINIMUM_REQUIRED(VERSION 3.22) 

set(common_include_path ../common CACHE PATH "learning common include path")
INCLUDE_DIRECTORIES(${common_include_path})

file(GLOB common_header_files ../common/ ../common/*.h)
AUX_SOURCE_DIRECTORY(../common/ common_SRC)

AUX_SOURCE_DIRECTORY(. DIR_SRC)

SET(header_files
    ${common_header_files}
)

SET(SRC_LIST 
    ${DIR_SRC}
    ${common_SRC}
)

add_executable(triangle ${SRC_LIST} ${header_files})
if (CMAKE_HOST_WIN32)
	set(TRIANGLE_WINDOWS_LIBS_DEBUG_PATH ../../../engine/lib_windows_debug CACHE PATH "windows lib debug path")
	set(TRIANGLE_WINDOWS_LIBS_RELEASE_PATH ../../../engine/lib_windows CACHE PATH "windows lib release path")
	
    TARGET_LINK_LIBRARIES(triangle debug ${TRIANGLE_WINDOWS_LIBS_DEBUG_PATH}/SDL2maind)
    TARGET_LINK_LIBRARIES(triangle debug ${TRIANGLE_WINDOWS_LIBS_DEBUG_PATH}/assimp-vc143-mtd)
	TARGET_LINK_LIBRARIES(triangle debug ${TRIANGLE_WINDOWS_LIBS_DEBUG_PATH}/engine)
    TARGET_LINK_LIBRARIES(triangle User32)
    TARGET_LINK_LIBRARIES(triangle winmm)
    TARGET_LINK_LIBRARIES(triangle version)
    TARGET_LINK_LIBRARIES(triangle Imm32)
    TARGET_LINK_LIBRARIES(triangle setupapi)
	TARGET_LINK_LIBRARIES(triangle Kernel32)
	
	target_compile_definitions(triangle PRIVATE PLATFORM_WIN32=1)
endif()