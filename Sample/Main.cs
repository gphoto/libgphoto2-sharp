// Main.cs created with MonoDevelop
// User: alan at 23:26 05/09/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
// If program detects some error with the software, please return a non-zero
// exit code to the system.
//
// project created on 05/09/2007 at 23:26
using System;
using System.Threading;
using Gphoto2;

namespace Sample
{
	class MainClass
	{
	
		public static void Main(string[] args)
		{	
			Console.WriteLine("Searching...");
			Camera[] cameras = Camera.Detect();
			
			if(cameras.Length == 0)
			{
				Console.WriteLine("No cameras were detected");
				return;
			}
			
			Console.WriteLine("About to connect...");
			foreach(Camera camera in cameras)
			{
				camera.Connect();
				Console.WriteLine("Connected to: {0} - {1}", camera.Name, camera.Connected);
				foreach(Gphoto2.FileSystem fs in camera.FileSystems)
				{
					Console.WriteLine("Filesystem: {0}", fs.Label);
					Console.WriteLine("Used: {2:0.00}MB, Freespace: {0:0.00}MB, Capacity: {1:0.00}MB", fs.FreeSpace / (1024.0 * 1024.0), fs.Capacity / (1024.0 * 1024.0), fs.UsedSpace / (1024.0 * 1024.0));
					Console.WriteLine("Can read: {0}, Can write: {1}, Can Delete: {2}", fs.CanRead, fs.CanWrite, fs.CanDelete);
					
					Console.WriteLine("Folders....");
					foreach(string s in fs.GetFolders("Music/"))
					{
						string path = FileSystem.CombinePath("Music", s);
						File[] files = fs.GetFiles(path);
						//Console.WriteLine("Found {0} files in {1}", files.Length, path);
						foreach(File file in files)
						{
							MusicFile music = (MusicFile)file;
							Console.WriteLine("Artist: {0}, Album: {1}, Track: {2}, Duration: {3:0.00}, UseCount: {4}",
							                  music.Artist, music.Album, music.Track, music.Duration / (1000.0 * 60), music.UseCount);
						}
					}
				}
				
				camera.Disconnect();
			}
			
			Console.WriteLine("Done");
		}
		
		/*
		public static void Main(string[] args)
		{
			PortInfo portInfo = null;
			CameraAbilities? ability = null;
			
			Console.WriteLine("Creating the context");
			Context c = new Context();
			Console.WriteLine("Created the context");
			
			
			// Get the list of all devices that are currently supported
			Console.WriteLine("Checking abilities");
			CameraAbilitiesList abilities = new CameraAbilitiesList();
			abilities.Load(c);
			Console.WriteLine("Checked the abilities");
			
			
			// Get the list of all the (usb?) ports that are currently available
			PortInfoList portInfoList = new PortInfoList();
			Console.WriteLine("Checking the port");
			portInfoList.Load();
			for(int i=0; i < portInfoList.Count(); i++)
			{
				portInfo = portInfoList.GetInfo(i);
				Console.WriteLine("Name: {0} Path: {1}", portInfo.Name, portInfo.Path);
	        }
			Console.WriteLine("Checked the port");
			
			
			// Create the list which we will store all the cameras
			// which are attached to the system and turned on.
			Console.WriteLine("Getting the cameralist");
			CameraList cameraList = new CameraList();
			Console.WriteLine("Got the cameralist");
			

			// Scan each port in portInfoList and check for any connected devices
			// Each connected device is then stored in the camera list. There may
			// be duplicates
			Console.WriteLine("Detecting Cameras");
			abilities.Detect(portInfoList, cameraList, c);
			Thread.Sleep(5000);
			Console.WriteLine("Detected Cameras: {0}", cameraList.Count());
			
			
			// Scan through all the detected cameras and choose the camera that isn't
			// found at usb:. That is for compatiblity purposes and should only be used
			// if there isn't another entry.
			ability = null;
			for(int index = 0; index < cameraList.Count(); index++)
			{
				if(cameraList.GetValue(index) == "usb:")
					continue;
				
				Console.WriteLine(cameraList.GetName(index) + ": " + cameraList.GetValue(index));
			
				// We choose the camera and get the port info for the port which it is connected to
				// This way we can tell libgphoto to connect to specifically that camera and not
				// the first auto-detected one.
				ability = abilities.GetAbilities(abilities.LookupModel(cameraList.GetName(index)));
				portInfo = portInfoList.GetInfo(portInfoList.LookupPath(cameraList.GetValue(index)));
				break;
			}

			if(ability == null)
			{
				Console.WriteLine("Found no cameras");
				return;
			}
			
			Console.WriteLine("Choose: {0}, {1}", ability.Value.model, ability.Value.id);
			

			Console.WriteLine("Connecting to the camera");
			Gphoto2.Camera camera = null;
			camera.Init(c, ability.Value, portInfo);
			Console.WriteLine("Connected to the camera");
			
			
			Console.WriteLine("Getting storage info");
			CameraStorageInformation[] infos = camera.GetStorageInformation(c);
			Console.WriteLine("Got storage info");
			
			for(int i=0; i < infos.Length; i++)
			{
				Console.Write("Base: {0}", infos[i].basedir);
				Console.Write("Capacity: {0} kB / {1} kB", infos[i].freekbytes, infos[i].capacitykbytes);
			}
			
			Console.WriteLine("Getting Filesystem");
			CameraFilesystem fs = camera.GetFS();
			Console.WriteLine("Got Filesystem");
			
			CameraStorageInformation info = infos[0];
			
			//fs.DeleteFile(info.basedir, "image.jpg", c);
			
			CameraFile file = new CameraFile();
			file.Open("/home/alan/image.jpg");
			fs.PutFile(info.basedir, file, c);
			
			CameraList list = fs.ListFiles(info.basedir, c);
			int count = list.Count();
			
			for(int i=0; i < count; i++)
				Console.WriteLine(list.GetName(i));
			
			fs.DeleteFile(info.basedir, "image.jpg", c);
			c.Dispose();
		}*/
	}
}
