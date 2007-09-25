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
					int count=0;
					PrintFiles(fs, "/",  ref count);
				}
				
				camera.Disconnect();
			}
			
			Console.WriteLine("Done");
		}
		
		private static void PrintFiles(FileSystem fs, string path, ref int count)
		{
			if(count == 1000)
				return;
			
			Console.WriteLine("Checking: {0}", path);
			foreach(string s in fs.GetFolders(path))
				PrintFiles(fs, FileSystem.CombinePath(path, s), ref count);
			
			if(count == 1000)
				return;
			
			foreach(File file in fs.GetFiles(path))
			{
				count++;
				if(count == 1000)
					return;
				
				MusicFile music = (MusicFile)file;
				Console.WriteLine("Artist: {0}, Album: {1}, Track: {2}, Duration: {3:0.00}, UseCount: {4}",
				                  music.Artist, music.Album, music.Track, music.Duration / (1000.0 * 60), music.UseCount);
			}
		}
	}
}
