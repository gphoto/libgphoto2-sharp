/***************************************************************************
 *  Abilities.cs
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
using LibGPhoto2;

namespace Gphoto2
{
	/// <summary>
	/// Represents all the functions the camera supports
	/// </summary>
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
		
		/// <value>
		/// True if the device supports prefiewing the view finder
		/// </value>
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
		/// <value>
		/// True if the device can capture images
		/// </value>
		public bool CanCaptureImages
		{
			get { return HasField(CameraOperation.CaptureImage); }
		}
		
		/// <value>
		/// True if the device can capture video
		/// </value>
		public bool CanCaptureVideo
		{
			get { return HasField(CameraOperation.CaptureVideo); }
		}
		
		/// <value>
		/// True if the device can capture audio
		/// </value>
		public bool CanCaptureAudio
		{
			get { return HasField(CameraOperation.CaptureAudio); }
		}
		
		/// <value>
		/// True if the device can preview captures
		/// </value>
		public bool CanCapturePreview
		{
			get { return HasField(CameraOperation.CapturePreview); }
		}
		
		/// <value>
		/// True if the device supports user configuration
		/// </value>
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
