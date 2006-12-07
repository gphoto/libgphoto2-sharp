dnl GP_PYTHON & Co.
dnl
dnl Autodetect python or use given python binary.
dnl
dnl Example usage:
dnl    GP_PYTHON
dnl
dnl # sitelib for noarch packages, sitearch for others (remove the unneeded one)
dnl %{!?python_sitelib: %define python_sitelib %(%{__python} -c "from distutils.sysconfig import get_python_lib; print get_python_lib()")}
dnl %{!?python_sitearch: %define python_sitearch %(%{__python} -c "from distutils.sysconfig import get_python_lib; print get_python_lib(1)")}
dnl
AC_DEFUN([GP_PYTHON],[dnl
dnl Determine user input
AC_ARG_VAR([PYTHON], [Executable to use as python interpreter])dnl
if test "x$PYTHON" = "x"; then
	AC_ARG_WITH([python],[AS_HELP_STRING(
		[--with-python=<python>],
		[Use <python> as python interpreter])],
		[PYTHON="$withval"],
		[PYTHON="autodetect"])dnl
fi
dnl Verify values
if test "x$PYTHON" = "xfalse" || test "x$PYTHON" = "xoff" || test "x$PYTHON" = "xno"; then
	PYTHON=no
	AC_MSG_CHECKING([for python interpreter])
	AC_MSG_RESULT([no])
elif test "x$PYTHON" = "xautodetect"; then
	# Implies AC-MSG-CHECKING(for python)
	AC_PATH_PROG([PYTHON], [python], [no])
else
	AC_MSG_CHECKING([for python interpreter])
	AC_MSG_RESULT([$PYTHON (explicitly set via option or env var)])
fi
dnl Define output
AM_CONDITIONAL([HAVE_PYTHON], [test "x$PYTHON" != "xno"])
AC_SUBST([PYTHON])
if test "x$PYTHON" != "xno"; then
	AC_MSG_CHECKING([python sitearch dir])
	AC_SUBST([python_sitearchdir], ["$($PYTHON -c 'from distutils.sysconfig import get_python_lib; print get_python_lib(1)')"])
	AC_MSG_RESULT([$python_sitearchdir])
fi
])dnl
dnl
dnl End of file.
