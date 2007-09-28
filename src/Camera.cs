using System;
using System.Collections.Generic;
using Gphoto2.Base;

namespace Gphoto2
{
    public class Camera : IDisposable
    {
		public static char DirectorySeperator = '/';
		private Abilities abilities;
		private CameraAbilities baseAbilities;
		private Base.Camera camera;
		private bool connected;
		private bool disposed;
		private Base.Context context;
		private Gphoto2.FileSystem[] fileSystems;
		private PortInfo port;
		
		
		public Abilities Abilities
		{
			get { return this.abilities; }
		}
		
		internal Base.Camera Device
		{
			get { return camera; }
		}
		
		internal Base.Context Context
		{
			get { return context; }
		}
		
		public bool Connected
		{
			get { return connected; }
		}
		
		public bool Disposed
		{
			get { return disposed; }
		}
		
		public Gphoto2.FileSystem[] FileSystems
		{
			get { return fileSystems; }
		}
		
		public string Name
		{
			get { return baseAbilities.model; }
		}
		
		private Camera (CameraAbilities abilities, PortInfo port, Context context)
		{
			this.abilities = new Abilities(abilities);
			this.baseAbilities = abilities;
			this.context = context;
			this.port = port;
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
		
		public void Connect()
		{
			CheckConnected(false);
			camera = new Base.Camera();
			camera.SetAbilities(baseAbilities);
			camera.SetPortInfo(port);
			camera.Init(context);
			try
			{
				Base.CameraStorageInformation[] storages = camera.GetStorageInformation(Context);
				fileSystems = new FileSystem[storages.Length];
				for (int i = 0; i < storages.Length; i++)
					fileSystems[i] = new FileSystem(this, storages[i]);
			}
			catch
			{
				Disconnect();
				return;
			}
			connected = true;
		}
		
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
		/// <returns></returns>
		public static Camera[] Detect()
		{
			Camera[] cameras;
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
					cameras = new Camera[cams.Count()];
					for(int i = 0; i < cams.Count(); i++)
					{
						CameraAbilities ability = abilities.GetAbilities(abilities.LookupModel(cams.GetName(i)));
						PortInfo portInfo = portInfoList.GetInfo(portInfoList.LookupPath(cams.GetValue(i)));
						cameras[i] = new Gphoto2.Camera(ability, portInfo, c);
					}
				}
			}
			
			return cameras;
		}
		
		public void Dispose()
		{
			if(Disposed)
				return;
			
			// This just makes sure that we have disconnected
			disposed = true;
			if(Connected)
				Disconnect();
			camera.Dispose();
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

