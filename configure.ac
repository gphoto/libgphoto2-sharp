AC_PREREQ(2.59)
dnl Next stable release must be 2.2.1 or later
dnl Bump the .1.trunk to 2.trunk etc....
AC_INIT([libgphoto2 Mono/.NET bindings], [2.2.1.4.trunk], [gphoto-devel@lists.sourceforge.net], [libgphoto2-sharp])
AC_CONFIG_SRCDIR([src/CameraList.cs])
dnl AC_CONFIG_HEADERS([config.h])
AC_CONFIG_MACRO_DIR([m4m])
AC_CONFIG_AUX_DIR([.])
AM_INIT_AUTOMAKE([-Wall -Werror gnu 1.9 dist-bzip2 check-news])

dnl Library version
dnl The easiest way to maintain this is to keep it synchronized with
dnl libgphoto2 proper.
AC_SUBST([LIBGPHOTO2_SHARP_CURRENT], [4])
AC_SUBST([LIBGPHOTO2_SHARP_REVISION], [0])


AC_PROG_CC
AC_C_CONST
AC_PROG_CPP
AC_PROG_INSTALL

dnl Something with the sequences is not quite alright yet.
dnl SED is supposed to be set in AC_LIBTOOL_SETUP, but the
dnl sequence seems to get mixed up.
SED=${SED-sed}
AC_LIBTOOL_WIN32_DLL
AC_LIBTOOL_DLOPEN
AM_PROG_LIBTOOL


dnl --------------------------------------------------------------------
dnl guess directory to install *.pc into
dnl --------------------------------------------------------------------
pkgconfigdir='${libdir}/pkgconfig'
AC_SUBST([pkgconfigdir])


dnl Check for libgphoto2
GP_CHECK_LIBRARY([LIBGPHOTO2], [libgphoto2], [>= 2.2.1.4])

dnl --- Check for mono and mcs
AC_PATH_PROG([CSC], [csc])
AC_PATH_PROG([MCS], [mcs])
AC_PATH_PROG([MONO], [mono])

CS="C#"
if test "x$CSC" = "xno" -a "x$MCS" = "xno"  ; then
	AC_MSG_ERROR([No $CS compiler found])
fi

if test "x$MCS" = "xno" ; then
	MCS=$CSC
fi

dnl --- FIXME: What about when mono and mcs are not found?

AC_SUBST([MONO])
AC_SUBST([MCS])

dnl -- Check for mono pc file

AC_MSG_CHECKING([for mono.pc])
if test -z `$PKG_CONFIG --variable=prefix mono`; then
  have_mono="no"
  AC_MSG_RESULT([missing the mono.pc file, usually found in the mono-devel package])
else
  have_mono="yes"
  AC_MSG_RESULT([found])
fi
AC_SUBST([cslibdir],[${libdir}/${PACKAGE_TARNAME}])

AM_CONDITIONAL([HAVE_MONO], [test "x${have_mono}" = "xyes"])


AC_CONFIG_FILES([
Makefile
m4m/Makefile
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
