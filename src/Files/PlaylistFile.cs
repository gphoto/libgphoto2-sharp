// PlaylistFile.cs created with MonoDevelop
// User: alan at 14:04 30/09/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using System.Collections.Generic;

namespace Gphoto2
{
	public class PlaylistFile : File
	{
		private List<Gphoto2.File> files;
		
		public List<Gphoto2.File> Files
		{
			get { return files; }
		}
		
		public string Name
		{
			get { GetString("Name"); }
			set { SetValue("Name", value); }
		}
		
		internal PlaylistFile(Camera camera, string metadata, string directory, string filename, bool local)
			: base (camera, metadata, directory, filename, local)
		{
			string file;
			string filesystem;
			files = new List<Gphoto2.File>();

			// The 'data' is a list of full filepaths seperated by newlines
			using (Base.CameraFile camfile = camera.Device.GetFile(directory, filename, Base.CameraFileType.Normal, camera.Context))
				metadata = System.Text.Encoding.UTF8.GetString(camfile.GetDataAndSize());
			
			StringReader r = new StringReader(metadata);
			while(file = r.ReadLine() != null)
			{
				FileSystem.SplitPath(file, out filesystem, out directory, out filename);
				FileSystem fs = camera.FileSystems.Find(delegate (FileSystem filesys) { return filesys.BaseDirectory == filesystem; });
				files.Add(File.Create(camera, fs, directory, filename));
			}
		}
	}
}
