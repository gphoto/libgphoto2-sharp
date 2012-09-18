AC_DEFUN([SHAMROCK_CHECK_NUNIT],
[
	NUNIT_REQUIRED=2.4.7

	PKG_CHECK_MODULES(NUNIT, nunit >= $NUNIT_REQUIRED, 
		do_tests="yes", do_tests="no")
	
	AC_SUBST(NUNIT_LIBS)
	AM_CONDITIONAL(ENABLE_TESTS, test "x$do_tests" = "xyes")

	if test "x$do_tests" = "xno"; then
        PKG_CHECK_MODULES(NUNIT, mono-nunit >= 2.4, 
            do_tests="yes", do_tests="no")
        
        AC_SUBST(NUNIT_LIBS)
        AM_CONDITIONAL(ENABLE_TESTS, test "x$do_tests" = "xyes")

        if test "x$do_tests" = "xno"; then
            AC_MSG_WARN([Could not find nunit: tests will not be available.])
        fi
	fi
])
