
using System;
using System.Drawing;

namespace Wreck.Resources
{
	/// <summary>
	/// Description of R.
	/// </summary>
	public class R
	{
		public static class strings
		{
			public const string APP_TITLE = "WRECK";
			public const string PROPERTY_STATE = "state";
			public const string PROPERTY_PROGRESS = "progress";
			public const string PROPERTY_VISITS = "visits";
			public const string PROPERTY_BEAN = "bean";
			public const string SKIP_DESKTOP_INI = "desktop.ini";
			public const string _7Z_LAST_MODIFIED_TIME = "7z:lastModifiedTime";
			public const string _7Z_CREATION_TIME = "7z:creationTime";
			public const string PATH_NAME = "path:name";
			public const string FILES= "Files";
			public const string STATISTICS = "Statistics";
			public const string ERROR = "Error";
			public const string OK = "OK";
			public const string STARTING = "Starting";
			public const string EMPTY = "";
			public const string FORECASTED_CORRECTIONS = "Forecasted Corrections";
			public const string TIMESTAMPS = "Timestamps";
			public const string FILE_COUNT = "File Count";
			public const string CREATION = "Creation";
			public const string LAST_MODIFIED = "Last Modified";
			public const string LAST_ACCESSED = "Last Accessed";
			public const string BACKUP_CREATION_TIME = "backup:creationTime";
			public const string BACKUP_LAST_MODIFIED_TIME = "backup:lastModifiedTime";
			public const string BACKUP_LAST_ACCESSED_TIME = "backup:lastAccessedTime";
			public const string FS_CREATION = "fs:creation";
			public const string FS_MODIFIED = "fs:modified";
			public const string USER_CUSTOM_DATE_TIME = "user:customDateTime";
			public const string ZIP_LAST_MODIFIED_TIME = "zip:lastModifiedTime";
			public const string CSV_CREATION = "csv:creation";
			public const string CSV_MODIFIED = "csv:modified";
			public const string CSV_ACCESSED = "csv:accessed";
			public const string DIR_EARLIEST = "dir:earliest";
			public const string DIR_LATEST = "dir:latest";
			public const string LOG_FILE_NAME = "META_INF.CSV";
			public const string ACTION_ANALYZE = "Analyze";
			public const string ACTION_BACKUP = "Backup";
			public const string ACTION_RESTORE = "Restore";
			public const string ACTION_VERIFY = "Verify";
		}
		
		public static class integer
		{
			public const int BORDER_4 = 4;
			public const int BORDER_8 = 8;
			public const int BORDER_16 = 16;
			public const int BORDER_32 = 32;
		}
		
		public static class icon
		{
			public const string APP = "app.png";
		}
		
		public static class style
		{
			/* Java Swing-specific objects. Not required for WinForms.
			public const Border BORDER_DEBUG = BorderFactory.createLineBorder(Color.CYAN, 2);
			public const Border BORDER_BEVEL_LOWER_SOFT = BorderFactory.createLoweredSoftBevelBorder();
			public const Border BORDER_EMPTY_4 = BorderFactory.createEmptyBorder(
				R.integer.BORDER_4, R.integer.BORDER_4, R.integer.BORDER_4, R.integer.BORDER_4);
			public const Border BORDER_EMPTY_8 = BorderFactory.createEmptyBorder(
				R.integer.BORDER_8, R.integer.BORDER_8, R.integer.BORDER_8, R.integer.BORDER_8);
			public const Border BORDER_FORECAST = BorderFactory.createEmptyBorder(
				R.integer.BORDER_32, 0, R.integer.BORDER_32, 0);
			public const Border BORDER_EMPTY_8_LEFT = BorderFactory.createEmptyBorder(
				0, R.integer.BORDER_8, 0, 0);
			public const Border BORDER_BEVEL_LOWER_EMPTY_8 = BorderFactory.createCompoundBorder(
				BorderFactory.createEmptyBorder(
					R.integer.BORDER_8, R.integer.BORDER_8, R.integer.BORDER_8, R.integer.BORDER_8),
				BorderFactory.createBevelBorder(BevelBorder.LOWERED));
			*/
		}
		
		public static class dimen
		{
			public const int FRAME_WIDTH = 480;
			public const int FRAME_HEIGHT = 640;
			public const int DIALOG_WIDTH = 360;
			public const int DIALOG_HEIGHT = 512;
			public const int TABLE_ROW_HEIGHT = 20;
		}
		
		public static class color
		{
			public static readonly Color TABLE_GRID_COLOR = Color.FromArgb(0xF0, 0xF0, 0xF0);
			public static readonly Color CORRECTION_REQUIRED = Color.FromArgb(210, 202, 0);
			public static readonly Color CORRECTION_NON_REQUIRED = Color.FromArgb(0, 211, 40);
			public static readonly Color CORRECTION_NO_METADATA = Color.FromArgb(210, 0, 0);
		}
	}
}
