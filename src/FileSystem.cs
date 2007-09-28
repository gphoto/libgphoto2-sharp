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
		
		private string BaseDirectory
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
				GetFileInternal(FileSystem.CombinePath(BaseDirectory, directory), filename);
				return true;
			}
			catch(Gphoto2.Base.GPhotoException ex)
			{
				if(ex.Error != Gphoto2.Base.ErrorCode.FileNotFound)
					throw;
			}
			
			return false;
		}
		
		public void CreateDirectory(string path, string directory)
		{
			if(path == null)
				throw new ArgumentNullException("path");
			
			if(string.IsNullOrEmpty(directory))
				throw new ArgumentException("directory cannot be null or empty");
			
			string mtpPath = CombinePath(BaseDirectory, path);
			camera.Device.MakeDirectory(mtpPath, directory, camera.Context);
		}
		
		public void DeleteFile(string directory, string filename)
		{
			if(directory == null)
				throw new ArgumentNullException("directory");
			if(string.IsNullOrEmpty(filename))
				throw new ArgumentException("filename cannot be null or empty");
			
			camera.Device.DeleteFile(Path.Combine(BaseDirectory, directory), filename, camera.Context);
		}
		
		public void DeleteFile(File file)
		{
			if(file == null)
				throw new ArgumentNullException("file");
			camera.Device.DeleteFile(file.Path, file.Filename, this.camera.Context);
		}
		
		public void DeleteAll(string folder)
		{
			DeleteAll(folder, false);
		}
		
		public void DeleteAll(string folder, bool removeFolder)
		{
			if(string.IsNullOrEmpty(folder))
				throw new ArgumentException("folder cannot be null or empty");
			
			string path = CombinePath(BaseDirectory, folder);
			camera.Device.DeleteAll(path, camera.Context);
			
			if(!removeFolder)
				return;
			
			int index = path.LastIndexOf(Camera.DirectorySeperator);
			string pathToDirectory = path.Substring(0, index);
			string directory = path.Length > index ? path.Substring(index + 1) : "";
			camera.Device.RemoveDirectory(pathToDirectory, directory, camera.Context);
		}

		private File GetFileInternal(string directory, string filename)
		{
			using (Base.CameraFile metadata = camera.Device.GetFile(directory, filename, Base.CameraFileType.MetaData, camera.Context))
				return File.Create(camera, metadata, directory, filename);
		}
		
		public File GetFile(string directory, string filename)
		{
			if(string.IsNullOrEmpty(directory))
				throw new ArgumentException("directory cannot be null or empty");
			if(string.IsNullOrEmpty(filename))
				throw new ArgumentException("filename cannot be null or empty");
			
			return GetFileInternal(CombinePath(BaseDirectory, directory), filename);
		}
		
		public File[] GetFiles(string directory)
		{
			if(string.IsNullOrEmpty(directory))
				throw new ArgumentException("directory cannot be null or empty");
			
			string path = CombinePath(BaseDirectory, directory);
			
			using (Base.CameraList list = camera.Device.ListFiles(path, camera.Context))
			{
				string[] filenames = ParseList(list);
				File[] files = new File[filenames.Length];
				for(int i = 0; i < files.Length; i++)
					files[i] = GetFileInternal(path, filenames[i]);
				
				return files;
			}
		}

		public string[] GetFolders(string directory)
		{
			if(string.IsNullOrEmpty(directory))
				throw new ArgumentException("directory cannot be null or empty");
			
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
		
		// FIXME: I can do some sanity checks to make sure i can actually upload
		// which will speed things up hugely in cases where uploading is not possible
		public File Upload(File file, string path)
		{
			path = CombinePath(BaseDirectory, path);
			
			// First put the actual file data on the camera
			using(Base.CameraFile data = new Base.CameraFile())
			{
				data.SetName(file.Filename);
				data.SetFileType(Base.CameraFileType.Normal);
				data.SetDataAndSize(System.IO.File.ReadAllBytes(Path.Combine(file.Path, file.Filename)));
				data.SetMimeType(file.MimeType);
				camera.Device.PutFile(path, data, camera.Context);
			}
			
			// Then put the metadata on camera.
			using(Base.CameraFile meta = new Gphoto2.Base.CameraFile())
			{
				meta.SetName(file.Filename);
				meta.SetFileType(Base.CameraFileType.MetaData);
				meta.SetDataAndSize(System.Text.Encoding.UTF8.GetBytes(file.MetadataToXml()));
				camera.Device.PutFile(path, meta, camera.Context);
			}
			
			// Then return the user a File object referencing the file on the camera
			using (Base.CameraFile camfile = camera.Device.GetFile(path, file.Filename, Base.CameraFileType.MetaData, camera.Context))
				return File.Create(camera, camfile, path, file.Filename);
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
			if(path2 == Camera.DirectorySeperator.ToString())
				return path1;

			if(path2.StartsWith("/"))
				path2 = path2.Substring(1);
			
			if(path1.EndsWith("/"))
				path1 = path1.Substring(0, path1.Length -1);
			
			return path1 + Camera.DirectorySeperator + path2;
		}
	}
}
