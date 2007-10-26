/***************************************************************************
 *  ImageFile.cs
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
	/// Represents an Image
	/// </summary>
	public class ImageFile : File
	{	
		/// <value>
		/// The bitdepth of the image
		/// </value>
		public int Depth
		{
			get { return GetInt ("ImageBitDepth"); }
			set { SetValue ("ImageBitDepth", value); }
		}
		
		/// <value>
		/// The exposure index of the image
		/// </value>
		public int ExposureIndex
		{
			get { return GetInt ("ExposureIndex"); }
			set { SetValue ("ExposureIndex", value); }
		}
		
		/// <value>
		/// The FNumber of the image
		/// </value>
		public int Fnumber
		{
			get { return GetInt ("Fnumber"); }
			set { SetValue ("Fnumber", value); }
		}
		
		/// <value>
		/// The exposure time of the image
		/// </value>
		public int ExposureTime
		{
			get { return GetInt ("ExposureTime"); }
			set { SetValue ("ExposureTime", value); }
		}
		
		/// <value>
		/// The height of the image in pixels
		/// </value>
		public int Height
		{
			get { return GetInt ("Height"); }
			set { SetValue ("Height", value); }
		}
		
		/// <value>
		/// The width of the image in pixels
		/// </value>
		public int Width
		{
			get { return GetInt ("Width"); }
			set { SetValue ("Width", value); }
		}
		
		internal ImageFile (Camera camera, FileSystem fs, string metadata, string directory, string filename, bool local)
			: base (camera, fs, metadata, directory, filename, local)
		{
			
		}
		
		/// <summary>
		/// Creates a new image file
		/// </summary>
		/// <param name="path">The path to the directory containing the image
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="filename">The filename of the image
		/// A <see cref="System.String"/>
		/// </param>
		public ImageFile (string path, string filename)
			: base (null, null, "", path, filename, true)
		{
			
		}
		
		/// <summary>
		/// Creates a new file from the supplied stream
		/// </summary>
		/// <param name="stream">The stream containing the file data
		/// A <see cref="Stream"/>
		/// </param>
//		public ImageFile(System.IO.Stream stream)
//			: base(stream)
//		{
//			
//		}
		
//		// FIXME: Return an image or something
//		public void CreateThumbnail()
//		{
//			
//		}
	}
}
