# Build libgphoto2-sharp, the C# bindings for libgphoto2.
#
# Due to lacking automake support for mono/C#, we compile the C#
# code as _DATA.
#
# We also have a test case which is called on "make check".
#
# This should be clean for
#   - out-of-tree builds
# 
# It may be buggy for
#   - cross-compiles
#
# This Makefile.am relies on a few variables defined in configure.in.

CLEANFILES =
EXTRA_DIST =

LIBGPHOTO2_SHARP_CSFILES =					\
	$(srcdir)/Base/Camera.cs				\
	$(srcdir)/Base/CameraAbilitiesList.cs			\
	$(srcdir)/Base/CameraFile.cs				\
	$(srcdir)/Base/CameraFilesystem.cs			\
	$(srcdir)/Base/CameraList.cs				\
	$(srcdir)/Base/CameraWidget.cs				\
	$(srcdir)/Base/Context.cs				\
	$(srcdir)/Base/ErrorCodes.cs				\
	$(srcdir)/Base/Object.cs				\
	$(srcdir)/Base/Port.cs					\
	$(srcdir)/Base/PortInfo.cs				\
	$(srcdir)/Base/PortInfoList.cs				\
								\
	$(srcdir)/Abilities.cs					\
	$(srcdir)/Camera.cs					\
	$(srcdir)/ErrorCode.cs					\
	$(srcdir)/File.cs					\
	$(srcdir)/FileSystem.cs					\
	$(srcdir)/Files/GenericFile.cs				\
	$(srcdir)/Files/ImageFile.cs				\
	$(srcdir)/Files/MusicFile.cs				\
	$(srcdir)/Files/PlaylistFile.cs				\
	$(srcdir)/GphotoException.cs				\
	$(srcdir)/Utilities.cs

MCSFLAGS = -debug -r:Mono.Posix

DOC_UPDATER = @MONODOCER@ -delete
DOC_ASSEMBLER = @MDASSEMBLER@ --out libgphoto2-sharp-docs --ecma

# Docs
EXTRA_DIST += libgphoto2-sharp-docs.source
EXTRA_DIST += $(srcdir)/en/*.xml $(srcdir)/en/*/*.xml
CLEANFILES += libgphoto2-sharp-docs.zip libgphoto2-sharp-docs.tree
if BUILD_DOCS
doc_DATA = \
	libgphoto2-sharp-docs.zip \
	libgphoto2-sharp-docs.tree \
	libgphoto2-sharp-docs.source
endif

pkgconfig_DATA = libgphoto2-sharp.pc

LIBGPHOTO2_SHARP_GENERATED_CSFILES = \
	AssemblyInfo.cs

cslib_DATA =				\
	libgphoto2-sharp.dll		\
	libgphoto2-sharp.dll.config	\
	libgphoto2-sharp.dll.mdb

CLEANFILES += libgphoto2-sharp.dll.mdb libgphoto2-sharp.dll
EXTRA_DIST += $(LIBGPHOTO2_SHARP_CSFILES)
EXTRA_DIST += AssemblyInfo.cs.in
EXTRA_DIST += libgphoto2-sharp.dll.config
libgphoto2-sharp.dll.mdb: libgphoto2-sharp.dll
libgphoto2-sharp.dll: $(LIBGPHOTO2_SHARP_GENERATED_CSFILES) $(LIBGPHOTO2_SHARP_CSFILES)
	$(MCS) $(MCSFLAGS) -out:$@ /target:library $(LIBGPHOTO2_SHARP_GENERATED_CSFILES) $(LIBGPHOTO2_SHARP_CSFILES)

CLEANFILES += TestGphoto2Sharp.exe TestGphoto2Sharp.exe.mdb
TestGphoto2Sharp.exe.mdb: TestGphoto2Sharp.exe
TestGphoto2Sharp.exe: $(srcdir)/TestGphoto2Sharp.cs libgphoto2-sharp.dll
	$(MCS) $(MCSFLAGS) -out:$@ -r:libgphoto2-sharp.dll $<

CLEANFILES += TestSizes.exe TestSizes.exe.mdb
TestSizes.exe.mdb: TestSizes.exe
TestSizes.exe: TestSizes.cs libgphoto2-sharp.dll
	$(MCS) $(MCSFLAGS) -out:$@ -r:libgphoto2-sharp.dll $<

CLEANFILES += Sample.exe.mdb Sample.exe
Sample.exe.mdb: Sample.exe
Sample.exe: $(top_srcdir)/Sample/Main.cs libgphoto2-sharp.dll
	$(MCS) $(MCSFLAGS) -out:$@ -r:libgphoto2-sharp.dll $<

update-docs: libgphoto2-sharp.dll
	$(DOC_UPDATER) -assembly:libgphoto2-sharp.dll -path:en

libgphoto2-sharp-docs.zip libgphoto2-sharp-docs.tree: $(srcdir)/en/*/*.xml
	$(DOC_ASSEMBLER) $(srcdir)/en

check_SCRIPTS = check-camera-list.sh check-sizes.sh
# FIXME: Fix build and re-add the *.exe files
check_DATA =
# check_DATA = TestGphoto2Sharp.exe TestSizes.exe Sample.exe
check_PROGRAMS = testsizes
EXTRA_PROGRAMS = testsizes

BUILT_SOURCES = testsizes.c TestSizes.cs
testsizes_SOURCES = testsizes.c
testsizes_CPPFLAGS = $(AM_CPPFLAGS) $(LIBGPHOTO2_PORT_CFLAGS) $(LIBGPHOTO2_CFLAGS) $(CPPFLAGS)

# FIXME: Fix build and re-add the tests
# TESTS = check-sizes.sh
# INSTALL_TESTS = check-camera-list.sh check-sample.sh

CLEANFILES += $(check_SCRIPTS)
CLEANFILES += $(BUILT_SOURCES)

EXTRA_DIST += libgphoto2-sharp.pc.in

EXTRA_DIST +=							\
	TestGphoto2Sharp.cs					\
	testsizes-typelist.txt					\
	testsizes-createsource.sh				\
	check-sample.sh.in					\
	check-sizes.sh.in					\
	check-camera-list.sh.in

testsizes.c: testsizes-typelist.txt $(srcdir)/testsizes-createsource.sh
	sh $(srcdir)/testsizes-createsource.sh c < $< > $@

TestSizes.cs: testsizes-typelist.txt $(srcdir)/testsizes-createsource.sh
	sh $(srcdir)/testsizes-createsource.sh csharp < $< > $@

include $(top_srcdir)/installcheck.mk
