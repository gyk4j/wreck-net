﻿
using System;

namespace JShim.NIO.File
{
	/// <summary>
	/// Description of Paths.
	/// </summary>
	public class Paths
	{
		public static Path Get(params string[] subpaths)
		{
			return FileSystems.GetDefault().GetPath(subpaths);
		}
	}
}
