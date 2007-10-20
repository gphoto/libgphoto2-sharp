using System;
using System.Runtime.InteropServices;
using Gphoto2;

namespace LibGPhoto2
{
    internal enum CameraDriverStatus
    {
        Production,
        Testing,
        Experimental,
        Depreciated
    }
    
	[Flags()]
    internal enum CameraOperation
    {
        None            = 0,
        CaptureImage    = 1 << 0,
        CaptureVideo    = 1 << 1,
        CaptureAudio    = 1 << 2,
        CapturePreview  = 1 << 3,
        Config          = 1 << 4
    }
    
	[Flags()]
    internal enum CameraFileOperation
    {
        None        = 0,
        Delete      = 1 << 1,
        Preview     = 1 << 3,
        Raw         = 1 << 4,
        Audio       = 1 << 5,
        Exif        = 1 << 6
    }
    
	[Flags()]
    internal enum CameraFolderOperation
    {
        None                = 0,
        DeleteAll           = 1 << 0,
        PutFile             = 1 << 1,
        MakeDirectory       = 1 << 2,
        RemoveDirectory     = 1 << 3
    }
    
    [StructLayout(LayoutKind.Sequential)]
    internal struct CameraAbilities
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)] public string model;
        public CameraDriverStatus status;
        
        public PortType port;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=64)] public int[] speed;
        
        public CameraOperation operations;
        public CameraFileOperation file_operations;
        public CameraFolderOperation folder_operations;
        
        public int usb_vendor;
        public int usb_product;
        public int usb_class;
        public int usb_subclass;
        public int usb_protocol;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=1024)] public string library;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=1024)] public string id;
        
        public int reserved1;
        public int reserved2;
        public int reserved3;
        public int reserved4;
        public int reserved5;
        public int reserved6;
        public int reserved7;
        public int reserved8;
    }
    
    internal class CameraAbilitiesList : Object
    {
        public CameraAbilitiesList()
        {
            IntPtr native;

            Error.CheckError (gp_abilities_list_new (out native));

            this.handle = new HandleRef (this, native);
        }
        
        protected override void Dispose (bool disposing)
        {
            if(!base.Disposed)
            {
                // Don't check the error as we don't want to throw an exception if it fails 
                gp_abilities_list_free(base.Handle);
                base.Dispose(disposing);
            }
        }
        
        public void Load (Context context)
        {
            Error.CheckError(gp_abilities_list_load (this.Handle, context.Handle));
        }
        
        public void Detect (PortInfoList info_list, CameraList l, Context context)
        {
            Error.CheckError (gp_abilities_list_detect (this.handle, info_list.Handle, 
                                                        l.Handle, context.Handle));
        }
        
        public int Count ()
        {
            return (int) Error.CheckError(gp_abilities_list_count (this.handle));
        }
        
        public int LookupModel (string model)
        {
            return (int) Error.CheckError(gp_abilities_list_lookup_model(this.handle, model));
        }
        
        public CameraAbilities GetAbilities (int index)
        {
            CameraAbilities abilities = new CameraAbilities ();

            Error.CheckError (gp_abilities_list_get_abilities(this.Handle, index, out abilities));

            return abilities;
        }
        
        public void Append (CameraAbilities abilities)
        {
            Error.CheckError (gp_abilities_list_append (this.Handle, ref abilities));
        }

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_abilities_list_new (out IntPtr native);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_abilities_list_free (HandleRef list);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_abilities_list_load (HandleRef list, HandleRef context);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_abilities_list_detect (HandleRef list, HandleRef info_list, HandleRef l, HandleRef context);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_abilities_list_count (HandleRef list);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_abilities_list_lookup_model (HandleRef list, string model);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_abilities_list_get_abilities (HandleRef list, int index, out CameraAbilities abilities);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_abilities_list_append (HandleRef list, ref CameraAbilities abilities);
    }
}
