project(Chowder)
CMAKE_MINIMUM_REQUIRED(VERSION 3.0) 

set(filament_header_path ${PROJECT_SOURCE_DIR}/engine/3rdparty/filament/filament/include CACHE PATH "filament include path")
include_directories(${filament_header_path})

set(filament_backend_header_path ${PROJECT_SOURCE_DIR}/engine/3rdparty/filament/filament/backend/include CACHE PATH "filament backend include path")
include_directories(${filament_backend_header_path})

set(SDL_header_path ${PROJECT_SOURCE_DIR}/engine/3rdparty/SDL/include CACHE PATH "SDL backend include path")
include_directories(${SDL_header_path})

set(math_include_path ${PROJECT_SOURCE_DIR}/engine/3rdparty/filament/libs/math/include CACHE PATH "math include path")
INCLUDE_DIRECTORIES(${math_include_path})

set(utils_include_path ${PROJECT_SOURCE_DIR}/engine/3rdparty/filament/libs/utils/include CACHE PATH "utils include path")
INCLUDE_DIRECTORIES(${utils_include_path})

set(filamentapp_include_path ${PROJECT_SOURCE_DIR}/engine/3rdparty/filament/libs/filamentapp/include CACHE PATH "filamentapp include path")
INCLUDE_DIRECTORIES(${filamentapp_include_path})

set(chowder_header_path ${PROJECT_SOURCE_DIR}/engine CACHE PATH "Chowder include path")
include_directories(${chowder_header_path})

set(chowder_gfx_header_path ${PROJECT_SOURCE_DIR}/engine/gfx/include CACHE PATH "Chowder include path")
include_directories(${chowder_gfx_header_path})

set(exception_header_path ${PROJECT_SOURCE_DIR}/engine/exception CACHE PATH "Chowder exception include path")
include_directories(${exception_header_path})
	
if (MSVC_VERSION GREATER_EQUAL "1900")
    include(CheckCXXCompilerFlag)
    CHECK_CXX_COMPILER_FLAG("/std:c++latest" _cpp_latest_flag_supported)
    if (_cpp_latest_flag_supported)
        add_compile_options("/std:c++latest")
    endif()
endif()
	
add_subdirectory(./engine)
add_subdirectory(./learning/triangle)