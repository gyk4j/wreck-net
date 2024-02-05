
using System;

namespace Wreck.Entity
{
	/// <summary>
	/// Description of FileStatisticsBean.
	/// </summary>
	public class FileStatisticsBean
	{
		private readonly string statistic;
		private readonly int count;
		
		public FileStatisticsBean(string statistic, int count) : base()
		{
			this.statistic = statistic;
			this.count = count;
		}
		
		public string Statistic
		{
			get { return statistic; }
		}

		public int Count
		{
			get { return count; }
		}
	}
}
