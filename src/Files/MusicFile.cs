/***************************************************************************
 *  MusicFile.cs
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

namespace Gphoto2
{
	/// <summary>
	/// Represents a music track
	/// </summary>
	public class MusicFile : File
	{
		/// <value>
		/// The name of the album
		/// </value>
		public string Album
		{
			get { return GetString("AlbumName"); }
			set { SetValue("AlbumName", value); }
		}
		
		/// <value>
		/// The name of the artist
		/// </value>
		public string Artist
		{
			get { return GetString("Artist"); }
			set { SetValue("Artist", value); }
		}
		
		/// <value>
		/// The bitrate of the track
		/// </value>
		public int Bitrate
		{
			get { return GetInt("AudioBitDepth"); }
			set { SetValue("AudioBitDepth", value); }
		}
		
		/// <value>
		/// The duration of the track in seconds
		/// </value>
		public int Duration
		{
			get { return GetInt("Duration"); }
			set { SetValue("Duration", value); }
		}
		
		/// <value>
		/// The format of the track
		/// </value>
		public string Format
		{
			get { return GetString("AudioWAVECodec"); }
			set { SetValue("AudioWAVECodec", value); }
		}
		
		/// <value>
		/// The genre of the track
		/// </value>
		public string Genre
		{
			get {return GetString("Genre"); }
			set { SetValue("Genre", value); }
		}
		
		/// <value>
		/// The title of the track
		/// </value>
		public string Title
		{
			get { return GetString("Name"); }
			set { SetValue("Name", value); }
		}
		
		/// <value>
		/// The tracknumber of the track
		/// </value>
		public int Track
		{
			get { return GetInt("Track"); }
			set { SetValue("Track", value); }
		}
		
		/// <value>
		/// The number of times the track has been played
		/// </value>
		public int UseCount
		{
			get { return GetInt("UseCount"); }
			set { SetValue("UseCount", value); }
		}

		/// <value>
		/// The year the track was recorded
		/// </value>
		public int Year
		{
			get
			{
				int year;
				string releaseDate = GetString("OriginalReleaseDate");
				
				if(releaseDate.Length > 4)
					releaseDate = releaseDate.Substring(0, 4);
				
				if(!int.TryParse(releaseDate, out year))
					return 0;
				
				return year;
			}
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("Year");
				SetValue("OriginalReleaseDate", string.Format("{0:0000}0101T0000.0", value));
			}
		}
		
		internal MusicFile(Camera camera, FileSystem fs, string metadata, string directory, string filename, bool local)
			: base (camera, fs, metadata, directory, filename, local)
		{

		}
		
		/// <summary>
		/// Creates a new music file
		/// </summary>
		/// <param name="directory">The path to the track
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="filename">The filename of the track
		/// A <see cref="System.String"/>
		/// </param>
		public MusicFile(string directory, string filename)
			: this(null, null, "", directory, filename, true)
		{
			
		}
		
		
		/// <summary>
		/// Creates a new file from the supplied stream
		/// </summary>
		/// <param name="stream">The stream containing the file data
		/// A <see cref="Stream"/>
		/// </param>
//		public MusicFile(System.IO.Stream stream)
//			: base(stream)
//		{
//			
//		}
		
		public override bool Equals (object o)
		{
			MusicFile f = o as MusicFile;
			return f == null ? false : Equals(f); 
		}
		
		public bool Equals(MusicFile file)
		{
			return file == null
				? false : this.Album == file.Album 
					&& this.Artist == file.Artist
					&& this.Title == file.Title
					&& this.Track == file.Track;
		}
		
		public override int GetHashCode ()
		{
			int result = 0;
			
			result ^= Album.GetHashCode();
			result ^= Artist.GetHashCode();
			result ^= Title.GetHashCode();
			result ^= Track;
			
			return result;
		}
	}
}
