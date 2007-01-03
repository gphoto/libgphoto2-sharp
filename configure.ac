AC_PREREQ(2.59)
dnl Next stable release must be 2.3.2 or later
dnl Bump the .1.trunk to 2.trunk etc....
AC_INIT([libgphoto2 Mono/.NET bindings], [2.3.1.2.trunk], [gphoto-devel@lists.sourceforge.net], [libgphoto2-sharp])
AC_CONFIG_SRCDIR([src/CameraList.cs])
dnl AC_CONFIG_HEADERS([config.h])
dnl AC_CONFIG_MACRO_DIR([unused-auto-m4])
AC_CONFIG_AUX_DIR([auto-aux])
AM_INIT_AUTOMAKE([-Wall -Werror gnu 1.9 dist-bzip2 check-news])

dnl Library version
dnl The easiest way to maintain this is to keep it synchronized with
dnl libgphoto2 proper.
AC_SUBST([LIBGPHOTO2_SHARP_CURRENT], [4])
AC_SUBST([LIBGPHOTO2_SHARP_REVISION], [0])

AC_PROG_CC
AC_C_CONST
AC_PROG_CPP
AM_PROG_CC_C_O
AC_PROG_INSTALL

dnl Something with the sequences is not quite alright yet.
dnl SED is supposed to be set in AC_LIBTOOL_SETUP, but the
dnl sequence seems to get mixed up.
SED=${SED-sed}
AC_LIBTOOL_WIN32_DLL
AC_LIBTOOL_DLOPEN
AM_PROG_LIBTOOL

dnl --- Find directory to install .pc into
pkgconfigdir='${libdir}/pkgconfig'
AC_SUBST([pkgconfigdir])
AC_SUBST([cslibdir],[${libdir}/${PACKAGE_TARNAME}])

dnl --- Check for libgphoto2
GP_CHECK_LIBRARY([LIBGPHOTO2], [libgphoto2], [>= 2.3.1.1.trunk])

dnl --- Use abock's Shamrock macros to check for Mono components
SHAMROCK_CHECK_MONO_MODULE(1.1.10)
SHAMROCK_FIND_MONO_1_0_COMPILER
SHAMROCK_FIND_MONO_RUNTIME
SHAMROCK_CHECK_MONO_1_0_GAC_ASSEMBLIES([Mono.Posix])

AC_CONFIG_FILES([
Makefile
m4m/Makefile
shamrock/Makefile
src/Makefile
src/AssemblyInfo.cs
src/libgphoto2-sharp.pc
src/check-camera-list.sh
src/check-sizes.sh
],[
dnl This relies on this code being called for each of the above files
dnl with ac_file set to the filename.
	if echo "$ac_file" | grep '.sh$' > /dev/null; then
		if test -f "$ac_file"; then
			chmod +x "$ac_file"
		fi
	fi
])
AC_OUTPUT
