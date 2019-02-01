# OpenVII
(Only compatible with Windows x64 for now) <br />

How to build:
1. Install Visual Studio 2017 Community Edition <br />
	Download from: https://visualstudio.microsoft.com/ <br />
	C# and C++ are required! <br />

2. Install vcpkg <br />
	Download from: https://github.com/Microsoft/vcpkg <br />
	Run bootstrap-vcpkg.bat <br />
	Run vcpkg install glib:x64-windows <br />

3. Install CMake <br />
	Download from: https://cmake.org/ <br />

4. Run CMake on Extern/FluidSynth <br />
	Set Where to build the binaries to Extern/FluidSynthWin64 <br />
	Click on Configure and set it to Visual Studio 2017 x64	<br />
	Set GLIBCONF_DIR to *vcpkg path*/installed/x64-windows/include <br />
	Set GLIBH_DIR	 to *vcpkg path*/installed/x64-windows/include <br />
	Set GLIB_LIB 	 to *vcpkg path*/installed/x64-windows/lib/glib-2.0.lib <br />
	Set GTHREAD_LIB  to *vcpkg path*/installed/x64-windows/lib/gthread-2.0.lib <br />
	Disable enable-ipv6 <br />
	Disable enable-network <br />
	Disable enable-pkgconfig <br />
	Click on Generate <br />

5. Open Extern/FluidSynthWin64/FluidSynth.sln <br />
	Build <br />
	
6. Navigate to Extern/FluidSynthWin64/src/Debug <br />
	Copy libfluidsynth-2.dll to OpenVII/bin/Debug/netcoreapp2.1 <br />

7. Navigate to *vcpkg path*/installed/x64-windows/bin <br />
	Copy all files to OpenVII/bin/Debug/netcoreapp2.1 <br />

8. Open OpenVII.sln <br />
	Build <br />