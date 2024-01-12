
using System;
using Wreck.IO.Reader.MetaTag;

namespace Wreck.IO.Reader
{
	/// <summary>
	/// Description of ReaderFactory.
	/// </summary>
	public class ReaderFactory : IDisposable
	{
		private static ReaderFactory instance = null;
		
		private readonly ExifToolReader exifToolReader;
		private readonly MediaInfoReader mediaInfoReader;
		private readonly SevenZipReader sevenZipReader;
//		private readonly PathReader pathReader;
//		
//		private readonly FileSystemReader fileSystemReader;
//		private readonly CustomDateTimeReader customDateTimeReader;
		
		public static bool IsInitialized()
		{
			return instance != null;
		}
	
		private ReaderFactory()
		{
			// Primary embedded metadata readers
			exifToolReader = new ExifToolReader();
			mediaInfoReader = new MediaInfoReader();
			sevenZipReader = new SevenZipReader();
//			pathReader = new PathReader();
			
			// Secondary backup readers
//			fileSystemReader = new FileSystemReader();
//			customDateTimeReader = new CustomDateTimeReader();
		}
		
		public static ReaderFactory Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new ReaderFactory();
				}
				return instance;
			}
		}
		
		public void Dispose()
		{
			exifToolReader.Dispose();
			mediaInfoReader.Dispose();
			sevenZipReader.Dispose();
//			pathReader.Dispose();
//			
//			fileSystemReader.Dispose();
//			customDateTimeReader.Dispose();
			
			instance = null;
		}
		
		public ExifToolReader ExifToolReader
		{
			get { return exifToolReader; }
		}
		
		public MediaInfoReader MediaInfoReader
		{
			get { return MediaInfoReader; }
		}
		
		public SevenZipReader SevenZipReader
		{
			get { return sevenZipReader; }
		}
	}
}
