
using System;
using System.Drawing;

namespace Wreck.Resources
{
	/// <summary>
	/// Description of R.
	/// </summary>
	public class R
	{
		public static class Strings
		{
			public const string AppTitle = "WRECK";
			public const string PropertyState = "state";
			public const string PropertyProgress = "progress";
			public const string PropertyVisits = "visits";
			public const string PropertyBean = "bean";
			public const string SkipDesktopIni = "desktop.ini";
			public const string _7zLastModifiedTime = "7z:lastModifiedTime";
			public const string _7zCreationTime = "7z:creationTime";
			public const string PathName = "path:name";
			public const string Files= "Files";
			public const string Statistics = "Statistics";
			public const string Error = "Error";
			public const string OK = "OK";
			public const string Starting = "Starting";
			public const string Empty = "";
			public const string ForecastedCorrections = "Forecasted Corrections";
			public const string Timestamps = "Timestamps";
			public const string FileCount = "File Count";
			public const string Creation = "Creation";
			public const string LastModified = "Last Modified";
			public const string LastAccessed = "Last Accessed";
			public const string BackupCreationTime = "backup:creationTime";
			public const string BackupLastModifiedTime = "backup:lastModifiedTime";
			public const string BackupLastAccessedTime = "backup:lastAccessedTime";
			public const string FsCreation = "fs:creation";
			public const string FsModified = "fs:modified";
			public const string UserCustomDateTime = "user:customDateTime";
			public const string ZipLastModifiedTime = "zip:lastModifiedTime";
			public const string CsvCreation = "csv:creation";
			public const string CsvModified = "csv:modified";
			public const string CsvAccessed = "csv:accessed";
			public const string DirEarliest = "dir:earliest";
			public const string DirLatest = "dir:latest";
			public const string LogFileName = "META_INF.CSV";
			public const string ActionAnalyze = "Analyze";
			public const string ActionBackup = "Backup";
			public const string ActionRestore = "Restore";
			public const string ActionVerify = "Verify";
		}
		
		public static class Integer
		{
			public const int Border04 = 4;
			public const int Border08 = 8;
			public const int Border16 = 16;
			public const int Border32 = 32;
		}
		
		public static class Icon
		{
			public const string App = "app.png";
		}
		
		public static class Style
		{
			/* Java Swing-specific objects. Not required for WinForms.
			public const Border BorderDebug = BorderFactory.CreateLineBorder(Color.CYAN, 2);
			public const Border BorderBevelLowerSoft = BorderFactory.CreateLoweredSoftBevelBorder();
			public const Border BorderEmpty4 = BorderFactory.CreateEmptyBorder(
				R.Integer.Border4, R.Integer.Border4, R.Integer.Border4, R.Integer.Border4);
			public const Border BorderEmpty8 = BorderFactory.CreateEmptyBorder(
				R.Integer.Border8, R.Integer.Border8, R.Integer.Border8, R.Integer.Border8);
			public const Border BorderForecast = BorderFactory.CreateEmptyBorder(
				R.Integer.Border32, 0, R.Integer.Border32, 0);
			public const Border BorderEmpty8Left = BorderFactory.CreateEmptyBorder(
				0, R.Integer.Border8, 0, 0);
			public const Border BorderBevelLowerEmpty8 = BorderFactory.CreateCompoundBorder(
				BorderFactory.CreateEmptyBorder(
					R.Integer.Border8, R.Integer.Border8, R.Integer.Border8, R.Integer.Border8),
				BorderFactory.CreateBevelBorder(BevelBorder.Lowered));
			*/
		}
		
		public static class Dimen
		{
			public const int FrameWidth = 480;
			public const int FrameHeight = 640;
			public const int DialogWidth = 360;
			public const int DialogHeight = 512;
			public const int TableRowHeight = 20;
		}
		
		public static class Color
		{
			public static readonly System.Drawing.Color TableGridColor = System.Drawing.Color.FromArgb(0xF0, 0xF0, 0xF0);
			public static readonly System.Drawing.Color CorrectionRequired = System.Drawing.Color.FromArgb(210, 202, 0);
			public static readonly System.Drawing.Color CorrectionNonRequired = System.Drawing.Color.FromArgb(0, 211, 40);
			public static readonly System.Drawing.Color CorrectionNoMetadata = System.Drawing.Color.FromArgb(210, 0, 0);
		}
	}
}
