
using System;
using System.Text;

namespace Javax.Swing
{
	/// <summary>
	/// Description of JFrame.
	/// </summary>
	public class JFrame
	{		
		public const int ExitOnClose = 3;
		
		private static readonly object defaultLookAndFeelDecoratedKey =
			new StringBuilder("JFrame.defaultLookAndFeelDecorated");
		
		private int defaultCloseOperation = WindowConstants.HideOnClose;
		
		public JFrame() : base()
		{
			FrameInit();
		}
		
		public JFrame(string title) //: base(title)
		{
			FrameInit();
		}
		
		protected void FrameInit()
		{
			
		}
		
		public void SetDefaultCloseOperation(int operation)
		{
			if (operation != WindowConstants.DoNothingOnClose &&
			    operation != WindowConstants.HideOnClose &&
			    operation != WindowConstants.DisposeOnClose &&
			    operation != WindowConstants.ExitOnClose) {
				throw new ArgumentException("defaultCloseOperation must be one of: DO_NOTHING_ON_CLOSE, HIDE_ON_CLOSE, DISPOSE_ON_CLOSE, or EXIT_ON_CLOSE");
			}

			if (operation == WindowConstants.ExitOnClose) {
//				SecurityManager security = System.getSecurityManager();
//				if (security != null) {
//					security.checkExit(0);
//				}
			}
			if (this.defaultCloseOperation != operation) {
				int oldValue = this.defaultCloseOperation;
				this.defaultCloseOperation = operation;
//				FirePropertyChange("defaultCloseOperation", oldValue, operation);
			}
		}
		
		public int DefaultCloseOperation
		{
			get { return defaultCloseOperation; }
		}
		
		public static void SetDefaultLookAndFeelDecorated(bool defaultLookAndFeelDecorated)
		{
			if (defaultLookAndFeelDecorated)
			{
//				SwingUtilities.AppContextPut(defaultLookAndFeelDecoratedKey, bool.TrueString);
			}
			else
			{
//				SwingUtilities.AppContextPut(defaultLookAndFeelDecoratedKey, bool.FalseString);
			}
		}
		
		public static bool IsDefaultLookAndFeelDecorated()
		{
			bool? defaultLookAndFeelDecorated = true;
//				(Boolean) SwingUtilities.AppContextGet(defaultLookAndFeelDecoratedKey);
			if (!defaultLookAndFeelDecorated.HasValue)
			{
				defaultLookAndFeelDecorated = false;
			}
			return defaultLookAndFeelDecorated.Value;
		}
	}
}
