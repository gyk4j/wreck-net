
using System;
using log4net;
using Wreck.View.Preview.Body.Settings;
using Wreck.View.Scanning;

namespace Wreck.View.Preview
{
	/// <summary>
	/// Description of PreviewView.
	/// </summary>
	public class PreviewView
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(PreviewView));
		
		private readonly PreviewDialog previewDialog;
		private readonly SettingsDialog repairActionDialog;
		private readonly ScanningDialog scanningDialog;
		
		public PreviewView()
		{
			previewDialog = new PreviewDialog();
			repairActionDialog = new SettingsDialog(previewDialog);
			scanningDialog = new ScanningDialog(previewDialog);
		}
		
		public PreviewDialog PreviewDialog
		{
			get { return previewDialog; }
		}
		
		public SettingsDialog RepairActionDialog
		{
			get { return repairActionDialog; }
		}
		
		public ScanningDialog ScanningDialog
		{
			get { return scanningDialog; }
		}
	}
}
