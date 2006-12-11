dnl We are using our own libltdl checks instead of AC_WITH_LTDL
dnl because we do not want to ship our own copy of libltdl any more.
dnl Not shipping libltdl makes it possible to ditch our own autogen.sh
dnl and relying on standard autoconf's "autoreconf".
dnl
dnl Look for external libltdl, not shipping internal libltdl.
AC_DEFUN([GP_LIB_LTDL],[dnl
AC_ARG_VAR([LTDLINCL],[C preprocessor options for libltdl])
AC_ARG_VAR([LIBLTDL],[Linker options for linking against libltdl])
if test "x${LTDLINCL}${LIBLTDL}" = "x"; then
AC_CHECK_HEADER([ltdl.h],
[dnl
AC_CHECK_LIB([ltdl], [lt_dlcaller_register],[dnl
LTDLINCL=""
LIBLTDL="-lltdl"
],[dnl
AC_MSG_ERROR([
$PACKAGE requires the ltdl library, included with libtool

Please make sure that the proper development package is installed
(libltdl-dev, libtool-ltdl-devel, etc.)
])dnl
])dnl
])
else
	AC_MSG_CHECKING([for libltdl flags])
	AC_MSG_RESULT([set explicitly: ${LTDLINCL} ${LIBLTDL}])
fi
AC_SUBST([LTDLINCL])
AC_SUBST([LIBLTDL])
])dnl
