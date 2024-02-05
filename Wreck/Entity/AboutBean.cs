
using System;

namespace Wreck.Entity
{
	/// <summary>
	/// Description of AboutBean.
	/// </summary>
	public class AboutBean
	{
		private readonly string icon;
		private readonly string attribution;
		private readonly string tooltip;
		private readonly Uri href;
		
		public AboutBean(string icon, string attribution, string tooltip, Uri href) : base()
		{
			this.icon = icon;
			this.attribution = attribution;
			this.tooltip = tooltip;
			this.href = href;
		}

		public string Icon
		{
			get { return icon; }
		}

		public string Attribution
		{
			get { return attribution; }
		}
		
		public string Tooltip
		{
			get { return tooltip; }
		}
		
		public Uri Href
		{
			get { return href; }
		}
	}
}
