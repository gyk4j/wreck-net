
using System;

namespace Wreck.IO.Reader
{
	/// <summary>
	/// Description of ReaderFactory.
	/// </summary>
	public class ReaderFactory : IDisposable
	{
		private static ReaderFactory instance = null;
		
//		private final ExifToolReader exifToolReader;
//		private final MediaInfoReader mediaInfoReader;
//		private final SevenZipReader sevenZipReader;
//		private final PathReader pathReader;
//		
//		private final FileSystemReader fileSystemReader;
//		private final CustomDateTimeReader customDateTimeReader;
		
		public static bool IsInitialized()
		{
			return instance != null;
		}
	
		private ReaderFactory()
		{
			// Primary embedded metadata readers
//			exifToolReader = new ExifToolReader();
//			mediaInfoReader = new MediaInfoReader();
//			sevenZipReader = new SevenZipReader();
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
//			exifToolReader.Dispose();
//			mediaInfoReader.Dispose();
//			sevenZipReader.Dispose();
//			pathReader.Dispose();
//			
//			fileSystemReader.Dispose();
//			customDateTimeReader.Dispose();
		}
	}
}
