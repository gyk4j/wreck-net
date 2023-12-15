
using System;
using System.Diagnostics;

using SevenZip;

namespace Wreck.Parser
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class SevenZipParser
	{
		public void GetVersion()
		{
			FileVersionInfo libraryVersion = SevenZipExtractor.GetLibraryVersion();
            Console.WriteLine(libraryVersion);
		}
		
		public void GetFeatures()
		{
			LibraryFeature features = SevenZipExtractor.CurrentLibraryFeatures;
            Console.WriteLine("Features: {0}{2}{1}", ((uint)features).ToString("X6"), features, Environment.NewLine);
		}
		
		public void List(string path)
		{
			using (SevenZipExtractor tmp = new SevenZipExtractor(path))
			{
				for (int i = 0; i < tmp.ArchiveFileData.Count; i++)
				{
					ArchiveFileInfo afi = tmp.ArchiveFileData[i];
					
					if(afi.IsDirectory)
					{
						Console.WriteLine("{0}", afi.FileName);
					}
					else
					{
						Console.WriteLine(
							"\t{0,8:#;#;0} {1}",
							(afi.Size <= 99999999)? afi.Size: 99999999,
							afi.FileName.Substring(afi.FileName.LastIndexOf('\\')+1)
						);
					}
				}
			}
		}
	}
}
