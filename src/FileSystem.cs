/***************************************************************************
 *  FileSystem.cs
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
	public class FileSystem
	{
		private Camera camera;
		private LibGPhoto2.CameraStorageInformation storage;
		
		internal string BaseDirectory
		{
			get { return HasField( LibGPhoto2.CameraStorageInfoFields.Base) ? storage.basedir : null;}
		}
		
		public bool CanDelete
		{
			get 
			{
				if(!HasField(LibGPhoto2.CameraStorageInfoFields.Access))
					return false;
				
				return HasField(LibGPhoto2.CameraStorageAccessType.ReadWrite)
					|| HasField(LibGPhoto2.CameraStorageAccessType.ReadOnlyWithDelete);
			}
		}
		
		public bool CanRead
		{
			get
			{
				if(!HasField(LibGPhoto2.CameraStorageInfoFields.Access))
					return false;
				
				return HasField(LibGPhoto2.CameraStorageAccessType.ReadOnly)
					|| HasField(LibGPhoto2.CameraStorageAccessType.ReadOnlyWithDelete)
					|| HasField(LibGPhoto2.CameraStorageAccessType.ReadWrite);
			}
		}

		public bool CanWrite
		{
			get
			{
				if(!HasField(LibGPhoto2.CameraStorageInfoFields.Access))
					return false;
				
				return HasField(LibGPhoto2.CameraStorageAccessType.ReadWrite);
			}
		}

		public long Capacity
		{
			get { return HasField(LibGPhoto2.CameraStorageInfoFields.MaxCapacity)
				? storage.capacitykbytes.ToInt64() * 1024 : -1; }
		}
		
		public string Description
		{
			get { return HasField(LibGPhoto2.CameraStorageInfoFields.Description)
				? storage.description : ""; }
		}

		internal LibGPhoto2.CameraStorageFilesystemType FilesystemType	
		{
			get { return HasField(LibGPhoto2.CameraStorageInfoFields.FilesystemType)
				? storage.fstype : LibGPhoto2.CameraStorageFilesystemType.Undefined; }
		}
		
		public long FreeSpace
		{
			get { return HasField(LibGPhoto2.CameraStorageInfoFields.FreeSpaceKbytes)
				? storage.freekbytes.ToInt64() * 1024 : -1; }
		}
		
		public string Label
		{
			get { return HasField(LibGPhoto2.CameraStorageInfoFields.Label) 
				? storage.label : ""; }
		}
		
		internal LibGPhoto2.CameraStorageType StorageType
		{
			get { return HasField(LibGPhoto2.CameraStorageInfoFields.StorageType)
				? storage.type : LibGPhoto2.CameraStorageType.Unknown; }
		}
		
		public long UsedSpace
		{
			get { return Capacity - FreeSpace; }
		}
		
		
		internal FileSystem(Camera camera, LibGPhoto2.CameraStorageInformation storage)
		{
			this.camera = camera;
			this.storage = storage;
		}
		
		public bool CanUpload(File file)
		{
			return FreeSpace > file.Size;
		}
		
		// FIXME: These are nasty hacks as there is no API for this
		public bool Contains(string directory)
		{
			try
			{
				camera.Device.ListFiles(CombinePath(BaseDirectory, directory), camera.Context);
				return true;
			}
			catch(GPhotoException ex)
			{
				if(ex.Error != ErrorCode.DirectoryNotFound)
					throw;
			}
			return false;
		}
		
		public bool Contains(string directory, string filename)
		{
			if(!Contains(directory))
				return false;
			
			try
			{
				GetFileInternal(directory, filename);
				return true;
			}
			catch(GPhotoException ex)
			{
				if(ex.Error != ErrorCode.FileNotFound)
					throw;
			}
			
			return false;
		}
		
		public int Count()
		{
			return Count("");
		}
		
		public int Count(string directory)
		{
			return Count(directory, false);
		}
		
		public int Count(string directory, bool recursive)
		{
			directory = CombinePath(BaseDirectory, directory);
			return CountRecursive(directory, recursive);
		}
		
		private int CountRecursive(string directory, bool recursive)
		{
			int count = 0;
			
			using (LibGPhoto2.CameraList list = camera.Device.ListFiles(directory, camera.Context))
				count += list.Count();
			
			if(!recursive)
				return count;
			
			using (LibGPhoto2.CameraList list = camera.Device.ListFolders(directory, camera.Context))
				foreach(string s in ParseList(list))
					count += CountRecursive(CombinePath(directory, s), recursive);
			
			return count;
		}
		
		public void CreateDirectory(string path)
		{
			if(string.IsNullOrEmpty(path))
				throw new ArgumentException("path cannot be null or empty");
			
			string[] parts = path.Split(Camera.DirectorySeperator);
			
			string current = "";
			foreach(string s in parts)
			{
				if(string.IsNullOrEmpty(s))
					continue;
				
				if (!Contains(CombinePath(current, s)))
					CreateDirectory(current, s);
				
				current = CombinePath(current, s);
			}
		}
		
		private void CreateDirectory(string path, string foldername)
		{
			if(path == null)
				throw new ArgumentNullException("path");
			
			if(string.IsNullOrEmpty(foldername))
				throw new ArgumentException("directory cannot be null or empty");
			
			path = CombinePath(BaseDirectory, path);
			
			camera.Device.MakeDirectory(path, foldername, camera.Context);
		}
		
		public void DeleteFile(string directory, string filename)
		{
			if(string.IsNullOrEmpty(filename))
				throw new ArgumentException("filename cannot be null or empty");
			
			camera.Device.DeleteFile(CombinePath(BaseDirectory, directory), filename, camera.Context);
		}
		
		public void DeleteFile(File file)
		{
			if(file == null)
				throw new ArgumentNullException("file");
			
			DeleteFile(file.Path, file.Filename);
		}
		
		public void DeleteAll(string folder)
		{
			DeleteAll(folder, false);
		}
		
		public void DeleteAll(string folder, bool removeFolder)
		{
			if(folder == null)
				throw new ArgumentNullException("folder");
			
			string path = CombinePath(BaseDirectory, folder);
			camera.Device.DeleteAll(path, camera.Context);
			
			if(!removeFolder || string.IsNullOrEmpty(folder))
				return;
			
			int index = path.LastIndexOf(Camera.DirectorySeperator);
			string pathToDirectory = path.Substring(0, index);
			string directory = path.Length > index ? path.Substring(index + 1) : "";
			camera.Device.RemoveDirectory(pathToDirectory, directory, camera.Context);
		}

		private File GetFileInternal(string directory, string filename)
		{
			return File.Create(camera, this, directory, filename);
		}
		
		public File GetFile(string directory, string filename)
		{
			if(string.IsNullOrEmpty(filename))
				throw new ArgumentException("filename cannot be null or empty");
			
			return GetFileInternal(directory, filename);
		}
		
		public File[] GetFiles(string directory)
		{
			string fullDirectory = CombinePath(BaseDirectory, directory);
			
			using (LibGPhoto2.CameraList list = camera.Device.ListFiles(fullDirectory, camera.Context))
			{
				string[] filenames = ParseList(list);
				File[] files = new File[filenames.Length];
				for(int i = 0; i < files.Length; i++)
					files[i] = GetFileInternal(directory, filenames[i]);
				
				return files;
			}
		}
		
		public string[] GetFolders()
		{
			return GetFolders("");
		}

		public string[] GetFolders(string directory)
		{			
			using (LibGPhoto2.CameraList list = camera.Device.ListFolders(CombinePath(BaseDirectory, directory), camera.Context))
				return ParseList(list);
		}
		
		private string[] ParseList(LibGPhoto2.CameraList list)
		{
			int count = list.Count();
			string[] results = new string[count];
			
			for(int i = 0; i < count; i++)
				results[i] = list.GetName(i);

			return results;
		}
		
		internal static void SplitPath(string path, out string filesystem, out string directory, out string filename)
		{
			// Split the path up and remove all empty entries
			List<string> parts = new List<string>(path.Split(Camera.DirectorySeperator));
			parts.RemoveAll(delegate (string s) { return string.IsNullOrEmpty(s); });
			
			// The filesystem is the first part and needs to be prepended with '/'
			filesystem = "/" + parts[0];
			
			// The filename is the last part
			filename = parts[parts.Count - 1];
			
			// Everything else is the 'directory' which contains the file
			directory = "";
			for(int i = 1; i < parts.Count - 1; i++)
				directory = CombinePath(directory, parts[i]);
		}
		
		// FIXME: I can do some sanity checks to make sure i can actually upload
		// which will speed things up hugely in cases where uploading is not possible
		public File Upload(File file, string directory)
		{
			return Upload(file, directory, file.Filename);
		}

		public File Upload(File file, string directory, string filename)
		{
			if(!Contains(directory))
				CreateDirectory(directory);
			
			string fullPath = CombinePath(BaseDirectory, directory);
			
			// First put the actual file data on the camera
			using (LibGPhoto2.CameraFile data = new LibGPhoto2.CameraFile())
			{
				data.SetName(filename);
				data.SetFileType(LibGPhoto2.CameraFileType.Normal);
				data.SetDataAndSize(System.IO.File.ReadAllBytes(Path.Combine(file.Path, file.Filename)));
				data.SetMimeType(file.MimeType);
				camera.Device.PutFile(fullPath, data, camera.Context);
			}
			
			// Then put the metadata on camera.
			using (LibGPhoto2.CameraFile meta = new LibGPhoto2.CameraFile())
			{
				meta.SetName(filename);
				meta.SetFileType(LibGPhoto2.CameraFileType.MetaData);
				meta.SetDataAndSize(System.Text.Encoding.UTF8.GetBytes(file.MetadataToXml()));
				camera.Device.PutFile(fullPath, meta, camera.Context);
			}
			
			// Then return the user a File object referencing the file on the camera
			// FIXME: Hack to copy the metadata correctly. Libgphoto returns null
			// metadata until the device refreshes it's database. Workaround is to manually
			// copy the metadata over from the old file.
			File returnFile = GetFileInternal(directory, filename);
			returnFile.Metadata.Clear();
			foreach (KeyValuePair<string, string> kp in file.Metadata)
				returnFile.Metadata.Add(kp.Key, kp.Value);
			
			// FIXME: This is another hack to fix the above issue
			returnFile.Size = file.Size;
			return returnFile;
		}
		
		private bool HasField(LibGPhoto2.CameraStorageInfoFields field)
		{
			return (storage.fields & field) == field;
		}
		
		private bool HasField(LibGPhoto2.CameraStorageAccessType field)
		{
			return (storage.access & field) == field;
		}
		
		public static string CombinePath(string path1, string path2)
		{
			if(string.IsNullOrEmpty(path2) || path2 == Camera.DirectorySeperator.ToString())
				return path1;

			if(path2 != null && path2.StartsWith("/"))
				path2 = path2.Substring(1);
			
			if(path1 != null && path1.EndsWith("/"))
				path1 = path1.Substring(0, path1.Length -1);
			
			return path1 + Camera.DirectorySeperator + path2;
		}
	}
}
