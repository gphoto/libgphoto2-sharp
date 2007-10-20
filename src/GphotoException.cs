/***************************************************************************
 *  GphotoException.cs
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
	public class GPhotoException : Exception
    {
		private ErrorCode error;
		
		public GPhotoException(ErrorCode error_code)
			: base ("Unknown Error")
        {
			error = error_code;
		}
		
		public GPhotoException (ErrorCode error_code, string message)
			: base (message)
		{
			error = error_code;
		}
		
		public override string ToString()
		{
			return ("Error: " + error.ToString() + ": " + base.ToString());
		}
		
		public ErrorCode Error
		{
			get { return error; }
		}
	}
}
