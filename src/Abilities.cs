using System;
using Gphoto2.Base;

namespace Gphoto2
{
	public class Abilities
	{
		private CameraAbilities abilities;

		/* File Operations */
		/// <value>
		/// True if the device allows the deletion of files
		/// </value>
		public bool CanDeleteFiles
		{
			get { return HasField(CameraFileOperation.Delete); }
		}
		
		public bool CanPreviewViewfinder
		{
			get { return HasField(CameraFileOperation.Preview);}
		}
		
		
		/// <value>
		/// True if the device supports retrieving images in the cameras native RAW format
		/// </value>
		public bool CanRetrieveRaw
		{
			get { return HasField(CameraFileOperation.Raw); }
		}

		/// <value>
		/// True if the device supports retriving audio snippets associated with a file
		/// </value>
		public bool CanRetrieveAudio
		{
			get { return HasField(CameraFileOperation.Audio); }
		}
		
		/// <value>
		/// True if the device supports retrieving Exif data associated with a file
		/// </value>
		public bool CanRetrieveExif
		{
			get { return HasField(CameraFileOperation.Exif); }
		}
		
		
		/* Folder Operations */
		/// <value>
		/// True if the device supports deleting all the files on it at once
		/// </value>
		public bool CanDeleteAll
		{
			get { return HasField(CameraFolderOperation.DeleteAll); }
		}
		
		/// <value>
		/// True if the device supports the uploading of files
		/// </value>
		public bool CanUploadFile
		{
			get { return HasField(CameraFolderOperation.PutFile); }
		}
		
		
		/// <value>
		/// True if the device supports the creation of directories
		/// </value>
		public bool CanCreateDirectory
		{
			get { return HasField(CameraFolderOperation.MakeDirectory); }
		}
		
		/// <value>
		/// True if the device supports deleting existing directories
		/// </value>
		public bool CanDeleteDirectory
		{
			get { return HasField(CameraFolderOperation.RemoveDirectory); }
		}
		

		/* Camera Operations */
		public bool CanCaptureImages
		{
			get { return HasField(CameraOperation.CaptureImage); }
		}
		
		public bool CanCaptureVideo
		{
			get { return HasField(CameraOperation.CaptureVideo); }
		}
		
		public bool CanCaptureAudio
		{
			get { return HasField(CameraOperation.CaptureAudio); }
		}
		
		public bool CanCapturePreview
		{
			get { return HasField(CameraOperation.CapturePreview); }
		}
		
		public bool CanConfigureCamera
		{
			get { return HasField(CameraOperation.Config); }
		}

		internal Abilities(CameraAbilities abilities)
		{
			this.abilities = abilities;
		}
		
		private bool HasField(CameraOperation operation)
		{
			return (abilities.operations & operation) == operation;
		}
		
		private bool HasField(CameraFolderOperation operation)
		{
			return (abilities.folder_operations & operation) == operation;
		}
		
		private bool HasField(CameraFileOperation operation)
		{
			return (abilities.file_operations & operation) == operation;
		}
	}
}
