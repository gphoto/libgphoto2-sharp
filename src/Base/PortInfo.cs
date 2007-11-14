using System;
using System.Runtime.InteropServices;

namespace LibGPhoto2
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _PortInfo
    {
        internal PortType type;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=64)] internal string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=4096)] internal string path;

        /* Private */
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=1024)] internal string library_filename;
    }
    
    internal class PortInfo 
    {
        internal _PortInfo Handle;

        internal PortInfo () {
        }
        
        public string Name {
            get { return Handle.name; }
        }
        
        public string Path {
            get { return Handle.path; }
        }
		
        public string LibraryFilename {
            get{return Handle.library_filename;}
        }
    }
}
