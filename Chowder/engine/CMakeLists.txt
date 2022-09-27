project(engine)
CMAKE_MINIMUM_REQUIRED(VERSION 3.0) 

set(chowder_gfx_src_header_path ./gfx/src CACHE PATH "Chowder common include path")
include_directories(${chowder_gfx_src_header_path})

file(GLOB chowder_header_files ./ ./*.h)
file(GLOB exception_header_files ./exception ./exception/*.h)
file(GLOB chowder_gfx_header_files ./gfx/include ./gfx/include/*.h)

file(GLOB chowder_gfx_src_header_files ./gfx/src ./gfx/src/*.h)
aux_source_directory(./gfx/src chowder_gfx_dir_src)

set(header_files
    ${chowder_header_files}
	${exception_header_files}
	${chowder_gfx_header_files}
    ${chowder_gfx_src_header_files}
)

set(src_list 
    ${chowder_gfx_dir_src}
	${chowder_gfx_dir_texture_src}
)

if (CMAKE_HOST_WIN32)
	set(chowder_platform_windows_header_path ./gfx/src/platform/windows CACHE PATH "Chowder platform windows include path")
	include_directories(${chowder_platform_windows_header_path})
	
	file(GLOB chowder_gfx_win32_header_files ./gfx/src/platform/windows ./gfx/src/platform/windows/*.h)
	aux_source_directory(./gfx/src/platform/windows chowder_gfx_win32_dir_src)
	
	file(GLOB chowder_vulkan_win32_header_files ./gfx/src/vulkan/platform/windows ./gfx/src/vulkan/platform/windows/*.h)
	aux_source_directory(./gfx/src/vulkan/platform/windows chowder_vulkan_win32_dir_src)
	
	set(header_files
		${header_files}
		${chowder_gfx_win32_header_files}
	)

	set(src_list 
		${src_list}
		${chowder_gfx_win32_dir_src}
	)
	
	if (CMAKE_HOST_WIN32)
		set(WINDOWS_LIBS_DEBUG_PATH ../../engine/lib_windows_debug CACHE PATH "windows lib debug path")
		set(WINDOWS_LIBS_RELEASE_PATH ../../engine/lib_windows CACHE PATH "windows lib release path")
	endif ()

endif ()

ADD_LIBRARY(engine STATIC ${src_list} ${header_files})
if (CMAKE_HOST_WIN32)
	target_compile_definitions(engine PRIVATE PLATFORM_WIN32=1)
endif ()
