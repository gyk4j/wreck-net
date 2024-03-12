
using System;

namespace Wreck.Resources
{
	/// <summary>
	/// Description of SourceEnum.
	/// </summary>
	public class SourceEnum
	{
		public static readonly SourceEnum METADATA =
			new SourceEnum
			(
				"Metadata tags",
				"Use metadata tag values if available."
			);
		
		public static readonly SourceEnum FILE_SYSTEM =
			new SourceEnum
			(
				"File system attributes",
				"Use the earliest file system attribute time"
			);
		
		public static readonly SourceEnum CUSTOM =
			new SourceEnum
			(
				"Custom",
				"Use a user-specified date"
			);
		
		private static readonly SourceEnum[] values = 
			new SourceEnum[]
			{
				METADATA,
				FILE_SYSTEM,
				CUSTOM
			};
		
		public static SourceEnum[] Values
		{
			get { return values; }
		}
		
		private string name;
		public string Name
		{
			get {  return name; }
		}
		private string description;
		public string Description
		{
			get { return description; }
		}
		
		SourceEnum(string name, string description)
		{
			this.name = name;
			this.description = description;
		}
		
		public override string ToString()
		{
			return Name;
		}
	}
}
