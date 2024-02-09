
using System;

namespace Javax.Swing
{
	/// <summary>
	/// Description of WindowConstants.
	/// </summary>
	public static class WindowConstants
	{
		/**
		 * The do-nothing default window close operation.
		 */
		public const int DoNothingOnClose = 0;

		/**
		 * The hide-window default window close operation
		 */
		public const int HideOnClose = 1;

		/**
		 * The dispose-window default window close operation.
		 * <p>
		 * <b>Note</b>: When the last displayable window
		 * within the Java virtual machine (VM) is disposed of, the VM may
		 * terminate.  See <a href="../../java/awt/doc-files/AWTThreadIssues.html">
		 * AWT Threading Issues</a> for more information.
		 * @see java.awt.Window#dispose()
		 * @see JInternalFrame#dispose()
		 */
		public const int DisposeOnClose = 2;

		/**
		 * The exit application default window close operation. Attempting
		 * to set this on Windows that support this, such as
		 * <code>JFrame</code>, may throw a <code>SecurityException</code> based
		 * on the <code>SecurityManager</code>.
		 * It is recommended you only use this in an application.
		 *
		 * @since 1.4
		 * @see JFrame#setDefaultCloseOperation
		 */
		public const int ExitOnClose = 3;
	}
}
