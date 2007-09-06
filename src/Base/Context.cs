using System;
using System.Runtime.InteropServices;

namespace Gphoto2.Base
{
    public class Context : Object
    {
        public Context ()
        {
            IntPtr ptr = gp_context_new ();
            if(ptr == IntPtr.Zero)
                throw new GPhotoException(ErrorCode.GeneralError, "Couldn't instantiate the Context");
            
            this.handle = new HandleRef (this, ptr);
        }
        
        protected override void Dispose (bool disposing)
        {
            if(!Disposed)
            {
                // Don't check the error as we don't want to throw an exception if it fails
                gp_context_unref(handle);
                base.Dispose(disposing);
            }
        }

        [DllImport ("libgphoto2.so")]
        internal static extern IntPtr gp_context_new ();

        [DllImport ("libgphoto2.so")]
        internal static extern void gp_context_unref   (HandleRef context);
    }
}
