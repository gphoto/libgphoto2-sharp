// PlaylistFile.cs created with MonoDevelop
// User: alan at 14:04 30/09/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Gphoto2
{
	public class PlaylistFile : File
	{
		internal PlaylistFile(Camera camera, string metadata, string directory, string filename, bool local)
			: base (camera, metadata, directory, filename, local)
		{
			
		}
	}
}
