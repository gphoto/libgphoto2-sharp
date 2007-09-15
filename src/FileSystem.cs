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
	public class FileSystem : IDisposable
	{
		private Camera camera;
		private bool disposed;
		private Base.CameraFilesystem filesystem;
		private Base.CameraStorageInformation storageInformation;
		
		public string BaseDirectory
		{
			get { return HasField(Base.CameraStorageInfoFields.Base) ? null : null;}
		}		
		
		public bool CanDelete
		{
			get { return false; }
		}
		
		public bool CanRead
		{
			get { return false; }
		}

		public bool CanWrite
		{
			get { return false; }
		}

		public long Capacity
		{
			get { return -1; }
		}
		
		public string Description
		{
			get { return null; }
		}
		
		public bool Disposed
		{
			get { return disposed; }
		}
		
		public Base.CameraStorageFilesystemType FilesystemType	
		{
			get { return Base.CameraStorageFilesystemType.Undefined; }
		}
		
		public long FreeSpace
		{
			get { return -1; }
		}
		
		public string Label
		{
			get { return null; }
		}
		
		public Base.CameraStorageType StorageType
		{
			get { return Base.CameraStorageType.Unknown; }
		}
		
		public long UsedSpace
		{
			get { return Capacity - UsedSpace; }
		}
		
		
		internal FileSystem(Camera camera)
		{
			this.camera = camera;
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
			
		void IDisposable.Dispose ()
		{
			Dispose(true);
		}
		
		protected virtual void Dispose(bool disposing)
		{
			if(Disposed)
				return;
			
			if(disposing)
				filesystem.Dispose();
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
			return (storageInformation.fields & field) == field;
		}
		
		private bool HasField(Base.CameraStorageAccessType field)
		{
			return (storageInformation.access & field) == field;
		}
	}
}
