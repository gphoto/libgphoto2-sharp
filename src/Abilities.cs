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
			get { return (abilities.file_operations & CameraFileOperation.Delete) == CameraFileOperation.Delete; }
		}
		
		public bool CanPreviewViewfinder
		{
			get { return (abilities.file_operations & CameraFileOperation.Preview) == CameraFileOperation.Preview;}
		}
		
		
		/// <value>
		/// True if the device supports retrieving images in the cameras native RAW format
		/// </value>
		public bool CanRetrieveRaw
		{
			get { return (abilities.file_operations & CameraFileOperation.Raw) == CameraFileOperation.Raw; }
		}

		/// <value>
		/// True if the device supports retriving audio snippets associated with a file
		/// </value>
		public bool CanRetrieveAudio
		{
			get { return (abilities.file_operations & CameraFileOperation.Audio) == CameraFileOperation.Audio; }
		}
		
		/// <value>
		/// True if the device supports retrieving Exif data associated with a file
		/// </value>
		public bool CanRetrieveExif
		{
			get { return (abilities.file_operations & CameraFileOperation.Exif) == CameraFileOperation.Exif; }
		}
		
		
		/* Folder Operations */
		/// <value>
		/// True if the device supports deleting all the files on it at once
		/// </value>
		public bool CanDeleteAll
		{
			get { return (abilities.folder_operations & CameraFolderOperation.DeleteAll) == CameraFolderOperation.DeleteAll; }
		}
		
		/// <value>
		/// True if the device supports the uploading of files
		/// </value>
		public bool CanUploadFile
		{
			get { return (abilities.folder_operations & CameraFolderOperation.PutFile) == CameraFolderOperation.PutFile; }
		}
		
		
		/// <value>
		/// True if the device supports the creation of directories
		/// </value>
		public bool CanCreateDirectory
		{
			get { return (abilities.folder_operations & CameraFolderOperation.MakeDirectory) == CameraFolderOperation.MakeDirectory; }
		}
		
		/// <value>
		/// True if the device supports deleting existing directories
		/// </value>
		public bool CanDeleteDirectory
		{
			get { return (abilities.folder_operations & CameraFolderOperation.RemoveDirectory) == CameraFolderOperation.RemoveDirectory; }
		}
		

		/* Camera Operations */
		public bool CanCaptureImages
		{
			get { return (abilities.operations & CameraOperation.CaptureImage) == CameraOperation.CaptureImage; }
		}
		
		public bool CanCaptureVideo
		{
			get { return (abilities.operations & CameraOperation.CaptureVideo) == CameraOperation.CaptureVideo; }
		}
		
		public bool CanCaptureAudio
		{
			get { return (abilities.operations & CameraOperation.CaptureAudio) == CameraOperation.CaptureAudio; }
		}
		
		public bool CanCapturePreview
		{
			get { return (abilities.operations & CameraOperation.CapturePreview) == CameraOperation.CapturePreview; }
		}
		
		public bool CanConfigureCamera
		{
			get { return (abilities.operations & CameraOperation.Config) == CameraOperation.Config; }
		}

		internal Abilities(CameraAbilities abilities)
		{
			this.abilities = abilities;
		}
	}
}
