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
			get { return null; }
		}
		
		public string Artist
		{
			get { return null; }
		}
		
		public int Bitrate
		{
			get { return 1; }
		}
		
		public double Duration
		{
			get { return -1; }
		}
		
		public string Format
		{
			get { return null; }
		}
		
		public string Genre
		{
			get {return null; }
		}
		
		public string Title
		{
			get { return null; }
		}
		
		public int Track
		{
			get { return -1; }
		}
		
		public int UseCount
		{
			get { return -1; }
		}

		public int Year
		{
			get { return -1; }
		}
		
		internal MusicFile(string metadata)
		{
			if(string.IsNullOrEmpty(metadata))
				throw new ArgumentException("metadata cannot be null or empty");
			
			ParseMetadata(metadata);
		}
		
		
		private void ParseMetadata(string metadata)
        {
            /*XmlReaderSettings s = new XmlReaderSettings();
            s.ConformanceLevel = ConformanceLevel.Fragment;
			
            using(XmlTextReader r = (XmlTextReader)XmlTextReader.Create(new StringReader (metadata), s))
            while(!r.EOF && r.NodeType != XmlNodeType.None)
            {
                r.Read();
                switch(r.Name)
                {
                case "AlbumName":
                    AlbumName = r.ReadString(); break;
                    
                case "Artist":
                    Artist = r.ReadString(); break;
                    
                case "Duration":
                    double.TryParse(r.ReadString(), out Duration); break;
                
                case "Genre":
                    Genre = r.ReadString(); break;
                    
                case "OriginalReleaseDate":
                        string releaseDate = r.ReadString();
                        if(releaseDate.Length > 4)
                            releaseDate = releaseDate.Substring(0, 4);
                        int.TryParse(releaseDate, out Year);
                        break;
                    
                case "Name":
                    Name = r.ReadString(); break;
                
                case "Track":
                    uint.TryParse(r.ReadString(), out Track); break;
                    
                case "UseCount":
                    uint.TryParse(r.ReadString(), out UseCount); break;
                
                default:    // Log these missing ones. Might be worth parsing in the future
                    break;
                }
                
                r.ReadEndElement();
            }*/
        }
	}
}
