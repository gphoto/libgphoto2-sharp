// ErrorCode.cs created with MonoDevelop
// User: alan at 00:11 21/10/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Gphoto2
{
	/// <summary>
	/// Represents all the possible errorcodes that can be
	/// returned by libgphoto2
	/// </summary>
	public enum ErrorCode
	{
		/* libgphoto2_port errors */
		GeneralError		= -1,
		BadParameters		= -2,
		NoMemory			= -3,
		Library				= -4,
		UnknownPort			= -5,
		NotSupported		= -6,
		IO					= -7,
		Timout				= -10,
		SupportedSerial		= -20,
		SupportedUSB		= -21,
		Init				= -31,
		Read				= -34,
		Write				= -35,
		Update				= -37,
		SerialSpeed			= -41,
		USBClearHalt		= -51,
		USBFind				= -52,
		USBClaim			= -53,
		Lock				= -60,
		Hal					= -70,

		/* libgphoto2 errors */
		CorruptedData		= -102,
		FileExists			= -103,
		ModelNotFound		= -105,
		DirectoryNotFound	= -107,
		FileNotFound		= -108,
		DirectoryExists		= -109,
		CameraBusy			= -110,
		PathNotAbsolute		= -111,
		Cancel				= -112,
		CameraError			= -113,
		OsFailure			= -114
	}
}
