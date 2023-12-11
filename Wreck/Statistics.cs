
using System;
using System.IO;

namespace Wreck
{
	/// <summary>
	/// Description of Struct1.
	/// </summary>
	public struct Statistics : IEquatable<Statistics>
	{
		public int Directories { get; set; }
		public int Files { get; set; }
		public int Skipped { get; set; }
		
		public int Count(FileInfo f)
		{
			return ++this.Files;
		}
		
		public int Count(DirectoryInfo d)
		{
			return ++this.Directories;
		}
		
		public int Skip(FileInfo f)
		{
			return ++this.Skipped;
		}
		
		public int Skip(DirectoryInfo d)
		{
			return ++this.Skipped;
		}
		
		#region Equals and GetHashCode implementation
		// The code in this region is useful if you want to use this structure in collections.
		// If you don't need it, you can just remove the region and the ": IEquatable<Struct1>" declaration.
		
		public override bool Equals(object obj)
		{
			if (obj is Statistics)
				return Equals((Statistics)obj); // use Equals method below
			else
				return false;
		}
		
		public bool Equals(Statistics other)
		{
			
			return 
				this.Files == other.Files &&
				this.Directories == other.Directories &&
				this.Skipped == other.Skipped;
		}
		
		public override int GetHashCode()
		{
			return 
				this.Files.GetHashCode() ^
				this.Directories.GetHashCode() ^ 
				this.Skipped.GetHashCode();
		}
		
		public static bool operator ==(Statistics left, Statistics right)
		{
			return left.Equals(right);
		}
		
		public static bool operator !=(Statistics left, Statistics right)
		{
			return !left.Equals(right);
		}
		#endregion
	}
}
