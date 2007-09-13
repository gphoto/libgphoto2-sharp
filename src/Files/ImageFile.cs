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
		public int Format
		{
			get { return -1; }
		}
		
		public int Depth
		{
			get { return -1; }
		}
		
		public int Height
		{
			get { return -1; }
		}
		
		public int Width
		{
			get { return -1; }
		}
		
		public ImageFile()
		{
		}
		
		// FIXME: Return an image or something
		public void CreateThumbnail()
		{
		}
	}
}
