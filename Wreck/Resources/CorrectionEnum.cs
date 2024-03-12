
using System;

namespace Wreck.Resources
{
	/// <summary>
	/// CorrectionEnum.
	/// </summary>
	public class CorrectionEnum
	{
		public static readonly CorrectionEnum CREATION =
			new CorrectionEnum
			(
				"Creation",
				"Apply correction to file creation time attribute"
			);
		
		public static readonly CorrectionEnum MODIFIED =
			new CorrectionEnum
			(
				"Last modified",
				"Apply correction to last modified time attribute"
			);
		
		public static readonly CorrectionEnum ACCESSED =
			new CorrectionEnum
			(
				"Last accessed",
				"Apply correction to last accessed time attribute"
			);
		
		private static readonly CorrectionEnum[] values = 
			new CorrectionEnum[]
			{
				CREATION,
				MODIFIED,
				ACCESSED
			};
		
		public static CorrectionEnum[] Values
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
		
		CorrectionEnum(string name, string description)
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
