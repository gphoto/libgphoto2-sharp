using System;
using System.Runtime.InteropServices;

namespace LibGPhoto2
{
    internal abstract class Object : IDisposable
    {
		private bool disposed;
        protected HandleRef handle;

		public bool Disposed
		{
			get { return disposed; }
		}
        
        public HandleRef Handle
        {
            get { return handle; }
        }
        
        public Object ()
			: this(IntPtr.Zero)
		{
			
		}

        public Object (IntPtr ptr)
        {
            handle = new HandleRef (this, ptr);
        }
        
        public void Dispose ()
        {
            Dispose (true);
        }
        
		// FIXME: Is this right?
        protected virtual void Dispose (bool disposing)
        {
			if(!Disposed)
			{
				disposed = true;
				System.GC.SuppressFinalize (this);
			}
        }
        
        ~Object ()
        {
            Dispose (false);
        }
    }
}
