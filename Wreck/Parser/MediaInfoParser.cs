
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;

using log4net;
using log4net.Config;

using MediaInfoLib;

namespace Wreck.Parser
{
	/// <summary>
	/// Description of MediaInfo.
	/// </summary>
	public class MediaInfoParser : IFileDateable
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(MediaInfoParser));
		
		public void TestMediaInfoLoad()
		{
			//Test if version of DLL is compatible : 3rd argument is "version of DLL tested;Your application name;Your application version"
			String ToDisplay;
			MediaInfo MI = new MediaInfo();

			ToDisplay = MI.Option("Info_Version", "0.7.0.0;MediaInfoDLL_Example_CS;0.7.0.0");
			if (ToDisplay.Length == 0)
			{
				Console.WriteLine("MediaInfo.Dll: this version of the DLL is not compatible");
				return;
			}

			//Information about MediaInfo
			ToDisplay += "\r\n\r\nInfo_Parameters\r\n";
			ToDisplay += MI.Option("Info_Parameters");

			ToDisplay += "\r\n\r\nInfo_Capacities\r\n";
			ToDisplay += MI.Option("Info_Capacities");

			ToDisplay += "\r\n\r\nInfo_Codecs\r\n";
			ToDisplay += MI.Option("Info_Codecs");

			//An example of how to use the library
			ToDisplay += "\r\n\r\nOpen\r\n";
			MI.Open("Example.ogg");

			ToDisplay += "\r\n\r\nInform with Complete=false\r\n";
			MI.Option("Complete");
			ToDisplay += MI.Inform();

			ToDisplay += "\r\n\r\nInform with Complete=true\r\n";
			MI.Option("Complete", "1");
			ToDisplay += MI.Inform();

			ToDisplay += "\r\n\r\nCustom Inform\r\n";
			MI.Option("Inform", "General;File size is %FileSize% bytes");
			ToDisplay += MI.Inform();

			ToDisplay += "\r\n\r\nGet with Stream=General and Parameter='FileSize'\r\n";
			ToDisplay += MI.Get(0, 0, "FileSize");

			ToDisplay += "\r\n\r\nGet with Stream=General and Parameter=46\r\n";
			ToDisplay += MI.Get(0, 0, 46);

			ToDisplay += "\r\n\r\nCount_Get with StreamKind=Stream_Audio\r\n";
			ToDisplay += MI.Count_Get(StreamKind.Audio);

			ToDisplay += "\r\n\r\nGet with Stream=General and Parameter='AudioCount'\r\n";
			ToDisplay += MI.Get(StreamKind.General, 0, "AudioCount");

			ToDisplay += "\r\n\r\nGet with Stream=Audio and Parameter='StreamCount'\r\n";
			ToDisplay += MI.Get(StreamKind.Audio, 0, "StreamCount");

			ToDisplay += "\r\n\r\nClose\r\n";
			MI.Close();

			//Example with a stream
			//ToDisplay+="\r\n"+ExampleWithStream()+"\r\n";

			//Displaying the text
			Console.WriteLine(ToDisplay);
		}
		
		public Dictionary<string, string> ExampleWithStream(string path, string[] parameters)
		{
			//Initilaizing MediaInfo
			MediaInfo MI = new MediaInfo();

			//From: preparing an example file for reading
			FileStream From = new FileStream(path, FileMode.Open, FileAccess.Read);

			//From: preparing a memory buffer for reading
			byte[] From_Buffer = new byte[64*1024];
			int    From_Buffer_Size; //The size of the read file buffer

			//Preparing to fill MediaInfo with a buffer
			MI.Open_Buffer_Init(From.Length, 0);

			//The parsing loop
			do
			{
				//Reading data somewhere, do what you want for this.
				From_Buffer_Size = From.Read(From_Buffer, 0, 64 * 1024);

				//Sending the buffer to MediaInfo
				System.Runtime.InteropServices.GCHandle GC = System.Runtime.InteropServices.GCHandle.Alloc(From_Buffer, System.Runtime.InteropServices.GCHandleType.Pinned);
				IntPtr From_Buffer_IntPtr = GC.AddrOfPinnedObject();
				Status Result = (Status)MI.Open_Buffer_Continue(From_Buffer_IntPtr, (IntPtr)From_Buffer_Size);
				GC.Free();
				if ((Result & Status.Finalized) == Status.Finalized)
					break;

				//Testing if MediaInfo request to go elsewhere
				if (MI.Open_Buffer_Continue_GoTo_Get() != -1)
				{
					Int64 Position = From.Seek(MI.Open_Buffer_Continue_GoTo_Get(), SeekOrigin.Begin); //Position the file
					MI.Open_Buffer_Init(From.Length, Position); //Informing MediaInfo we have seek
				}
			}
			while (From_Buffer_Size > 0);

			//Finalizing
			MI.Open_Buffer_Finalize(); //This is the end of the stream, MediaInfo must finnish some work

			//Get() example
			Dictionary<string, string> kvp = new Dictionary<string, string>();
			foreach(string parameter in parameters)
			{
				kvp.Add(parameter, MI.Get(StreamKind.General, 0, parameter));
			}
			return kvp;
		}
		
		public void GetDateTimes(
			FileInfo fi,
			out DateTime? creationTime,
			out DateTime? lastWriteTime,
			out DateTime? lastAccessTime)
		{
			MediaInfo MI = new MediaInfo();
			
			String ifLoaded = MI.Option(string.Format("Info_Version", "0.7.0.0;{0};0.7.0.0", Wreck.NAME));
			if (ifLoaded.Length == 0 || "Unable to load MediaInfo library".Equals(ifLoaded))
			{
				log.Error("Failed to load MediaInfo.Dll or this version of the DLL is not compatible");
				throw new ApplicationException("Failed to load MediaInfo");
//				creationTime = lastWriteTime = lastAccessTime = DateTime.Now;
//				return;
			}
			else
			{
				log.Info(ifLoaded);
			}
			
			MI.Open(fi.FullName);
			string encodedDate;
			
			encodedDate = MI.Get(StreamKind.General, 0, "Encoded_Date").Trim();
			if(encodedDate.Equals(string.Empty))
				encodedDate = MI.Get(StreamKind.General, 0, "Tagged_Date").Trim();
			
			MI.Close();
			
			DateTime? t = null;
			
			if(!encodedDate.Equals(string.Empty))
			{
				DateTimeStyles style = encodedDate.StartsWith("UTC")?
					DateTimeStyles.AssumeUniversal: DateTimeStyles.AssumeLocal;
				
				encodedDate = encodedDate.Replace("UTC", "").Trim();
				try
				{
					t = DateTime.ParseExact(
						encodedDate,
						"yyyy-MM-dd HH:mm:ss", // format
						CultureInfo.InvariantCulture,
						style);
				}
				catch(FormatException)
				{
					log.DebugFormat("{0} in unexpected format.", encodedDate);
				}
				
			}
			
			creationTime = t;
			lastWriteTime = t;
			lastAccessTime = t;
		}
	}
}
