
using System;
using System.Windows.Forms;
using Java.AWT;

namespace Javax.Swing
{
	/// <summary>
	/// Description of JDialog.
	/// </summary>
	public class JDialog
	{
		protected readonly JFrame owner;
		
		public JDialog() : this(null, false)
		{
		}
		
		public JDialog(JFrame owner) : this(owner, false)
		{
		}
		
		public JDialog(JFrame owner, bool modal) : this(owner, "", modal)
		{
		}
		
		public JDialog(JFrame owner, string title) : this(owner, title, false)
		{
		}
		
		public JDialog(JFrame owner, string title, bool modal) //: base(owner, title, modal)
		{
			if (owner == null)
			{
//				WindowListener ownerShutdownListener =
//					SwingUtilities.GetSharedOwnerFrameShutdownListener();
//				AddWindowListener(ownerShutdownListener);
			}
//			DialogInit();
		}
		
		public JDialog(JFrame owner, Dialog.ModalityType modalityType) : this(owner, "", modalityType)
		{
		}
		
		public JDialog(JFrame owner, string title, Dialog.ModalityType modalityType) //: base(owner, title, modalityType)
		{
//			DialogInit();
		}
	}
}
