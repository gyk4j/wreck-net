
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

using Java.NIO.File;
using Sun.NIO.FS;

namespace Sun.NIO.FS
{
	/// <summary>
	/// Description of WindowsFileStore.
	/// </summary>
	public class WindowsFileStore : FileStore
	{
		private readonly string root;
		private readonly VolumeInformation volInfo;
		private readonly int volType;
		private readonly string displayName;   // returned by toString

//		private int hashCode;
		
		private static System.IO.DriveInfo driveInfo;
		private const int DRIVE_REMOVABLE = (int) System.IO.DriveType.Removable;
		
		private WindowsFileStore(string root)
		{
			Debug.Assert(root.ToCharArray()[root.Length-1] == '\\');
			this.root = root;
			this.volInfo = GetVolumeInformation(root);
			this.volType = GetDriveType(root);

			// file store "display name" is the volume name if available
			string vol = volInfo.VolumeName;
			if (!string.IsNullOrEmpty(vol))
			{
				this.displayName = vol;
			}
			else
			{
				// TBD - should we map all types? Does this need to be localized?
				this.displayName = (volType == DRIVE_REMOVABLE) ? "Removable Disk" : "";
			}
		}
		
		[DllImport("kernel32.dll")]
		private static extern long GetVolumeInformation(
			string PathName,
			StringBuilder VolumeNameBuffer,
			UInt32 VolumeNameSize,
			ref UInt32 VolumeSerialNumber,
			ref UInt32 MaximumComponentLength,
			ref UInt32 FileSystemFlags,
			StringBuilder FileSystemNameBuffer,
			UInt32 FileSystemNameSize);
		
		private static VolumeInformation GetVolumeInformation(string root)
		{
			// root must be of format: "X:\" where X is drive letter
			uint serialNumber = 0;
			uint maxComponentLength = 0;
			StringBuilder sbVolumeName = new StringBuilder(256);
			UInt32 fileSystemFlags = new UInt32();
			StringBuilder sbFileSystemName = new StringBuilder(256);
			
			long result = GetVolumeInformation(
				root,
				sbVolumeName,
				(UInt32)sbVolumeName.Capacity,
				ref serialNumber,
				ref maxComponentLength,
				ref fileSystemFlags,
				sbFileSystemName,
				(UInt32) sbFileSystemName.Capacity);
			
			VolumeInformation v = result == 0 ?
				new VolumeInformation(
					string.Empty,
					string.Empty,
					0,
					0) :
				new VolumeInformation(
					sbFileSystemName.ToString(),
					sbVolumeName.ToString(),
					(int) serialNumber,
					(int) fileSystemFlags);
			
			driveInfo = new System.IO.DriveInfo(root);
			return v;
		}
		
		private static int GetDriveType(string root)
		{
			return (int) driveInfo.DriveType;
		}
		
		static WindowsFileStore Create(string root, bool ignoreNotReady)
		{
			return new WindowsFileStore(root);
		}
		
		static WindowsFileStore Create(WindowsPath file)
		{
			// if the file is a link then GetVolumePathName returns the
			// volume that the link is on so we need to call it with the
			// final target
			string target = file.ToString();
			return CreateFromPath(target);
		}
		
		private static WindowsFileStore CreateFromPath(string target)
		{
			string root = GetVolumePathName(target);
			return new WindowsFileStore(root);
		}
		
		private static string GetVolumePathName(string target)
		{
			return System.IO.Path.GetPathRoot(target);
		}
		
		public override object GetAttribute(string attribute)
		{
			return driveInfo.RootDirectory.Attributes;
		}
		
//		public override V GetFileStoreAttributeView<V>(Type type) where V : FileStoreAttributeView;
		
		public override long GetTotalSpace()
		{
			return driveInfo.TotalSize;
		}
		public override long GetUnallocatedSpace()
		{
			return driveInfo.TotalFreeSpace;
		}
		
		public override long GetUsableSpace()
		{
			return driveInfo.AvailableFreeSpace;
		}
		
		public override bool IsReadOnly()
		{
			return (
				driveInfo.RootDirectory.Attributes &
				System.IO.FileAttributes.ReadOnly) ==
				System.IO.FileAttributes.ReadOnly;
		}
		
		public override string Name()
		{
			return driveInfo.Name;
		}
		
//		public override bool SupportsFileAttributeView<T>(Type type) where T : FileAttributeView;
		
		public override bool SupportsFileAttributeView(string name)
		{
			return false;
		}
		
		public override string Type()
		{
			string[] types = new string[]
			{
				"Unknown",
				"NoRootDirectory",
				"Removable",
				"Fixed",
				"Network",
				"CDRom",
				"Ram"
			};
			
			return types[(int) driveInfo.DriveType];
		}
	}
	
	class VolumeInformation
	{
		private string fileSystemName;
		private string volumeName;
		private int volumeSerialNumber;
		private int flags;
//		private VolumeInformation() { }
		
		public VolumeInformation(
			string fileSystemName,
			string volumeName,
			int volumeSerialNumber,
			int flags
		)
		{
			this.fileSystemName = fileSystemName;
			this.volumeName = volumeName;
			this.volumeSerialNumber = volumeSerialNumber;
			this.flags = flags;
		}

		public string FileSystemName      { get { return fileSystemName; } }
		public string VolumeName          { get { return volumeName; } }
		public int VolumeSerialNumber     { get { return volumeSerialNumber; } }
		public int Flags                  { get { return flags; } }
	}
}
