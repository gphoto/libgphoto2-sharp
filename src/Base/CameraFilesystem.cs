using System;
using System.Runtime.InteropServices;

namespace Gphoto2.Base
{
    public enum CameraFilePermissions
    {
        None = 0,
        Read = 1 << 0,
        Delete = 1 << 1,
        All = 0xFF
    }

    public enum CameraFileStatus
    {
        NotDownloaded,
        Downloaded
    }
    
    public enum CameraFileInfoFields
    {
        None        = 0,
        Type        = 1 << 0,
        Name        = 1 << 1,
        Size        = 1 << 2,
        Width       = 1 << 3,
        Height      = 1 << 4,
        Permissions = 1 << 5,
        Status      = 1 << 6,
        MTime       = 1 << 7,
        All         = 0xFF
    }
    
    [StructLayout (LayoutKind.Sequential)]
    public unsafe struct CameraFileInfoAudio
    {
        public CameraFileInfoFields fields;
        public CameraFileStatus status;
        public ulong size;
        [MarshalAs (UnmanagedType.ByValTStr, SizeConst=64)] public char[] type;
    }
    
    [StructLayout (LayoutKind.Sequential)]
    public unsafe struct CameraFileInfoPreview
    {
        public CameraFileInfoFields fields;
        public CameraFileStatus status;
        public ulong size;
        [MarshalAs (UnmanagedType.ByValTStr, SizeConst=64)] public char[] type;
        
        public uint width, height;
    }
    
    [StructLayout (LayoutKind.Sequential)]
    public unsafe struct CameraFileInfoFile
    {
        public CameraFileInfoFields fields;
        public CameraFileStatus status;
        public ulong size;
        [MarshalAs (UnmanagedType.ByValTStr, SizeConst=64)] public char[] type;
        
        public uint width, height;
        [MarshalAs (UnmanagedType.ByValTStr, SizeConst=64)] public char[] name;
        public CameraFilePermissions permissions;
        public long time;
    }
    
    [StructLayout (LayoutKind.Sequential)]
    public unsafe struct CameraFileInfo
    {
        public CameraFileInfoPreview preview;
        public CameraFileInfoFile file;
        public CameraFileInfoAudio audio;
    }
      
#if false
    [StructLayout (LayoutKind.Sequential)]
    internal unsafe struct _CameraFilesystem
    {
        
        internal delegate ErrorCode _CameraFilesystemGetFileFunc (HandleRef fs, char *folder, char *filename, CameraFileType type, HandleRef file, void *data, HandleRef context);

