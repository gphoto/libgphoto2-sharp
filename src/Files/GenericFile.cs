/***************************************************************************
 *  GenericFile.cs
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
	/// Represents a generic file on the camera. This is used to represent a file that isn't an
	/// image, playlist or music file
	/// </summary>
	public class GenericFile : File
	{
		internal GenericFile (Camera camera, FileSystem fs, string metadata, string path, string filename, bool local)
			: base (camera, fs, metadata, path, filename, local)
		{
			
		}
		
		/// <summary>
		/// Creates a new generic file
		/// </summary>
		/// <param name="path">The path to the file
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="filename">The filename
		/// A <see cref="System.String"/>
		/// </param>
		public GenericFile (string path, string filename)
			: base (null, null, "", path, filename, true)
		{
			
		}
		
		/// <summary>
		/// Creates a new file from the supplied stream
		/// </summary>
		/// <param name="stream">The stream containing the file data
		/// A <see cref="Stream"/>
		/// </param>
//		public GenericFile(System.IO.Stream stream)
//			: base(stream)
//		{
//			
//		}
	}
}
