
using System;
using System.Diagnostics;

using SevenZip;
using System.IO;

namespace Wreck.Parser
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class SevenZipParser : IFileDateable
	{
		public static readonly string[] SUPPORTED_FORMATS =
		{
			"7z",
			"bz2", "bzip2", "tbz2", "tbz",
			"gz", "gzip", "tgz",
			"tar",
			"wim",
			"xz", "txz",
			"zip", "zipx", "jar", "xpi", "odt", "ods", "docx", "xlsx", "epub",
			"apm",
			"ar", "a", "deb", "lib",
			"arj",
			"cab",
			"chm", "chw", "chi", "chq",
			//"msi", "msp", "doc", "xls", "ppt",
			"cpio",
			"cramfs",
			"dmg",
			"ext", "ext2", "ext3", "ext4", "img",
			"fat", "img",
			"hfs", "hfsx",
			"hxs", "hxi", "hxr", "hxq", "hxw", "lit",
			"ihex",
			"iso", "img",
			"lzh", "lha",
			"lzma",
			"mbr",
			"mslz",
			"mub",
			"nsis",
			"ntfs", "img",
			"mbr",
			"rar", "r00",
			"rpm",
			"ppmd",
			"qcow", "qcow2", "qcow2c",
			"001",
			"squashfs",
			"udf", "iso", "img",
			"scap",
			"uefif",
			"vdi",
			"vhd",
			"vmdk",
			"wim", "esd",
			"xar", "pkg",
			"z", "taz"
		};
		
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
		
		private void Enumerate(FileInfo fi, IArchiveEntryEnumerator e)
		{
			if(Array.IndexOf(SUPPORTED_FORMATS, fi.Extension.Remove(0, 1).ToLower()) < 0)
			{
				Debug.Print("Skipped unsupported format: {0} ", fi.FullName);
				return;
			}
			
			string password = PasswordProvider.GetPassword(fi);
			
			using (SevenZipExtractor sze = password.Equals(string.Empty)?
			       new SevenZipExtractor(fi.FullName):
			       new SevenZipExtractor(fi.FullName, password))
			{
				for (int i = 0; i < sze.ArchiveFileData.Count; i++)
				{
					ArchiveFileInfo afi = sze.ArchiveFileData[i];
					e.OnArchiveFileInfo(afi);
				}
			}
		}
		
		public void List(string path)
		{
			FileInfo fi = new FileInfo(path);
			ArchiveFileInfoPrinter afip = new ArchiveFileInfoPrinter();
			Enumerate(fi, afip);
		}
		
		public void GetDateTimes(
			FileInfo fi,
			out DateTime? creationTime,
			out DateTime? lastWriteTime,
			out DateTime? lastAccessTime)
		{
			ArchiveFileInfoDater afid = new ArchiveFileInfoDater();
			Enumerate(fi, afid);
			
			creationTime = afid.Latest;
			lastWriteTime = afid.Latest;
			lastAccessTime = afid.Latest;
			
			if(afid.Earliest.HasValue || afid.Latest.HasValue)
			{
				Debug.Print(
					"[SevenZipParser] Earliest = {0}",
					afid.Earliest.HasValue? afid.Earliest.Value.ToString(): "?");
				Debug.Print(
					"[SevenZipParser] Latest = {0}",
					afid.Latest.HasValue? afid.Latest.Value.ToString(): "?");
			}
		}
	}
	
	interface IArchiveEntryEnumerator
	{
		void OnArchiveFileInfo(ArchiveFileInfo afi);
	}
	
	class ArchiveFileInfoPrinter : IArchiveEntryEnumerator
	{
		public void OnArchiveFileInfo(ArchiveFileInfo afi)
		{
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
	
	class ArchiveFileInfoDater : IArchiveEntryEnumerator
	{
		private DateTime? earliest;
		private DateTime? latest;
		
		public ArchiveFileInfoDater()
		{
			this.earliest = null;
			this.latest = null;
		}
		
		public DateTime? Earliest
		{
			get
			{
				return earliest;
			}
			
			set
			{
				earliest = (!earliest.HasValue || value.Value.CompareTo(earliest.Value) < 0)?
					value: earliest.Value;
			}
		}
		
		public DateTime? Latest
		{
			get
			{
				return latest;
			}
			
			set
			{
				latest = (!latest.HasValue || value.Value.CompareTo(latest.Value) > 0)?
					value: latest.Value;
			}
		}
		
		public void OnArchiveFileInfo(ArchiveFileInfo afi)
		{
			/*
			Debug.Print("{0} {1} {2} {3}",
			            afi.CreationTime,
			            afi.LastWriteTime,
			            afi.LastAccessTime,
			            afi.FileName);
			 */
			
			// From testing, the only guaranteed field that has valid date time
			// value is LastWriteTime (aka Modified), but even that may not
			// always be available. For some files/directories, they can be
			// blank/empty, and 7-Zip will return current date time during
			// runtime.
			
			TimeSpan diff = DateTime.Now - afi.LastWriteTime;
			if(diff.Minutes >= 1)
			{
				this.Earliest = afi.LastWriteTime;
				this.Latest = afi.LastWriteTime;
				
				// 7-Zip will return current time as placeholder when there is
				// no valid date time metadata.
				// Only use the CreationTime when it appears to return a valid
				// date time value.
				if(afi.CreationTime.CompareTo(afi.LastWriteTime) < 0)
				{
					this.Earliest = afi.CreationTime;
					this.Latest = afi.CreationTime;
				}
				
				// LastAccessTime never seems to provide anything except current
				// date time. So it is ignored.
			}	
		}
	}
	
	class PasswordProvider
	{
		public static string GetPassword(FileInfo fi)
		{
			return GetPassword(fi.FullName);
		}
		
		public static string GetPassword(string path)
		{
			Debug.Print("Password for {0}", path);
			return string.Empty;
		}
	}
}
