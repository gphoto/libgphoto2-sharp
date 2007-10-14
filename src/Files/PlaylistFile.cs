/***************************************************************************
 *  PlaylistFile.cs
 *
 *  Copyright (C) 2007 Alan McGovern
 *  Written by Alan McGovern <alan.mcgovern@gmail.com>
 ****************************************************************************/

/*  THIS FILE IS LICENSED UNDER THE MIT LICENSE AS OUTLINED IMMEDIATELY BELOW: 
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a
 *  copy of this software and associated documentation files (the "Software"),  
 *  to deal in the Software without restriction, including without limitation  
 *  the rights to use, copy, modify, merge, publish, distribute, sublicense,  
 *  and/or sell copies of the Software, and to permit persons to whom the  
 *  Software is furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 *  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
 *  DEALINGS IN THE SOFTWARE.
 */


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
			get { return GetString("Name"); }
			set { SetValue("Name", value); }
		}
		
		internal PlaylistFile(Camera camera, FileSystem fsystem, string metadata, string directory, string filename, bool local)
			: base (camera, fsystem, metadata, directory, filename, local)
		{
			string file;
			string filesystem;
			files = new List<Gphoto2.File>();
			string fullDirectory = FileSystem.CombinePath(fsystem.BaseDirectory, directory);

			// The 'data' is a list of full filepaths seperated by newlines
			using (LibGPhoto2.CameraFile camfile = camera.Device.GetFile(fullDirectory, filename, LibGPhoto2.CameraFileType.Normal, camera.Context))
				metadata = System.Text.Encoding.UTF8.GetString(camfile.GetDataAndSize());
			
			StringReader r = new StringReader(metadata);
			while((file = r.ReadLine()) != null)
			{
				FileSystem.SplitPath(file, out filesystem, out directory, out filename);
				FileSystem fs = camera.FileSystems.Find(delegate (FileSystem filesys) { return filesys.BaseDirectory == filesystem; });
				files.Add(File.Create(camera, fs, directory, filename));
			}
		}
	}
}
