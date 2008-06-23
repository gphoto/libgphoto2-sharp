AC_DEFUN([GP_UDEV],[dnl
if test "x${udevscriptdir}" = "x"; then	udevscriptdir="\${libdir}/udev"; fi
AC_ARG_VAR([udevscriptdir],[Directory where udev scripts like check-ptp-camere will be installed])
AC_SUBST([udevscriptdir])
AM_CONDITIONAL([HAVE_UDEV],[echo $host|grep -i linux])
])dnl
