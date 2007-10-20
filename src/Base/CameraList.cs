using System;
using System.Runtime.InteropServices;
using Gphoto2;

namespace LibGPhoto2
{
    internal class CameraList : Object
    {
        public CameraList ()
        {
            IntPtr native;
            Error.CheckError (gp_list_new (out native));
                      
            this.handle = new HandleRef (this, native);
        }
        
        protected override void Dispose (bool disposing)
        {
            if(!Disposed)
            {
                // Don't check the error as we don't want to throw an exception if it fails
                gp_list_unref (handle);
                base.Dispose(disposing);
            }
        }
        
        public int Count ()
        {
            return (int) Error.CheckError(gp_list_count (handle));
        }
        
        public void SetName (int n, string name)
        {
            Error.CheckError(gp_list_set_name (this.Handle, n, name));
        }
        
        public void SetValue (int n, string value)
        {
            Error.CheckError(gp_list_set_value (this.Handle, n, value));
        }
        
        /// <summary>
        /// The model of the device which has been connected to
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetName (int index)
        {
            string name;

            Error.CheckError (gp_list_get_name (this.Handle, index, out name));

            return name;
        }
        
        /// <summary>
        /// The port on which the device is connected to
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public string GetValue (int index)
        {
            string value;

            Error.CheckError (gp_list_get_value (this.Handle, index, out value));

            return value;
        }

        public void Append (string name, string value)
        {
            Error.CheckError (gp_list_append (this.Handle, name, value));
        }
        
        public void Populate (string format, int count)
        {
            Error.CheckError (gp_list_populate (this.Handle, format, count));
        }
        
        public void Reset ()
        {
            Error.CheckError (gp_list_reset (this.Handle));
        }
        
        public void Sort ()
        {
            Error.CheckError (gp_list_sort (this.Handle));
        }
        
        public int GetPosition (string name, string value)
        {
            // Cache the value of count to reduce the number of calls needed
            // to native code. Is there a need to check both the name and value?
            int count = Count ();
            for (int index = 0; index < count; index++)
                if (GetName (index) == name && GetValue (index) == value)
                    return index;
            
            return -1;
        }
        
        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_list_new (out IntPtr list);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_list_unref (HandleRef list);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_list_count (HandleRef list);
        
        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_list_set_name (HandleRef list, int index, string name);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_list_set_value (HandleRef list, int index, string value);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_list_get_name (HandleRef list, int index, out string name);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_list_get_value (HandleRef list, int index, out string value);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_list_append (HandleRef list, string name, string value);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_list_populate (HandleRef list, string format, int count);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_list_reset (HandleRef list);

        [DllImport ("libgphoto2.so")]
        private static extern ErrorCode gp_list_sort (HandleRef list);
    }
}