        internal delegate ErrorCode _CameraFilesystemDeleteFileFunc (HandleRef fs, char *folder, char *filename, void *data, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_set_file_funcs (HandleRef fs, _CameraFilesystemGetFileFunc get_file_func, _CameraFilesystemDeleteFileFunc del_file_func, void *data);
        
        internal delegate ErrorCode _CameraFilesystemGetInfoFunc (HandleRef fs, char *folder, char *filename, CameraFileInfo *info, void *data, HandleRef context);

        internal delegate ErrorCode _CameraFilesystemSetInfoFunc (HandleRef fs, char *folder, char *filename, CameraFileInfo info, void *data, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_set_info_funcs (HandleRef fs, _CameraFilesystemGetInfoFunc get_info_func, _CameraFilesystemSetInfoFunc set_info_func, void *data);

        internal delegate ErrorCode _CameraFilesystemPutFileFunc (HandleRef fs, char *folder, HandleRef file, void *data, HandleRef context);

        internal delegate ErrorCode _CameraFilesystemDeleteAllFunc (HandleRef fs, char *folder, void *data, HandleRef context);

        internal delegate ErrorCode _CameraFilesystemDirFunc (HandleRef fs, char *folder, char *name, void *data, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_set_folder_funcs (HandleRef fs, _CameraFilesystemPutFileFunc put_file_func, _CameraFilesystemDeleteAllFunc delete_all_func, _CameraFilesystemDirFunc make_dir_func, _CameraFilesystemDirFunc remove_dir_func, void *data);

        internal delegate ErrorCode _CameraFilesystemListFunc (HandleRef fs, char *folder, HandleRef list, void *data, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_set_list_funcs (HandleRef fs, _CameraFilesystemListFunc file_list_func, _CameraFilesystemListFunc folder_list_func, void *data);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_append (HandleRef fs, [MarshalAs(UnmanagedType.LPTStr)] string folder, [MarshalAs(UnmanagedType.LPTStr)] string filename, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_set_file_noop (HandleRef fs, [MarshalAs(UnmanagedType.LPTStr)] string folder, HandleRef file, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_dump (HandleRef fs);
    }
#endif

    public enum CameraStorageInfoFields
    {
        Base            = 1 << 0,
        Label           = 1 << 1,
        Description     = 1 << 2,
        Access          = 1 << 3,
        StorageType     = 1 << 4,
        FilesystemType  = 1 << 5,
        MaxCapacity     = 1 << 6,
        FreeSpaceKbytes = 1 << 7,
        FreeSpaceImages = 1 << 8
    }

    public enum CameraStorageType
    {
        Unknown         = 0,
        FixedRom        = 1,
        RemovableRom    = 2,
        FixedRam        = 3,
        RemovableRam    = 4
    }
    
    public enum CameraStorageAccessType
    {
        ReadWrite           = 0,
        ReadOnly            = 1,
        ReadOnlyWithDelete  = 2
    }

    public enum CameraStorageFilesystemType
    {
        Undefined           = 0,
        GenericFlat         = 1,
        GenericHierarchical = 2,
        Dcf                 = 3
    }

    [StructLayout (LayoutKind.Sequential)]
    public unsafe struct CameraStorageInformation
    {
        public CameraStorageInfoFields fields;
        [MarshalAs (UnmanagedType.ByValTStr, SizeConst=256)] public string basedir;
        [MarshalAs (UnmanagedType.ByValTStr, SizeConst=256)] public string label;
        [MarshalAs (UnmanagedType.ByValTStr, SizeConst=256)] public string description;
        public CameraStorageType type;
        public CameraStorageFilesystemType fstype;
        public CameraStorageAccessType access;
        public uint capacitykbytes;
        public uint freekbytes;
        public uint freeimages;
    }
    
    public class CameraFilesystem : Object
    {
        bool need_dispose;
        CameraList file_list;
        CameraList folder_list;
        
        public CameraFilesystem ()
        {
            IntPtr native;
            
            Error.CheckError (gp_filesystem_new (out native));
            
            this.handle = new HandleRef (this, native);
            need_dispose = true;
        }
        
        // FIXME: Is the disposing of that intptr taken care of?
        // Does it need to be?
        internal CameraFilesystem (IntPtr fs)
        {
            this.handle = new HandleRef (this, fs);
            need_dispose = false;
        }

        protected override void Dispose (bool disposing)
        {
            if (need_dispose && !Disposed)
            {
                // Don't check the error as we don't want to throw an exception if it fails
                gp_filesystem_free (this.Handle);
                base.Dispose(disposing);
            }
        }

        public CameraList ListFiles (string folder, Context context)
        {
            CameraList list = new CameraList ();
            
            Error.CheckError(gp_filesystem_list_files (this.Handle, folder, list.Handle, context.Handle));
            
            return list;
        }

        public CameraList ListFolders (string folder, Context context)
        {
            CameraList list = new CameraList ();
            
            Error.CheckError(gp_filesystem_list_folders (this.Handle, folder, list.Handle, context.Handle));
            
            return list;
        }

        public CameraFile GetFile (string folder, string filename, CameraFileType type, Context context)
        {
            CameraFile file = new CameraFile ();
            
            Error.CheckError(gp_filesystem_get_file (this.Handle, folder, filename, type, file.Handle, context.Handle));
            
            return file;
        }
        
        public void PutFile (string folder, CameraFile file, Context context)
        {
            Console.WriteLine("libgphoto2-sharp DBG: PutFile(folder={0}, CameraFile.GetName={1}, context)", folder, file.GetName());

            Error.CheckError(gp_filesystem_put_file (this.Handle, folder, file.Handle, context.Handle));
        }
        
        public void DeleteFile (string folder, string filename, Context context)
        {
            Error.CheckError(gp_filesystem_delete_file (this.Handle, folder, filename, context.Handle));
        }
        
        public void DeleteAll (string folder, Context context)
        {
            Error.CheckError(gp_filesystem_delete_all (this.Handle, folder, context.Handle));
        }
        
        public void MakeDirectory (string folder, string name, Context context)
        {
            Error.CheckError(gp_filesystem_make_dir (this.Handle, folder, name, context.Handle));
        }
        
        public void RemoveDirectory (string folder, string name, Context context)
        {
            Error.CheckError(gp_filesystem_remove_dir (this.Handle, folder, name, context.Handle));
        }
        
        public CameraFileInfo GetInfo (string folder, string filename, Context context)
        {
            CameraFileInfo fileinfo = new CameraFileInfo ();
            
            Error.CheckError(gp_filesystem_get_info  (this.Handle, folder, filename, out fileinfo, context.Handle));
            
            return fileinfo;
        }
        
        public void SetInfo (string folder, string filename, CameraFileInfo fileinfo, Context context)
        {
            Error.CheckError(gp_filesystem_set_info (this.Handle, folder, filename, fileinfo, context.Handle));
        }
        
        public int GetNumber (string folder, string filename, Context context)
        {
            return (int) Error.CheckError(gp_filesystem_number (this.Handle, folder, filename, context.Handle));
        }
        
        public string GetName (string folder, int number, Context context)
        {
            string name;
            
            Error.CheckError(gp_filesystem_name (this.Handle, folder, number, out name, context.Handle));
            
            return name;
        }
        
        public int Count (string folder, Context context)
        {
            return (int) Error.CheckError(gp_filesystem_count (this.Handle, folder, context.Handle));
        }
        
        public void Reset ()
        {
            Error.CheckError(gp_filesystem_reset (this.Handle));
        }
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_new (out IntPtr fs);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_free (HandleRef fs);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_list_files (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, HandleRef list, HandleRef context);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_list_folders (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, HandleRef list, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_get_file (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, [MarshalAs (UnmanagedType.LPTStr)] string filename, CameraFileType type, HandleRef file, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_put_file (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, HandleRef file, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_delete_file (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, [MarshalAs (UnmanagedType.LPTStr)] string filename, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_delete_all (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_make_dir (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, [MarshalAs (UnmanagedType.LPTStr)] string name, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_remove_dir (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, [MarshalAs (UnmanagedType.LPTStr)] string name, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_get_info (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, [MarshalAs (UnmanagedType.LPTStr)] string filename, out CameraFileInfo info, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_set_info (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, [MarshalAs (UnmanagedType.LPTStr)] string filename, CameraFileInfo info, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_set_info_noop (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, CameraFileInfo info, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_number (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, [MarshalAs (UnmanagedType.LPTStr)] string filename, HandleRef context);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_name (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, int filenumber, out string filename, HandleRef context);

        /* TODO: implement wrapper
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_get_folder (HandleRef fs, [MarshalAs(UnmanagedType.LPTStr)] string filename, IntPtr folder, HandleRef context);
        */
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_count (HandleRef fs, [MarshalAs (UnmanagedType.LPTStr)] string folder, HandleRef context);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_filesystem_reset (HandleRef fs);

    }
}
