// MusicFile.cs created with MonoDevelop
// User: alan at 21:00 13/09/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Gphoto2
{
	public class MusicFile : File
	{
		public string Album
		{
			get { return GetString("Album"); }
			set { SetString("Album", value); }
		}
		
		public string Artist
		{
			get { return GetString("Artist"); }
			set { SetString("Artist", value); }
		}
		
		public int Bitrate
		{
			get { return GetInt("AudioBitDepth"); }
			set { SetInt("AudioBitDepth", value); }
		}
		
		public int Duration
		{
			get { return GetInt("Duration"); }
			set { SetInt("Duration", value); }
		}
		
		public string Format
		{
			get { return GetString("AudioWAVECodec"); }
			set { SetString("AudioWAVECodec", value); }
		}
		
		public string Genre
		{
			get {return GetString("Genre"); }
			set { SetString("Genre", value); }
		}
		
		public string Title
		{
			get { return GetString("Name"); }
			set { SetString("Name", value); }
		}
		
		public int Track
		{
			get { return GetInt("Track"); }
			set { SetInt("Track", value); }
		}
		
		public int UseCount
		{
			get { return GetInt("UseCount"); }
			set { SetInt("UseCount", value); }
		}

		public int Year
		{
			get
			{
				int year;
				string releaseDate = GetString("OriginalReleaseDate");
				
				if(releaseDate.Length > 4)
					releaseDate = releaseDate.Substring(0, 4);
				
				if(!int.TryParse(releaseDate, out year))
					return -1;
				
				return year;
			}
			set
			{
				SetString("OriginalReleaseDate", string.Format("{0:0000}0101T0000.0", value));
			}
		}
		
		internal MusicFile(string metadata, string directory, string filename)
			: base (metadata, directory, filename)
		{
			if(string.IsNullOrEmpty(metadata))
				throw new ArgumentException("metadata cannot be null or empty");
			
			ParseMetadata(metadata);
		}
	}
}
