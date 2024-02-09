
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Javax.Swing;

namespace Wreck.View.Scanning
{
	/// <summary>
	/// Description of ScanningDialog.
	/// </summary>
	public class ScanningDialog : JDialog
	{
		private const string Scanning = "Scanning";
		
		private readonly Label icon;
		private readonly Label action;
		private readonly ProgressBar progress;
		private readonly Button cancel;
		
		public ScanningDialog(JFrame owner) : base()
		{
//			ScanningDialogForm form = (ScanningDialogForm) owner;
//			icon = form.ScanIcon;
//			action = form.Action;
//			progress = form.Progress;
//			cancel = form.Cancel;
		}

//		Not virtual cannot be overridden.
//		public void Open()
//		{
//		}
		
//		Not virtual cannot be overridden.
//		public void Close()
//		{
//		}
		
		public ProgressBar Progress
		{
			get { return progress; }
			set
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(Scanning);
				sb.Append(" - ");
				sb.Append(progress);
				sb.Append("%...");
//				this.Text = sb.ToString();
			}
		}
		
		public Label Action
		{
			get { return action; }
		}
		
		public Button Cancel
		{
			get { return cancel; }
		}
	}
}
