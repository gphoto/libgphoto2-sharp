using System;
using System.Runtime.InteropServices;
using Gphoto2;

namespace LibGPhoto2
{
    internal class Error
    {
        private static string GetErrorAsString(ErrorCode e)
        {
            IntPtr raw_message = gp_result_as_string(e);
            return Marshal.PtrToStringAnsi(raw_message);
        }

        private static string GetIOErrorAsString(ErrorCode e)
        {
            IntPtr raw_message = gp_port_result_as_string(e);
            return Marshal.PtrToStringAnsi(raw_message);
        }
        
        internal static bool IsError (ErrorCode error_code)
        {
            return (error_code < 0);
        }
        
        internal static GPhotoException ErrorException (ErrorCode error_code)
        {
            string message = "Unknown Error";
            int error_code_int = (int) error_code;
            
            if (error_code_int <= -102 && error_code_int >= -114)
                message = GetErrorAsString(error_code);
            else if (error_code_int <= -1 && error_code_int >= -70)
                message = GetIOErrorAsString(error_code);

            return new GPhotoException(error_code, message);
        }
        
        internal static ErrorCode CheckError (ErrorCode error)
        {
            if (IsError (error))
                throw ErrorException (error);
            
            return error;
        }
        
        [DllImport ("libgphoto2.so")]
        private static extern IntPtr gp_result_as_string (ErrorCode result);
        
        [DllImport ("libgphoto2_port.so")]
        private static extern IntPtr gp_port_result_as_string (ErrorCode result);
    }
}
