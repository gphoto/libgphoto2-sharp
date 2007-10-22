/***************************************************************************
 *  Camera.cs
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
using System.Collections.Generic;
using LibGPhoto2;

namespace Gphoto2
{
	/// <summary>
	/// This represents an MTP device
	/// </summary>
    public class Camera : IDisposable
    {
		public static char DirectorySeperator = '/';
		private Abilities abilities;
		private CameraAbilities baseAbilities;
		private LibGPhoto2.Camera camera;
		private bool connected;
		private bool disposed;
		private LibGPhoto2.Context context;
		private List<Gphoto2.FileSystem> fileSystems;
		private PortInfo port;
		private int usbBusNumber;
		private int usbDeviceNumber;
		
		/// <value>
		/// The functions which the camera supports
		/// </value>
		public Abilities Abilities
		{
			get { return this.abilities; }
		}

		
		/// <value>
		/// The camera that this object is wrapping
		/// </value>
		internal LibGPhoto2.Camera Device
		{
			get { return camera; }
		}
		
		/// <value>
		/// The context which we need to pass libgphoto for every operation
		/// </value>
		internal LibGPhoto2.Context Context
		{
			get { return context; }
		}
		
		/// <value>
		/// True if the device has been connected to
		/// </value>
		public bool Connected
		{
			get { return connected; }
		}
		
		/// <value>
		/// True if the device has been disposed
		/// </value>
		public bool Disposed
		{
			get { return disposed; }
		}
		
		/// <value>
		/// The list of all filesystems on the device
		/// </value>
		public List<FileSystem> FileSystems
		{
			get { return fileSystems; }
		}
		
		/// <value>
		/// The name of the device
		/// </value>
		public string Name
		{
			get { return baseAbilities.model; }
		}
		
		/// <value>
		/// The product id of the device
		/// </value>
		public int Product
		{
			get { return baseAbilities.usb_product; }
		}
		
		/// <value>
		/// The number of the UsbBus that the device is connected to
		/// </value>
		public int UsbBusNumber
		{
			get { return usbBusNumber; }
		}
		
		/// <value>
		/// The number of the UsbPort that the device is connected to
		/// </value>
		public int UsbDeviceNumber
		{
			get { return usbDeviceNumber; }
		}
		
		/// <value>
		/// The vendor ID for the device
		/// </value>
		public int Vendor
		{
			get { return baseAbilities.usb_vendor; }
		}
		
		private Camera (CameraAbilities abilities, PortInfo port, Context context)
		{
		    string[] parts = port.Path.Substring(4).Split(',');
			this.abilities = new Abilities(abilities);
			this.baseAbilities = abilities;
			this.context = context;
			this.port = port;
			this.usbBusNumber = int.Parse(parts[0]);
			this.usbDeviceNumber = int.Parse(parts[1]);
		}
		
		private void CheckConnected(bool alreadyConnected)
		{
			if(this.Disposed)
				throw new ObjectDisposedException(typeof(Camera).Name);
			if(alreadyConnected && !Connected)
				throw new GPhotoException(ErrorCode.GeneralError, "Camera has not been connected to yet");
			if(!alreadyConnected && Connected)
				throw new GPhotoException(ErrorCode.GeneralError, "Camera has already been connected to");
		}
		
		/// <summary>
		/// Connect to the device 
		/// </summary>
		public void Connect()
		{
			CheckConnected(false);
			camera = new LibGPhoto2.Camera();
			camera.SetAbilities(baseAbilities);
			camera.SetPortInfo(port);
			camera.Init(context);
			try
			{
				LibGPhoto2.CameraStorageInformation[] storages = camera.GetStorageInformation(Context);
				fileSystems = new List<FileSystem>(storages.Length);
				for (int i = 0; i < storages.Length; i++)
					fileSystems.Add(new FileSystem(this, storages[i]));
			}
			catch
			{
				camera.Exit(context);
				throw;
			}
			connected = true;
		}
		
		/// <summary>
		/// Disconnect from the device
		/// </summary>
		public void Disconnect()
		{
			CheckConnected(true);
			connected = false;
			try
			{
				using (camera)
					camera.Exit(context);
			}
			finally
			{
				camera = null;
			}
		}
		
		/// <summary>
		/// Detects all usable cameras which are connected to the system
		/// </summary>
		/// <returns>A list containing all cameras which can be connected to</returns>
		public static List<Camera> Detect()
		{
			if (Utilities.Is64Bit && Environment.OSVersion.Platform != PlatformID.Unix)
			{
				Console.WriteLine("A 64bit windows system has been detected. This is not supported");
				Console.WriteLine("due to the complexity of interoperating with libgphoto2");
				Console.WriteLine("as it exposes variable length 'long' types in it's API.");
				Console.WriteLine("The API is unlikely to change before version 3 of the library");
				Console.WriteLine("The current status of this can be found on the libgphoto2");
				Console.WriteLine("mailing list. A detailed explanation can be found in the");
				Console.WriteLine("README file for libgphoto2-sharp");
				return new List<Camera>();
			}
			
			List<Camera> cameras = new List<Camera>();
			Context c = new Context();
			
			using (CameraAbilitiesList abilities = new CameraAbilitiesList())
			using (PortInfoList portInfoList = new PortInfoList())
			using (CameraList cameraList = new CameraList())
			{
				// Get the list of all devices that are currently supported
				abilities.Load(c);
				
				// Get the list of all the (usb?) ports that are currently available
				portInfoList.Load();
				
				// Create the list of all the connected devices which can be used
				abilities.Detect(portInfoList, cameraList, c);

				// Scan through all the detected cameras and remove any duplicates
				using (CameraList cams = RemoveDuplicates(cameraList))
				{
					int count = cams.Count();
					for(int i = 0; i < count; i++)
					{
						CameraAbilities ability = abilities.GetAbilities(abilities.LookupModel(cams.GetName(i)));
						PortInfo portInfo = portInfoList.GetInfo(portInfoList.LookupPath(cams.GetValue(i)));
						cameras.Add(new Gphoto2.Camera(ability, portInfo, c));
					}
				}
			}
			
			return cameras;
		}
		
		/// <summary>
		/// Disposes of the device
		/// </summary>
		public void Dispose()
		{
			if(Disposed)
				return;
			
			// This just makes sure that we have disconnected
			if(Connected)
				Disconnect();
			
			disposed = true;
		}
		
		/// <summary>
		/// Disconnects from the device, then reconnects again
		/// </summary>
		public void Reconnect()
		{
			Disconnect();
			Connect();
		}
		
		// FIXME: The actual conditions for ignoring 'usb:' ones is
		// when it is the only entry for that device. I'm not 100% how
		// to handle to of the same device when they are represented by
		// 'usb:' as opposed to the fully qualified name
		private static CameraList RemoveDuplicates(CameraList cameras)
		{
			CameraList list = new CameraList();
			try
			{
				int count = cameras.Count();
				for(int i=0; i < count; i++)
				{
					string name = cameras.GetName(i);
					string value = cameras.GetValue(i);
					
					if(value == "usb:")
						continue;
						
					list.Append(name, value);
				}
			}
			catch
			{
				list.Dispose();
				throw;
			}
			
			return list;
		}
    }
}

