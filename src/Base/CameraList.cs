using System;
using System.Runtime.InteropServices;

namespace Gphoto2.Base
{
    public class CameraList : Object 
    {
        public CameraList ()
        {
            IntPtr native;
            Error.CheckError (gp_list_new (out native));
                      
            this.handle = new HandleRef (this, native);
        }
        
        protected override void Cleanup ()
        {
            gp_list_unref (handle);
        }
        
        public int Count ()
        {
            ErrorCode result = gp_list_count (handle);

            if (Error.IsError (result))
                throw Error.ErrorException (result);

            return (int) result;
        }
        
        public void SetName (int n, string name)
        {
            ErrorCode result = gp_list_set_name(this.Handle, n, name);

            if (Error.IsError (result))
                throw Error.ErrorException (result);
        }
        
        public void SetValue (int n, string value)
        {
            ErrorCode result = gp_list_set_value (this.Handle, n, value);

            if (Error.IsError (result))
                throw Error.ErrorException (result);
        }
        
        public string GetName (int index)
        {
            string name;

            Error.CheckError (gp_list_get_name(this.Handle, index, out name));

            return name;
        }
        
        public string GetValue (int index)
        {
            string value;

            Error.CheckError (gp_list_get_value(this.Handle, index, out value));

            return value;
        }

        public void Append (string name, string value)
        {
            Error.CheckError (gp_list_append(this.Handle, name, value));
        }
        
        public void Populate (string format, int count)
        {
            Error.CheckError (gp_list_populate(this.Handle, format, count));
        }
        
        public void Reset ()
        {
            Error.CheckError (gp_list_reset(this.Handle));
        }
        
        public void Sort ()
        {
            Error.CheckError (gp_list_sort(this.Handle));
        }
        
        public int GetPosition(string name, string value)
        {
            for (int index = 0; index < Count(); index++)
            {
                if (GetName(index) == name && GetValue(index) == value)
                    return index;
            }
            
            return -1;
        }
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_list_new (out IntPtr list);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_list_unref (HandleRef list);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_list_count (HandleRef list);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_list_set_name (HandleRef list, int index, string name);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_list_set_value (HandleRef list, int index, string value);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_list_get_name (HandleRef list, int index, out string name);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_list_get_value (HandleRef list, int index, out string value);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_list_append (HandleRef list, string name, string value);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_list_populate (HandleRef list, string format, int count);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_list_reset (HandleRef list);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_list_sort (HandleRef list);
    }
}
