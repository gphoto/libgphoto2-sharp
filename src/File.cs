using System;
using System.IO;
using System.Collections.Generic;

namespace Gphoto2
{
    public abstract class File
	{
		/// <value>
		/// True if the metadata has been changed and needs to be updated on the device
		/// </value>
		public bool Dirty
		{
			get { return false; }
			protected set { }
		}
		
		/// <value>
		/// The fully qualified path to the file
		/// </value>
		public string Path
		{
			get { return null; }
		}
		
		/// <value>
		///  The size of the file in bytes
		/// </value>
		public int Size
		{
			get { return -1; }
		}
		
		/// <summary>
		/// Reads the entire file and returns it as a byte array
		/// </summary>
		/// <returns>
		/// A <see cref="System.Byte"/>
		/// </returns>
		public byte[] Download()
		{
			return null;
		}
		
		/// <summary>
		/// Reads the entire file and writes it to the supplied stream
		/// </summary>
		public void Download(Stream stream)
		{
			byte[] data = Download();
			stream.Write(data, 0, data.Length);
		}
		
		/// <summary>
		/// Updates the metadata of the file on the camara
		/// </summary>
		/// <param name="file">
		/// A <see cref="Base.CameraFile"/>
		/// </param>
		/// <returns>
		/// A <see cref="File"/>
		/// </returns>
		public virtual void Update()
		{
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="file">
		/// A <see cref="Base.CameraFile"/>
		/// </param>
		/// <returns>
		/// A <see cref="File"/>
		/// </returns>
		internal static File Create(Base.CameraFile file)
		{
			string mime = file.GetMimeType();
			string metadata = System.Text.Encoding.UTF8.GetString(file.GetDataAndSize());
			
			switch(mime)
			{
			case Base.MimeTypes.ASF:
			case Base.MimeTypes.MP3:
			case Base.MimeTypes.WMA:
			case Base.MimeTypes.WAV:
			case Base.MimeTypes.OGG:
				return new MusicFile(metadata);
				

				// FIXME: Return a 'generic' file
			default:
				Console.WriteLine("Can't parse: {0}", mime);
				return null;
			}
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