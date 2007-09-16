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
			get { return Capacity - UsedSpace; }
		}
		
		
		internal FileSystem(Camera camera, Base.CameraStorageInformation storage)
		{
			this.camera = camera;
			this.storage = storage;
		}
		
		public void Count(string directory)
		{
			if(string.IsNullOrEmpty(directory))
				throw new ArgumentException("directory cannot be null or empty");
		}
		
		public void DeleteFile(string folder, string filename)
		{
			if(string.IsNullOrEmpty(folder))
				throw new ArgumentException("folder cannot be null or empty");
			if(string.IsNullOrEmpty(filename))
				throw new ArgumentException("filename cannot be null or empty");
		}
		
		public void DeleteFile(File file)
		{
			if(file == null)
				throw new ArgumentNullException("file");
			camera.CameraDevice.DeleteFile(file.Path, file.FileName, this.camera.Context);
		}
		
		public void DeleteAll(string folder)
		{
			DeleteAll(folder, false);
		}
		
		public void DeleteAll(string folder, bool removeFolder)
		{
			if(string.IsNullOrEmpty(folder))
				throw new ArgumentException("folder cannot be null or empty");
		}
		
		public File GetFile(string directory, string filename)
		{
			if(string.IsNullOrEmpty(directory))
				throw new ArgumentException("directory cannot be null or empty");
			if(string.IsNullOrEmpty(filename))
				throw new ArgumentException("filename cannot be null or empty");
			
			return null;
		}
		
		public File[] GetFiles(string directory)
		{
			if(string.IsNullOrEmpty(directory))
				throw new ArgumentException("directory cannot be null or empty");
			
			return null;
		}

		public string[] GetFolders(string directory)
		{
			if(string.IsNullOrEmpty(directory))
				throw new ArgumentException("directory cannot be null or empty");
			
			return null;
		}
		
		public void Upload(File file)
		{
			
		}
		
		private bool HasField(Base.CameraStorageInfoFields field)
		{
			return (storage.fields & field) == field;
		}
		
		private bool HasField(Base.CameraStorageAccessType field)
		{
			return (storage.access & field) == field;
		}
	}
}
