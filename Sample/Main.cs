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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Gphoto2;

namespace Sample
{
	class MainClass
	{

		public static void Main(string[] args)
		{
			Console.WriteLine("Searching...");
			List<Camera> cameras = Camera.Detect();
			
			if(cameras.Count == 0)
			{
				Console.WriteLine("No cameras were detected");
				return;
			}
			
			Console.WriteLine("About to connect...");
			foreach(Camera camera in cameras)
			{
				try
				{
					camera.Connect();
					Console.WriteLine("Connected to: {0} - {1}", camera.Name, camera.Connected);
					foreach(Gphoto2.FileSystem fs in camera.FileSystems)
					{
						Console.WriteLine("Filesystem: {0}", fs.Label);
						Console.WriteLine("Used: {2:0.00}MB, Freespace: {0:0.00}MB, Capacity: {1:0.00}MB", fs.FreeSpace / (1024.0 * 1024.0), fs.Capacity / (1024.0 * 1024.0), fs.UsedSpace / (1024.0 * 1024.0));
						Console.WriteLine("Can read: {0}, Can write: {1}, Can Delete: {2}", fs.CanRead, fs.CanWrite, fs.CanDelete);
						
						//System.IO.File.WriteAllBytes("/usr/local/alb.alb", fs.GetFile("Albums", "Best Of Bowie (Disc 1).alb").Download());
						Console.WriteLine("Folders....");
						int count=0;
						long start = System.Environment.TickCount;
						//TestUpload(fs);
						PrintFiles(fs, "Playlists",  ref count);
						//using(System.IO.TextWriter s = new System.IO.StreamWriter(new System.IO.FileStream("/usr/local/output", System.IO.FileMode.Create)))
						//	foreach(string str in Gphoto2.Base.Camera.paths)
						//		s.WriteLine(str);
						
						start = System.Environment.TickCount - start;
						Console.WriteLine("Time taken for {1} files: {0:0.00}", start / 1000.0, count);
						Console.WriteLine("Took: {0:0.00}ms per total", ((double)TotalTime / count));						
					}
				}
				finally
				{
					if(camera.Connected)
						camera.Disconnect();
				}
			}
			
			Console.WriteLine("Done");
		}
		
		private static void TestUpload(Gphoto2.FileSystem fs)
		{
			MusicFile f = new MusicFile("/home/alan", "a.mp3");
			f.Album = "AAAA";
			f.Artist = "Band Of Horses";
			f.Bitrate = 160;
			f.Duration = 60;
			f.Format = "MP3";
			f.Genre = "Rock";
			f.Year = 2000;
			
			Console.WriteLine("About to upload");
			
			if(fs.CanUpload(f))
			{
				string path = FileSystem.CombinePath("Music", "Test");
//				if(!fs.Contains(path))
//					fs.CreateDirectory("Music", "Test");
//				
//				if(fs.Contains(path, "a.mp3"))
//				{
//					Console.WriteLine("Deleting existing");
//					fs.DeleteFile(path, "a.mp3");
//				}
				
				Console.WriteLine("Uploading");
				try
				{
					MusicFile fa = (MusicFile)fs.Upload(f, path);
					Console.Write(fa.Album + fa.Artist + fa.Bitrate.ToString());
				}
				catch
				{
					Console.WriteLine("Crashed uploading");
				}
				Console.WriteLine("Uploaded");
			}			
		}
		private static long TotalTime=0;
		private static void PrintFiles(FileSystem fs, string path, ref int count)
		{
			if(count == 16000) return;
			foreach(string s in fs.GetFolders(path))
				PrintFiles(fs, FileSystem.CombinePath(path, s), ref count);
			if(count == 16000) return;
			long total = System.Environment.TickCount;
			File[] files = fs.GetFiles(path);
			foreach(File file in files)
			{
				if(count == 16000) return;
				count++;
				if(count % 500 == 0)
				{
//					Console.WriteLine("Took: {0:0.00}ms per track", (double)Gphoto2.Base.Camera.TIMETAKEN / count);
					Console.WriteLine("Took: {0:0.00}ms per total", ((double)TotalTime / count));
					//Gphoto2.Base.Camera.TIMETAKEN = 0;
					//TotalTime = 0;
					//count = 0;
				}

				if(file.Filename.EndsWith(".wma")
				   || file.Filename.EndsWith(".alb")
				   || file.Filename.EndsWith("jpg")
				   || file.Filename.EndsWith("mp3")
				   || file.Filename.EndsWith(".pla"))
					continue;
			}
			if(count == 16000) return;
			TotalTime += System.Environment.TickCount - total;
		}
	}
}
