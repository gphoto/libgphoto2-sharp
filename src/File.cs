/***************************************************************************
 *  File.cs
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
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Gphoto2
{
	/// <summary>
	/// The base class which represents a File on the camera
	/// </summary>
	public abstract class File
	{
		private Camera camera;
		private bool dirty;
		private string fileName;
		private bool localFile;
		private FileSystem filesystem;
		private Dictionary<string, string> metadata;
		private string mimetype;
		private string path;
		private long size;
		
		/// <value>
		/// The date when the file was added to the device
		/// </value>
		public DateTime DateAdded
		{
			get { return ParseDate(GetString("DateAdded")); }
		}
		
		/// <value>
		/// The date when the file was created
		/// </value>
		public DateTime DateCreated
		{
			get { return ParseDate(GetString("DateCreated")); }
		}
		
		/// <value>
		/// The date the file was last modified on
		/// </value>
		public DateTime DateModified
		{
			get { return ParseDate(GetString("DateModified")); }
		}
		
		/// <value>
		/// The name of the file
		/// </value>
		public string Filename
		{
			get { return fileName; }
		}
		
		internal FileSystem FileSystem
		{
			get { return this.filesystem; }
		}
		
		/// <value>
		/// True if the metadata has been changed and needs to be syncronised with the device
		/// </value>
		public bool IsDirty
		{
			get { return dirty; }
			protected set { dirty = value; }
		}
		
		/// <value>
		/// The date when the file was last played
		/// </value>
		public DateTime LastPlayed
		{
			get { return ParseDate(GetString("LastAccessed")); }
		}
		
		/// <value>
		/// True if the file is on the local filesystem, false if the file is on the MTP device
		/// </value>
		public bool LocalFile
		{
			get { return localFile; }
		}
		
		/// <value>
		/// A list of key/value pairs representing the metadata stored for the file
		/// </value>
		public Dictionary<string, string> Metadata
		{
			get { return metadata; }
		}
		
		internal string MimeType
		{
			get { return mimetype; }
		}
		
		/// <value>
		/// The path to the file
		/// </value>
		public string Path
		{
			get { return path; }
		}
		
		/// <value>
		/// The rating of the file
		/// </value>
		public int Rating
		{
			  get { return GetInt("Rating"); }
			  set { SetValue("Rating", value); }
		}
		
		/// <value>
		///  The size of the file in bytes
		/// </value>
		public long Size
		{
			get { return size; }
			internal set { size = value; } // FIXME: This is a hack to workaround a libgphoto2 issue for uploading new files
		}
		
		protected File(Camera camera, FileSystem fs, string metadata, string path, string filename, bool local)
		{
			if(metadata == null)
				throw new ArgumentNullException("metadata");
			
			this.camera = camera;
			this.fileName = filename;
			this.filesystem = fs;
			this.localFile = local;
			this.path = path;
			this.metadata = new Dictionary<string, string>();
			this.mimetype = GuessMimetype(filename);
			ParseMetadata(metadata);
			
			if(local)
				size = new FileInfo(System.IO.Path.Combine(path, filename)).Length;
			else
				size = (long)camera.Device.GetFileInfo(FileSystem.CombinePath(fs.BaseDirectory, path), filename, camera.Context).file.size;
		}
		
		// When the user creates a file, it can only reference a local file
		// So we only need the path to the file and it's filename
		protected File(string path, string filename)
			: this (null, null, "", path, filename, true)
		{
			
		}
		
		/// <summary>
		/// Reads the entire file and returns it as a byte array
		/// </summary>
		/// <returns>
		/// A <see cref="System.Byte"/>
		/// </returns>
		public byte[] Download()
		{
			if(LocalFile)
				throw new InvalidOperationException("This file is already on the local filesystem");
			
			string fullPath = FileSystem.CombinePath(filesystem.BaseDirectory, path);
			using ( LibGPhoto2.CameraFile file = camera.Device.GetFile(fullPath, fileName,  LibGPhoto2.CameraFileType.Normal, camera.Context))
				return file.GetDataAndSize();
		}
		

		/// <summary>
		/// Reads the entire file and writes it to the supplied stream
		/// </summary>
		/// <param name="stream">
		/// A <see cref="Stream"/>
		/// </param>
		/// <returns>The number of bytes written
		/// A <see cref="System.Int32"/>
		/// </returns>
		public long Download(Stream stream)
		{
			byte[] data = Download();
			stream.Write(data, 0, data.Length);
			return data.LongLength;
		}
		
		protected int GetInt(string key)
		{
			int val;
			string str = GetString(key);
			
			if(!int.TryParse(str, out val))
				return -1;
			
			return val;
		}
		
		protected string GetString(string key)
		{
			string str;
			if(!Metadata.TryGetValue(key, out str))
				return "";
			
			return str ?? "";
		}
		
		
		internal string MetadataToXml()
		{
			StringBuilder sb = new StringBuilder(metadata.Count * 32);
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.ConformanceLevel = System.Xml.ConformanceLevel.Fragment;
			
			using (XmlWriter writer = XmlWriter.Create(sb, settings))
			{
				foreach(KeyValuePair<string, string> keypair in metadata)
				{
					writer.WriteStartElement(keypair.Key);
					writer.WriteString(keypair.Value);
					writer.WriteEndElement();
				}

				return sb.ToString();
			}
		}
		
		private DateTime ParseDate(string date)
		{
			return DateTime.Now;
		}
		
		// s = start tag
		// v = value
		// e = end tag
		//                                  s:1                v    e
		static Regex element = new Regex(@"<([^<>]+?)>(?<value>.*?)</\1>", RegexOptions.Compiled);
		
		protected void ParseMetadata(string metadata)
		{
			XmlReaderSettings s = new XmlReaderSettings();
			s.ConformanceLevel = ConformanceLevel.Fragment;
			s.IgnoreWhitespace = true;
			s.CheckCharacters = false;
			
			// Parse the metadata into a dictionary so we can show it
			// all to the user
			try
			{
				using (XmlReader r = (XmlReader)XmlReader.Create(new StringReader (metadata), s))
					while (r.Read())
						this.metadata.Add(r.Name, r.ReadString());
			}
			catch (Exception ex)
			{
				// If the standard XmlReader fails, the XML is 'invalid', so
				// try using the last-ditch parser. It's a regex to attempt to
				// match start/end tags and extract the value.
				this.metadata = ParseToDictionary(metadata);
			}
		}
		
		internal static Dictionary<string, string> ParseToDictionary(string xml)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			MatchCollection matches = element.Matches(xml);
			
			foreach (Match match in matches)
				dictionary.Add(match.Groups[1].Value, match.Groups["value"].Value);
			
			return dictionary;
		}
		
		protected void SetValue(string key, int value)
		{
			SetValue(key, value.ToString());
		}
		
		protected void SetValue(string key, string value)
		{
			value = value ?? "";
			
			if(!Metadata.ContainsKey(key))
			{
				dirty = true;
				Metadata.Add(key, value);
				return;
			}
			
			if(Metadata[key] != value)
				dirty = true;
			
			Metadata[key] = value;
		}
		
		/// <summary>
		/// Synchronises the file metadata with the camera
		/// </summary>
		public void Update()
		{
			if(LocalFile)
				throw new InvalidOperationException("Cannot update metadata on a local file");
			
			if (!IsDirty)
				return;
			
			string metadata = MetadataToXml();
			using (LibGPhoto2.CameraFile file = new  LibGPhoto2.CameraFile())
			{
				file.SetFileType( LibGPhoto2.CameraFileType.MetaData);
				file.SetName(Filename);
				file.SetDataAndSize(System.Text.Encoding.UTF8.GetBytes(metadata));
				camera.Device.PutFile(FileSystem.CombinePath(filesystem.BaseDirectory, path), file, camera.Context);
			}
			dirty = false;
		}

		internal static File Create(Camera camera, FileSystem fs, string directory, string filename)
		{
			 LibGPhoto2.CameraFile metadataFile;
			string metadata = null;
			string mime = GuessMimetype(filename);
			 LibGPhoto2.CameraFileType type =   LibGPhoto2.CameraFileType.MetaData;
			string fullDirectory = FileSystem.CombinePath(fs.BaseDirectory, directory);
			
			using (metadataFile = camera.Device.GetFile(fullDirectory, filename, type, camera.Context))
				metadata = Encoding.UTF8.GetString(metadataFile.GetDataAndSize());
			
			/* First check to see if it's a music file */
			if (mime ==  LibGPhoto2.MimeTypes.MP3 ||
			    mime ==  LibGPhoto2.MimeTypes.WMA ||
			    mime ==   LibGPhoto2.MimeTypes.WAV ||
			    mime ==   LibGPhoto2.MimeTypes.OGG)
			{
				return new MusicFile(camera, fs, metadata, directory, filename, false);
			}
			
			/* Second check to see if it's an image */
			if(mime ==  LibGPhoto2.MimeTypes.BMP ||
			   mime ==   LibGPhoto2.MimeTypes.JPEG ||
			   mime ==   LibGPhoto2.MimeTypes.PNG ||
			   mime ==   LibGPhoto2.MimeTypes.RAW ||
			   mime ==   LibGPhoto2.MimeTypes.TIFF)
			{
				return new ImageFile(camera, fs, metadata, directory, filename, false);
			}

			/* Third check to see if it's a playlist */
			if(filename.EndsWith(".zpl"))
			{
				// A playlist needs the actual data to work correctly as opposed to the metadata
				// this data is grabbed in the Playlist constructor
				return new PlaylistFile(camera, fs, metadata, directory, filename, false);
			}
			
			return new GenericFile(camera, fs, metadata, directory, filename, false);
		}
		
		private static string GuessMimetype(string filename)
		{
			if(filename.EndsWith(".asf", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.ASF;
			
			if(filename.EndsWith(".avi", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.AVI;
			
			if(filename.EndsWith(".BMP", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.BMP;
						
			if(filename.EndsWith(".CRW", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.CRW;	
			
			if(filename.EndsWith(".EXIF", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.EXIF;	
			
			if(filename.EndsWith(".JPEG", System.StringComparison.OrdinalIgnoreCase)
			   || filename.EndsWith(".JPG", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.JPEG;	
			
			if(filename.EndsWith(".MP3", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.MP3;	
			
			if(filename.EndsWith(".MPG", System.StringComparison.OrdinalIgnoreCase)
			   || filename.EndsWith(".MPEG", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.MPEG;	
			
			if(filename.EndsWith(".OGG", System.StringComparison.OrdinalIgnoreCase)
			   || filename.EndsWith(".OGM", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.OGG;
						
			if(filename.EndsWith(".PGM", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.PGM;
						
			if(filename.EndsWith(".PNG", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.PNG;
						
			if(filename.EndsWith(".PNM", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.PNM;
						
			if(filename.EndsWith(".PPM", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.PPM;
									
			if(filename.EndsWith(".MOV", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.QUICKTIME;
									
			if(filename.EndsWith(".RAW", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.RAW;
									
			if(filename.EndsWith(".TIFF", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.TIFF;
									
			if(filename.EndsWith(".WAV", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.WAV;
									
			if(filename.EndsWith(".WMA", System.StringComparison.OrdinalIgnoreCase))
				return  LibGPhoto2.MimeTypes.WMA;

			return  LibGPhoto2.MimeTypes.UNKNOWN;
		}
	}
}


/*  {PTP_OPC_StorageID,"StorageID"},
	{PTP_OPC_ObjectFormat,"ObjectFormat"},
	{PTP_OPC_ProtectionStatus,"ProtectionStatus"},
	{PTP_OPC_ObjectSize,"ObjectSize"},
	{PTP_OPC_AssociationType,"AssociationType"},
	{PTP_OPC_AssociationDesc,"AssociationDesc"},
	{PTP_OPC_ObjectFileName,"ObjectFileName"},
	{PTP_OPC_DateCreated,"DateCreated"},
	{PTP_OPC_DateModified,"DateModified"},
	{PTP_OPC_Keywords,"Keywords"},
	{PTP_OPC_ParentObject,"ParentObject"},
	{PTP_OPC_AllowedFolderContents,"AllowedFolderContents"},
	{PTP_OPC_Hidden,"Hidden"},
	{PTP_OPC_SystemObject,"SystemObject"},
	{PTP_OPC_PersistantUniqueObjectIdentifier,"PersistantUniqueObjectIdentifier"},
	{PTP_OPC_SyncID,"SyncID"},
	{PTP_OPC_PropertyBag,"PropertyBag"},
	{PTP_OPC_Name,"Name"},
	{PTP_OPC_CreatedBy,"CreatedBy"},
	{PTP_OPC_Artist,"Artist"},
	{PTP_OPC_DateAuthored,"DateAuthored"},
	{PTP_OPC_Description,"Description"},
	{PTP_OPC_URLReference,"URLReference"},
	{PTP_OPC_LanguageLocale,"LanguageLocale"},
	{PTP_OPC_CopyrightInformation,"CopyrightInformation"},
	{PTP_OPC_Source,"Source"},
	{PTP_OPC_OriginLocation,"OriginLocation"},
	{PTP_OPC_DateAdded,"DateAdded"},
	{PTP_OPC_NonConsumable,"NonConsumable"},
	{PTP_OPC_CorruptOrUnplayable,"CorruptOrUnplayable"},
	{PTP_OPC_ProducerSerialNumber,"ProducerSerialNumber"},
	{PTP_OPC_RepresentativeSampleFormat,"RepresentativeSampleFormat"},
	{PTP_OPC_RepresentativeSampleSize,"RepresentativeSampleSize"},
	{PTP_OPC_RepresentativeSampleHeight,"RepresentativeSampleHeight"},
	{PTP_OPC_RepresentativeSampleWidth,"RepresentativeSampleWidth"},
	{PTP_OPC_RepresentativeSampleDuration,"RepresentativeSampleDuration"},
	{PTP_OPC_RepresentativeSampleData,"RepresentativeSampleData"},
	{PTP_OPC_Width,"Width"},
	{PTP_OPC_Height,"Height"},
	{PTP_OPC_Duration,"Duration"},
	{PTP_OPC_Rating,"Rating"},
	{PTP_OPC_Track,"Track"},
	{PTP_OPC_Genre,"Genre"},
	{PTP_OPC_Credits,"Credits"},
	{PTP_OPC_Lyrics,"Lyrics"},
	{PTP_OPC_SubscriptionContentID,"SubscriptionContentID"},
	{PTP_OPC_ProducedBy,"ProducedBy"},
	{PTP_OPC_UseCount,"UseCount"},
	{PTP_OPC_SkipCount,"SkipCount"},
	{PTP_OPC_LastAccessed,"LastAccessed"},
	{PTP_OPC_ParentalRating,"ParentalRating"},
	{PTP_OPC_MetaGenre,"MetaGenre"},
	{PTP_OPC_Composer,"Composer"},
	{PTP_OPC_EffectiveRating,"EffectiveRating"},
	{PTP_OPC_Subtitle,"Subtitle"},
	{PTP_OPC_OriginalReleaseDate,"OriginalReleaseDate"},
	{PTP_OPC_AlbumName,"AlbumName"},
	{PTP_OPC_AlbumArtist,"AlbumArtist"},
	{PTP_OPC_Mood,"Mood"},
	{PTP_OPC_DRMStatus,"DRMStatus"},
	{PTP_OPC_SubDescription,"SubDescription"},
	{PTP_OPC_IsCropped,"IsCropped"},
	{PTP_OPC_IsColorCorrected,"IsColorCorrected"},
	{PTP_OPC_ImageBitDepth,"ImageBitDepth"},
	{PTP_OPC_Fnumber,"Fnumber"},
	{PTP_OPC_ExposureTime,"ExposureTime"},
	{PTP_OPC_ExposureIndex,"ExposureIndex"},
	{PTP_OPC_DisplayName,"DisplayName"},
	{PTP_OPC_BodyText,"BodyText"},
	{PTP_OPC_Subject,"Subject"},
	{PTP_OPC_Prority,"Prority"},
	{PTP_OPC_GivenName,"GivenName"},
	{PTP_OPC_MiddleNames,"MiddleNames"},
	{PTP_OPC_FamilyName,"FamilyName"},

	{PTP_OPC_Prefix,"Prefix"},
	{PTP_OPC_Suffix,"Suffix"},
	{PTP_OPC_PhoneticGivenName,"PhoneticGivenName"},
	{PTP_OPC_PhoneticFamilyName,"PhoneticFamilyName"},
	{PTP_OPC_EmailPrimary,"EmailPrimary"},
	{PTP_OPC_EmailPersonal1,"EmailPersonal1"},
	{PTP_OPC_EmailPersonal2,"EmailPersonal2"},
	{PTP_OPC_EmailBusiness1,"EmailBusiness1"},
	{PTP_OPC_EmailBusiness2,"EmailBusiness2"},
	{PTP_OPC_EmailOthers,"EmailOthers"},
	{PTP_OPC_PhoneNumberPrimary,"PhoneNumberPrimary"},
	{PTP_OPC_PhoneNumberPersonal,"PhoneNumberPersonal"},
	{PTP_OPC_PhoneNumberPersonal2,"PhoneNumberPersonal2"},
	{PTP_OPC_PhoneNumberBusiness,"PhoneNumberBusiness"},
	{PTP_OPC_PhoneNumberBusiness2,"PhoneNumberBusiness2"},
	{PTP_OPC_PhoneNumberMobile,"PhoneNumberMobile"},
	{PTP_OPC_PhoneNumberMobile2,"PhoneNumberMobile2"},
	{PTP_OPC_FaxNumberPrimary,"FaxNumberPrimary"},
	{PTP_OPC_FaxNumberPersonal,"FaxNumberPersonal"},
	{PTP_OPC_FaxNumberBusiness,"FaxNumberBusiness"},
	{PTP_OPC_PagerNumber,"PagerNumber"},
	{PTP_OPC_PhoneNumberOthers,"PhoneNumberOthers"},
	{PTP_OPC_PrimaryWebAddress,"PrimaryWebAddress"},
	{PTP_OPC_PersonalWebAddress,"PersonalWebAddress"},
	{PTP_OPC_BusinessWebAddress,"BusinessWebAddress"},
	{PTP_OPC_InstantMessengerAddress,"InstantMessengerAddress"},
	{PTP_OPC_InstantMessengerAddress2,"InstantMessengerAddress2"},
	{PTP_OPC_InstantMessengerAddress3,"InstantMessengerAddress3"},
	{PTP_OPC_PostalAddressPersonalFull,"PostalAddressPersonalFull"},
	{PTP_OPC_PostalAddressPersonalFullLine1,"PostalAddressPersonalFullLine1"},
	{PTP_OPC_PostalAddressPersonalFullLine2,"PostalAddressPersonalFullLine2"},
	{PTP_OPC_PostalAddressPersonalFullCity,"PostalAddressPersonalFullCity"},
	{PTP_OPC_PostalAddressPersonalFullRegion,"PostalAddressPersonalFullRegion"},
	{PTP_OPC_PostalAddressPersonalFullPostalCode,"PostalAddressPersonalFullPostalCode"},
	{PTP_OPC_PostalAddressPersonalFullCountry,"PostalAddressPersonalFullCountry"},
	{PTP_OPC_PostalAddressBusinessFull,"PostalAddressBusinessFull"},
	{PTP_OPC_PostalAddressBusinessLine1,"PostalAddressBusinessLine1"},
	{PTP_OPC_PostalAddressBusinessLine2,"PostalAddressBusinessLine2"},
	{PTP_OPC_PostalAddressBusinessCity,"PostalAddressBusinessCity"},
	{PTP_OPC_PostalAddressBusinessRegion,"PostalAddressBusinessRegion"},
	{PTP_OPC_PostalAddressBusinessPostalCode,"PostalAddressBusinessPostalCode"},
	{PTP_OPC_PostalAddressBusinessCountry,"PostalAddressBusinessCountry"},
	{PTP_OPC_PostalAddressOtherFull,"PostalAddressOtherFull"},
	{PTP_OPC_PostalAddressOtherLine1,"PostalAddressOtherLine1"},
	{PTP_OPC_PostalAddressOtherLine2,"PostalAddressOtherLine2"},
	{PTP_OPC_PostalAddressOtherCity,"PostalAddressOtherCity"},
	{PTP_OPC_PostalAddressOtherRegion,"PostalAddressOtherRegion"},
	{PTP_OPC_PostalAddressOtherPostalCode,"PostalAddressOtherPostalCode"},
	{PTP_OPC_PostalAddressOtherCountry,"PostalAddressOtherCountry"},
	{PTP_OPC_OrganizationName,"OrganizationName"},
	{PTP_OPC_PhoneticOrganizationName,"PhoneticOrganizationName"},
	{PTP_OPC_Role,"Role"},
	{PTP_OPC_Birthdate,"Birthdate"},
	{PTP_OPC_MessageTo,"MessageTo"},
	{PTP_OPC_MessageCC,"MessageCC"},
	{PTP_OPC_MessageBCC,"MessageBCC"},
	{PTP_OPC_MessageRead,"MessageRead"},
	{PTP_OPC_MessageReceivedTime,"MessageReceivedTime"},
	{PTP_OPC_MessageSender,"MessageSender"},
	{PTP_OPC_ActivityBeginTime,"ActivityBeginTime"},
	{PTP_OPC_ActivityEndTime,"ActivityEndTime"},
	{PTP_OPC_ActivityLocation,"ActivityLocation"},
	{PTP_OPC_ActivityRequiredAttendees,"ActivityRequiredAttendees"},
	{PTP_OPC_ActivityOptionalAttendees,"ActivityOptionalAttendees"},
	{PTP_OPC_ActivityResources,"ActivityResources"},
	{PTP_OPC_ActivityAccepted,"ActivityAccepted"},
	{PTP_OPC_Owner,"Owner"},
	{PTP_OPC_Editor,"Editor"},
	{PTP_OPC_Webmaster,"Webmaster"},
	{PTP_OPC_URLSource,"URLSource"},
	{PTP_OPC_URLDestination,"URLDestination"},
	{PTP_OPC_TimeBookmark,"TimeBookmark"},
	{PTP_OPC_ObjectBookmark,"ObjectBookmark"},
	{PTP_OPC_ByteBookmark,"ByteBookmark"},
	{PTP_OPC_LastBuildDate,"LastBuildDate"},
	{PTP_OPC_TimetoLive,"TimetoLive"},
	{PTP_OPC_MediaGUID,"MediaGUID"},
	{PTP_OPC_TotalBitRate,"TotalBitRate"},
	{PTP_OPC_BitRateType,"BitRateType"},
	{PTP_OPC_SampleRate,"SampleRate"},
	{PTP_OPC_NumberOfChannels,"NumberOfChannels"},
	{PTP_OPC_AudioBitDepth,"AudioBitDepth"},
	{PTP_OPC_ScanDepth,"ScanDepth"},
	{PTP_OPC_AudioWAVECodec,"AudioWAVECodec"},
	{PTP_OPC_AudioBitRate,"AudioBitRate"},
	{PTP_OPC_VideoFourCCCodec,"VideoFourCCCodec"},
	{PTP_OPC_VideoBitRate,"VideoBitRate"},
	{PTP_OPC_FramesPerThousandSeconds,"FramesPerThousandSeconds"},
	{PTP_OPC_KeyFrameDistance,"KeyFrameDistance"},
	{PTP_OPC_BufferSize,"BufferSize"},
	{PTP_OPC_EncodingQuality,"EncodingQuality"},
	{PTP_OPC_EncodingProfile,"EncodingProfile"},
	{PTP_OPC_BuyFlag,"BuyFlag"},
 */
