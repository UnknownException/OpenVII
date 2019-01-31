# OpenVII
(Only compatible with Windows x64 for now)

How to build:
1. Install Visual Studio 2017 Community Edition
	Download from: https://visualstudio.microsoft.com/
	C# and C++ are required!
   
2. Install vcpkg
	Download from: https://github.com/Microsoft/vcpkg
	Run bootstrap-vcpkg.bat
	Run vcpkg install glib:x64-windows

3. Install CMake
	Download from: https://cmake.org/

4. Run CMake on Extern/FluidSynth
	Set Where to build the binaries to Extern/FluidSynthWin64
	Click on Configure and set it to Visual Studio 2017 x64	
	Set GLIBCONF_DIR to *vcpkg path*/installed/x64-windows/include
	Set GLIBH_DIR	 to *vcpkg path*/installed/x64-windows/include
	Set GLIB_LIB 	 to *vcpkg path*/installed/x64-windows/lib/glib-2.0.lib
	Set GTHREAD_LIB  to *vcpkg path*/installed/x64-windows/lib/gthread-2.0.lib
	Disable enable-ipv6
	Disable enable-network
	Disable enable-pkgconfig
	Click on Generate
	
5. Open Extern/FluidSynthWin64/FluidSynth.sln
	Build
	
6. Navigate to Extern/FluidSynthWin64/src/Debug
	Copy libfluidsynth-2.dll to OpenVII/bin/Debug/netcoreapp2.1
	
7. Navigate to *vcpkg path*/installed/x64-windows/bin
	Copy all files to OpenVII/bin/Debug/netcoreapp2.1
	
8. Open OpenVII.sln
	Build