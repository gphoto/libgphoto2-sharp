AC_PREREQ(2.59)
dnl Next stable release must be 2.2.1 or later
dnl Bump the .1.trunk to 2.trunk etc....
AC_INIT([libgphoto2 C# bindings], [2.2.1.4.trunk], [gphoto-devel@lists.sourceforge.net], [libgphoto2-sharp])
AC_CONFIG_SRCDIR([src/CameraList.cs])
dnl AC_CONFIG_HEADERS([config.h])
AC_CONFIG_MACRO_DIR([m4m])
AC_CONFIG_AUX_DIR([.])
AM_INIT_AUTOMAKE([-Wall -Werror gnu 1.9 dist-bzip2 check-news])

dnl Check for libgphoto2
GP_CHECK_LIBRARY([LIBGPHOTO2], [libgphoto2], [>= 2.2.1.4])

dnl Check for Mono and Mono C# compiler from f-spot

dnl --- Check for mono and mcs
dnl --- FIXME: What about when mono and mcs are not found?

AC_PATH_PROG([MONO], [mono])
AC_PATH_PROG([MCS], [mcs])
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
src/Makefile
m4m/Makefile
])
AC_OUTPUT
