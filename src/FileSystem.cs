// FileSystem.cs created with MonoDevelop
// User: alan at 14:55 09/09/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using System.Collections.Generic;

namespace Gphoto2
{
	public class FileSystem
	{
		private Camera camera;
		private Base.CameraStorageInformation storage;
		
		internal string BaseDirectory
		{
			get { return HasField(Base.CameraStorageInfoFields.Base) ? storage.basedir : null;}
		}
		
		public bool CanDelete
		{
			get 
			{
				if(!HasField(Base.CameraStorageInfoFields.Access))
					return false;
				
				return HasField(Base.CameraStorageAccessType.ReadWrite)
					|| HasField(Base.CameraStorageAccessType.ReadOnlyWithDelete);
			}
		}
		
		public bool CanRead
		{
			get
			{
				if(!HasField(Base.CameraStorageInfoFields.Access))
					return false;
				
				return HasField(Base.CameraStorageAccessType.ReadOnly)
					|| HasField(Base.CameraStorageAccessType.ReadOnlyWithDelete)
					|| HasField(Base.CameraStorageAccessType.ReadWrite);
			}
		}

		public bool CanWrite
		{
			get
			{
				if(!HasField(Base.CameraStorageInfoFields.Access))
					return false;
				
				return HasField(Base.CameraStorageAccessType.ReadWrite);
			}
		}

		public long Capacity
		{
			get { return HasField(Base.CameraStorageInfoFields.MaxCapacity)
				? (long)storage.capacitykbytes * 1024 : -1; }
		}
		
		public string Description
		{
			get { return HasField(Base.CameraStorageInfoFields.Description)
				? storage.description : ""; }
		}

		public Base.CameraStorageFilesystemType FilesystemType	
		{
			get { return HasField(Base.CameraStorageInfoFields.FilesystemType)
				? storage.fstype : Base.CameraStorageFilesystemType.Undefined; }
		}
		
		public long FreeSpace
		{
			get { return HasField(Base.CameraStorageInfoFields.FreeSpaceKbytes)
				? (long)storage.freekbytes * 1024 : -1; }
		}
		
		public string Label
		{
			get { return HasField(Base.CameraStorageInfoFields.Label) 
				? storage.label : ""; }
		}
		
		public Base.CameraStorageType StorageType
		{
			get { return HasField(Base.CameraStorageInfoFields.StorageType)
				? storage.type : Base.CameraStorageType.Unknown; }
		}
		
		public long UsedSpace
		{
			get { return Capacity - FreeSpace; }
		}
		
		
		internal FileSystem(Camera camera, Base.CameraStorageInformation storage)
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
			catch(Gphoto2.Base.GPhotoException ex)
			{
				if(ex.Error != Gphoto2.Base.ErrorCode.DirectoryNotFound)
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
			catch(Gphoto2.Base.GPhotoException ex)
			{
				if(ex.Error != Gphoto2.Base.ErrorCode.FileNotFound)
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
			
			using (Base.CameraList list = camera.Device.ListFiles(directory, camera.Context))
				count += list.Count();
			
			if(!recursive)
				return count;
			
			using (Base.CameraList list = camera.Device.ListFolders(directory, camera.Context))
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
			
			Console.WriteLine("Created");
		}
		
		public void CreateDirectory(string path, string foldername)
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
			// We strip out the 'base directory' when creating the File object
			// so when we return it to the user, they don't see it
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
			
			using (Base.CameraList list = camera.Device.ListFiles(fullDirectory, camera.Context))
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
			using (Base.CameraList list = camera.Device.ListFolders(CombinePath(BaseDirectory, directory), camera.Context))
				return ParseList(list);
		}
		
		private string[] ParseList(Base.CameraList list)
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
		public File Upload(File file, string path)
		{
			return Upload(file, path, file.Filename);
		}

		public File Upload(File file, string path, string filename)
		{
			if(!Contains(path))
				CreateDirectory(path);
			
			string fullPath = CombinePath(BaseDirectory, path);
			
			// First put the actual file data on the camera
			using(Base.CameraFile data = new Base.CameraFile())
			{
				data.SetName(filename);
				data.SetFileType(Base.CameraFileType.Normal);
				data.SetDataAndSize(System.IO.File.ReadAllBytes(Path.Combine(file.Path, file.Filename)));
				data.SetMimeType(file.MimeType);
				camera.Device.PutFile(fullPath, data, camera.Context);
			}
			
			// Then put the metadata on camera.
			using(Base.CameraFile meta = new Gphoto2.Base.CameraFile())
			{
				meta.SetName(filename);
				meta.SetFileType(Base.CameraFileType.MetaData);
				meta.SetDataAndSize(System.Text.Encoding.UTF8.GetBytes(file.MetadataToXml()));
				camera.Device.PutFile(fullPath, meta, camera.Context);
			}
			
			// Then return the user a File object referencing the file on the camera
			// FIXME: Hack to copy the metadata correctly. Libgphoto returns null
			// metadata until the device refreshes it's database. Workaround is to manually
			// copy the metadata over from the old file.
			File returnFile = GetFileInternal(path, filename);
			returnFile.Metadata.Clear();
			foreach (KeyValuePair<string, string> kp in file.Metadata)
				returnFile.Metadata.Add(kp.Key, kp.Value);
			
			// FIXME: This is another hack to fix the above issue
			returnFile.Size = file.Size;
			return returnFile;
		}
		
		private bool HasField(Base.CameraStorageInfoFields field)
		{
			return (storage.fields & field) == field;
		}
		
		private bool HasField(Base.CameraStorageAccessType field)
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
