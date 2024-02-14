
using System;
using System.Drawing;
using System.Windows.Forms;

using log4net;

namespace Wreck
{
	/// <summary>
	/// Description of ScanningDialogForm.
	/// </summary>
	public partial class ScanningDialogForm : Form
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ScanningDialogForm));
			
		public ScanningDialogForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			log.Debug("Initializing ScanningDialogForm");
//			scanningDialog = new ScanningDialog(this);
			log.Debug("Initialized ScanningDialogForm");
		}
		
		public Label ScanIcon
		{
			get { return lblIcon; }
		}
		
		public ProgressBar Progress
		{
			get { return pgbProgress; }
		}
		
		public Label Action
		{
			get { return lblAction; }
		}
		
		public Button Cancel
		{
			get { return btnCancel; }
		}
	}
}
