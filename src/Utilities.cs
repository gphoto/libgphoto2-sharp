// Utilities.cs created with MonoDevelop
// User: alan at 20:33 17/10/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Gphoto2
{
	public static class Utilities
	{
		public static bool Is64Bit
		{
			get
			{
				// Long's are 32bit if we're on windows, so we support that
				if (System.IO.Path.DirectorySeparatorChar == '\\')
					return false;
				
				// Otherwise we're on linux and longs are the same size as a pointer
				// so if it's size is 8, we have 64bit longs
				return IntPtr.Size == 8;
			}
		}
	}
}
