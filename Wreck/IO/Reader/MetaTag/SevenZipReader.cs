﻿
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using log4net;
using SevenZip;
using Wreck.Resources;
using Wreck.Security;

namespace Wreck.IO.Reader.MetaTag
{
	/// <summary>
	/// Description of SevenZipReader.
	/// </summary>
	public class SevenZipReader : AbstractTimestampReader
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(SevenZipReader));
		
		protected static readonly string[] MODIFIED =
		{
			R.Strings._7zLastModifiedTime
		};
		
		protected static readonly string[] SEVENZIP_EXTENSIONS =
		{
			".7z", ".zip", ".rar", ".001", ".cab", ".iso", ".xz", ".txz",
			".lzma", ".tar", ".cpio", ".bz2", ".bzip2", ".tbz2", ".tbz", ".gz",
			".gzip", ".tgz", ".tpz", ".z", ".taz", ".lzh", ".lha", ".rpm",
			".deb", ".arj", ".vhd", ".wim", ".swm", ".fat", ".ntfs", ".dmg",
			".hfs", ".xar", ".squashfs",
			// custom additions for zip or rar derived extensions
			".cbr", ".cbz", ".cbt", ".cba", ".cb7",
			".jar", ".ear", ".war",
//			".docx", ".docm", ".pptx", ".pptm", ".xlsx", ".xlsm",
			".3mf", ".dwfx", ".amlx", ".cddx", ".familyx", ".fdix", ".appv",
			".pbix", ".pbit", ".semblio", ".vsix", ".vsdx", ".appx",
			".appxbundle", ".cspkg", ".xps", ".mmzx", ".nupkg", ".oxps",
			".aasx", ".jtx", ".slx", ".smpk", ".scdoc",
//			".odt", ".fodt", ".ods", ".fods", ".odp", ".fodp", ".odg",
//			".fodg", ".odf",
			".xpi"
		};
		
		public static readonly List<string> SUPPORTED_EXTENSIONS = new List<string>(SEVENZIP_EXTENSIONS);
		
		private static PasswordProvider passwordProvider;
		
		public SevenZipReader() : base()
		{
			passwordProvider = PasswordProvider.Instance;
			
			FileVersionInfo libraryVersion = SevenZipExtractor.GetLibraryVersion();
			LibraryFeature features = SevenZipExtractor.CurrentLibraryFeatures;
			
			LOG.InfoFormat("7-Zip v{0}", libraryVersion.FileVersion);
		}
		
		private static bool IsSupportedFileExtension(FileSystemInfo file)
		{
			string extension = file.Extension.ToLower();
			return SUPPORTED_EXTENSIONS.Contains(extension);
		}
		
		public override string[] Modified()
		{
			return MODIFIED;
		}
		
		public override void Extract(FileSystemInfo file, List<Metadata> metadata)
		{
			if(!File.Exists(file.FullName))
			{
//				LOG.DebugFormat("Skipped non-file: {0}", file.FullName);
				return;
			}
			else if(!IsSupportedFileExtension(file))
			{
//				LOG.DebugFormat("Skipped unsupported format: {0}", file.Name);
				return;
			}
			
			try
			{
				using(SevenZipExtractor sze = Get7ZipExtractor((FileInfo) file))
				{
					DateTime modified = DateTime.MinValue;
					for (int i = 0; i < sze.ArchiveFileData.Count; i++)
					{
						ArchiveFileInfo afi = sze.ArchiveFileData[i];
						
						if(afi.LastWriteTime == null)
							continue;
						
						modified = modified.CompareTo(afi.LastWriteTime) > 0?
							modified: afi.LastWriteTime;
					}
					
					Add(metadata, R.Strings._7zLastModifiedTime, modified.ToString(), modified);
				}
				
//				LOG.DebugFormat("OK {0}", file.Name);
			}
			catch(SevenZipLibraryException ex)
			{
				throw new ApplicationException("Failed to load 7z.dll", ex);
			}
			catch(SevenZipArchiveException ex)
			{
				LOG.ErrorFormat("{0}: {1}", file.Name, ex.Message);
			}
			catch(ArgumentException ex)
			{
				LOG.ErrorFormat("{0}: {1}", file.Name, ex.Message);
//				throw new ArgumentException(ex.Message, ex);
			}
		}
		
		private SevenZipExtractor Get7ZipExtractor(FileInfo file)
		{
			SevenZipExtractor sze;
			
			string password = passwordProvider.GetPassword(file);
			
			// HACK: Specify archive format for unrecognized extensions.
			if(file.Extension.EndsWith(".cbr"))
			{
				// Format is specified. Tell 7-Zip to treat file as certain format.
				InArchiveFormat rar = InArchiveFormat.Rar;
				
				sze = string.IsNullOrEmpty(password)?
					new SevenZipExtractor(file.FullName, rar):
					new SevenZipExtractor(file.FullName, password, rar);
			}
			else
			{
				// If no format is specified, use auto-detection by 7-Zip library.
				sze = string.IsNullOrEmpty(password)?
					new SevenZipExtractor(file.FullName):
					new SevenZipExtractor(file.FullName, password);
			}
			
			return sze;
		}
	}
}
