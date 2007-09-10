// FileSystem.cs created with MonoDevelop
// User: alan at 14:55 09/09/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Gphoto2
{
	public class FileSystem
	{
		private Base.CameraFilesystem filesystem;
		private Camera camera;
		
		internal FileSystem(Camera camera)
		{
			this.camera = camera;
			//filesystem = camera.GetFS();
			//Base.CameraStorageInformation[] storage = camera.GetStorageInformation(camera.Context);
		}
	}
}
