using System;
using System.Runtime.InteropServices;

namespace Gphoto2.Base
{
    public class PortInfoList : Object 
    {
        public PortInfoList()
        {
            IntPtr native;

            Error.CheckError (gp_port_info_list_new (out native));

            this.handle = new HandleRef (this, native);
        }
        
        protected override void Dispose (bool disposing)
        {
            if(!Disposed)
            {
                // Don't check the error as we don't want to throw an exception if it fails
                gp_port_info_list_free (this.Handle);
                base.Dispose(disposing);
            }
        }
        
        public void Load ()
        {
            Error.CheckError(gp_port_info_list_load (this.Handle));
        }
        
        public int Count()
        {
            return (int) Error.CheckError (gp_port_info_list_count (this.Handle));
        }
        
        public PortInfo GetInfo (int n)
        {
            PortInfo info = new PortInfo ();
            
            Error.CheckError (gp_port_info_list_get_info (this.handle, n,  out info.Handle));
            
            return info;
        }
        
        public int LookupPath (string path)
        {
            return (int) Error.CheckError (gp_port_info_list_lookup_path(this.handle, path));
        }
        
        public int LookupName(string name)
        {
            return (int) Error.CheckError (gp_port_info_list_lookup_name (this.Handle, name));
        }
        
        public int Append (PortInfo info)
        {
            return (int) Error.CheckError (gp_port_info_list_append (this.Handle, info.Handle));
        }

        [DllImport ("libgphoto2_port.so")]
        private static extern ErrorCode gp_port_info_list_new (out IntPtr handle);
        
        [DllImport ("libgphoto2_port.so")]
        private static extern ErrorCode gp_port_info_list_free (HandleRef handle);
        
        [DllImport ("libgphoto2_port.so")]
        private static extern ErrorCode gp_port_info_list_load (HandleRef handle);

        [DllImport ("libgphoto2_port.so")]
        private static extern ErrorCode gp_port_info_list_count (HandleRef handle);

        [DllImport ("libgphoto2_port.so")]
        internal unsafe static extern ErrorCode gp_port_info_list_get_info (HandleRef handle, int n, out _PortInfo info);

        [DllImport ("libgphoto2_port.so")]
        private static extern ErrorCode gp_port_info_list_lookup_path (HandleRef handle, [MarshalAs(UnmanagedType.LPTStr)]string path);

        [DllImport ("libgphoto2_port.so")]
        private static extern ErrorCode gp_port_info_list_lookup_name (HandleRef handle, string name);

        [DllImport ("libgphoto2_port.so")]
        internal unsafe static extern ErrorCode gp_port_info_list_append (HandleRef handle, _PortInfo info);
    }
}
