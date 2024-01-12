
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;

using log4net;

namespace Wreck.Security
{
	/// <summary>
	/// Description of PasswordProvider.
	/// </summary>
	public class PasswordProvider
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(PasswordProvider));
		
		protected const string CONFIG_SECTION = "PasswordProvider";
		
		private NameValueCollection passwords;
		
		private static PasswordProvider instance = null;
		
		public static PasswordProvider Instance
		{
			get
			{
				if (instance == null)
					instance = new PasswordProvider();
				return instance;
			}
		}
		
		private PasswordProvider()
		{
			passwords = (NameValueCollection) ConfigurationManager.GetSection(CONFIG_SECTION);
		}
		
		/// <summary>
		/// Get the password to be used by 7-Zip for opening a particular archive file.
		/// </summary>
		/// <param name="file">A file path</param>
		/// <returns>Password for file in App.config if found, else <c>null</c></returns>
		public string GetPassword(FileSystemInfo file)
		{
			string key = file.FullName;
			return passwords.Get(key);
		}
	}
}
