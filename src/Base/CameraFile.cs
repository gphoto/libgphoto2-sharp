using Mono.Unix;
using System;
using System.Runtime.InteropServices;

namespace Gphoto2.Base
{
    public enum CameraFileType
    {
        Preview,
        Normal,
        Raw,
        Audio,
        Exif,
        MetaData
    }

    public enum CameraFileAccessType
    {
        Memory,
        Fd
    }
    
    public class MimeTypes
    {
        [MarshalAs(UnmanagedType.LPTStr)] public static string WAV = "audio/wav";
        [MarshalAs(UnmanagedType.LPTStr)] public static string RAW = "image/x-raw";
        [MarshalAs(UnmanagedType.LPTStr)] public static string PNG = "image/png";
        [MarshalAs(UnmanagedType.LPTStr)] public static string PGM = "image/x-portable-graymap";
        [MarshalAs(UnmanagedType.LPTStr)] public static string PPM = "image/x-portable-pixmap";
        [MarshalAs(UnmanagedType.LPTStr)] public static string PNM = "image/x-portable-anymap";
        [MarshalAs(UnmanagedType.LPTStr)] public static string JPEG = "image/jpeg";
        [MarshalAs(UnmanagedType.LPTStr)] public static string TIFF = "image/tiff";
        [MarshalAs(UnmanagedType.LPTStr)] public static string BMP = "image/bmp";
        [MarshalAs(UnmanagedType.LPTStr)] public static string QUICKTIME = "video/quicktime";
        [MarshalAs(UnmanagedType.LPTStr)] public static string AVI = "video/x-msvideo";
        [MarshalAs(UnmanagedType.LPTStr)] public static string CRW = "image/x-canon-raw";
        [MarshalAs(UnmanagedType.LPTStr)] public static string UNKNOWN = "application/octet-stream";
        [MarshalAs(UnmanagedType.LPTStr)] public static string EXIF = "application/x-exif";
        [MarshalAs(UnmanagedType.LPTStr)] public static string MP3 = "audio/mpeg";
        [MarshalAs(UnmanagedType.LPTStr)] public static string OGG = "application/ogg";
        [MarshalAs(UnmanagedType.LPTStr)] public static string WMA = "audio/x-wma";
        [MarshalAs(UnmanagedType.LPTStr)] public static string ASF = "audio/x-asf";
        [MarshalAs(UnmanagedType.LPTStr)] public static string MPEG = "video/mpeg";
    }

    public class CameraFile : Object 
    {
        public CameraFile()
        {
            IntPtr native;

            Error.CheckError (gp_file_new (out native));

            this.handle = new HandleRef (this, native);
        }

        public CameraFile (UnixStream file)
        {
            IntPtr native;
            
            Error.CheckError (gp_file_new_from_fd (out native, file.Handle));
            
            this.handle = new HandleRef (this, native);
        }

        public CameraFile (int fd)
        {
            IntPtr native;

            Error.CheckError (gp_file_new_from_fd (out native, fd));

            this.handle = new HandleRef (this, native);
        }

        protected override void Cleanup ()
        {
            gp_file_unref (this.Handle);
        }
        
        public void Append (byte[] data)
        {
            Error.CheckError (gp_file_append (this.Handle, data, (ulong)data.Length));
        }
        
        public void Open (string filename)
        {
            Error.CheckError (gp_file_open (this.Handle, filename));
        }

        public void Save (string filename)
        {
            Error.CheckError (gp_file_save (this.Handle, filename));
        }
        
        public void Clean (string filename)
        {
            Error.CheckError (gp_file_clean (this.Handle));
        }

        public string GetName ()
        {
            string name;
            
            Error.CheckError (gp_file_get_name (this.Handle, out name));

            return name;
        }
        
        public void SetName (string name)
        {
            Error.CheckError (gp_file_set_name (this.Handle, name));
        }

        public CameraFileType GetFileType ()
        {
            CameraFileType type;

            Error.CheckError (gp_file_get_type (this.Handle, out type));

            return type;
        }
        
        public void SetFileType (CameraFileType type)
        {
            Error.CheckError (gp_file_set_type (this.Handle, type));
        }
        
        public string GetMimeType ()
        {
            string mime;
            
            Error.CheckError (gp_file_get_mime_type (this.Handle, out mime));

            return mime;
        }

        public void SetMimeType (string mime_type)
        {
            Error.CheckError (gp_file_set_mime_type (this.Handle, mime_type));
        }
        
        public void DetectMimeType ()
        {
            Error.CheckError (gp_file_detect_mime_type  (this.Handle));
        }
        
        public void AdjustNameForMimeType ()
        {
            Error.CheckError (gp_file_adjust_name_for_mime_type (this.Handle));
        }
        
        public void Convert (string mime_type)
        {
            Error.CheckError (CameraFile.gp_file_convert (this.Handle, mime_type));
        }
        
        public void Copy (CameraFile source)
        {
            Error.CheckError (gp_file_copy (this.Handle, source.Handle));
        }
        
        public void SetHeader (byte[] header)
        {
            Error.CheckError (gp_file_set_header(this.Handle, header));
        }
        
        public void SetWidthHeight (int width, int height)
        {
            Error.CheckError (gp_file_set_width_and_height(this.Handle, width, height));
        }
        
        public void SetDataAndSize (byte[] data)
        {
            Error.CheckError (gp_file_set_data_and_size (this.Handle, data, (ulong)data.Length));
        }
        
        public byte[] GetDataAndSize ()
        {
            ulong size;
            byte[] data;
            unsafe
            {
                IntPtr data_addr = IntPtr.Zero;
                Error.CheckError (gp_file_get_data_and_size (this.Handle, out data_addr, out size));
                data = new byte[size];
                Marshal.Copy(data_addr, data, 0, (int)size);
            }

            return data;
        }

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_new (out IntPtr file);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_new_from_fd (out IntPtr file, int fd);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_unref (HandleRef file);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_append (HandleRef file, byte[] data, ulong size);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_open (HandleRef file, string filename);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_save (HandleRef file, string filename);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_clean (HandleRef file);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_get_name (HandleRef file, out string name);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_set_name (HandleRef file, string name);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_get_type (HandleRef file, out CameraFileType type);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_set_type (HandleRef file, CameraFileType type);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_get_mime_type (HandleRef file, out string mime_type);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_set_mime_type (HandleRef file, string mime_type);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_detect_mime_type (HandleRef file);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_adjust_name_for_mime_type (HandleRef file);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_convert (HandleRef file, [MarshalAs(UnmanagedType.LPTStr)] string mime_type);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_copy (HandleRef destination, HandleRef source);

        /* TODO: Implement a wrapper for this
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_set_color_table (HandleRef file, byte *red_table, int red_size, byte *green_table, int green_size, byte *blue_table, int blue_size);
        */

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_set_header (HandleRef file, [MarshalAs(UnmanagedType.LPTStr)] byte[] header);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_set_width_and_height (HandleRef file, int width, int height);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_set_data_and_size (HandleRef file, byte[] data, ulong size);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_file_get_data_and_size (HandleRef file, out IntPtr data, out ulong size);


    }
}
