AC_DEFUN([GP_UDEV],[dnl
udevscriptdir="\${libdir}/udev"
AC_ARG_VAR([udevscriptdir],[Directory where udev scripts like check-ptp-camere will be installed])
AC_SUBST([udevscriptdir])
])dnl
