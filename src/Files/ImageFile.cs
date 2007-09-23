// ImageFile.cs created with MonoDevelop
// User: alan at 21:00 13/09/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Gphoto2
{
	public class ImageFile : File
	{
		// Format enum for the images?
//		public int Format
//		{
//			get { return -1; }
//		}
		
		public int Depth
		{
			get { return GetInt("ImageBitDepth"); }
			set { SetValue("ImageBitDepth", value); }
		}
		
		public int Height
		{
			get { return GetInt("Height"); }
			set { SetValue("Height", value); }
		}
		
		public int Width
		{
			get { return GetInt("Width"); }
			set { SetValue("Width", value); }
		}
		
		internal ImageFile(Camera camera, string metadata, string directory, string filename, bool local)
			: base (camera, metadata, directory, filename, local)
		{
			
		}
		
		// FIXME: Return an image or something
		public void CreateThumbnail()
		{
			
		}
	}
}
