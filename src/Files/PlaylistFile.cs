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
		
		public PlaylistFile(string path, string filename)
			: base(path, filename)
		{
			
		}
		
		internal PlaylistFile(Camera camera, string metadata, string directory, string filename, bool local)
			: base (camera, metadata, directory, filename, local)
		{
			string file;
			string filesystem;
			files = new List<Gphoto2.File>();
			
			// The 'metadata' is a list of full filepaths seperated by newlines
			StringReader reader = new StringReader(metadata);
			while((file = reader.ReadLine()) != null)
			{
				FileSystem.SplitPath(file, out filesystem, out directory, out filename);
				FileSystem filesys = camera.FileSystems.Find(delegate (FileSystem fs) { return fs.BaseDirectory == filesystem; });

				// Load the files from the camera
				files.Add(File.Create(camera, filesys, FileSystem.CombinePath(filesystem, directory), filename));
			}
		}
	}
}
