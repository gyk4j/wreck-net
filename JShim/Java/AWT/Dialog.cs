
using System;

namespace Java.AWT
{
	/// <summary>
	/// Description of Dialog.
	/// </summary>
	public static class Dialog
	{
		public enum ModalityType
		{
			/**
			 * <code>MODELESS</code> dialog doesn't block any top-level windows.
			 */
			Modeless,
			/**
			 * A <code>DOCUMENT_MODAL</code> dialog blocks input to all top-level windows
			 * from the same document except those from its own child hierarchy.
			 * A document is a top-level window without an owner. It may contain child
			 * windows that, together with the top-level window are treated as a single
			 * solid document. Since every top-level window must belong to some
			 * document, its root can be found as the top-nearest window without an owner.
			 */
			DocumentModal,
			/**
			 * An <code>APPLICATION_MODAL</code> dialog blocks all top-level windows
			 * from the same Java application except those from its own child hierarchy.
			 * If there are several applets launched in a browser, they can be
			 * treated either as separate applications or a single one. This behavior
			 * is implementation-dependent.
			 */
			ApplicationModal,
			/**
			 * A <code>TOOLKIT_MODAL</code> dialog blocks all top-level windows run
			 * from the same toolkit except those from its own child hierarchy. If there
			 * are several applets launched in a browser, all of them run with the same
			 * toolkit; thus, a toolkit-modal dialog displayed by an applet may affect
			 * other applets and all windows of the browser instance which embeds the
			 * Java runtime environment for this toolkit.
			 * Special <code>AWTPermission</code> "toolkitModality" must be granted to use
			 * toolkit-modal dialogs. If a <code>TOOLKIT_MODAL</code> dialog is being created
			 * and this permission is not granted, a <code>SecurityException</code> will be
			 * thrown, and no dialog will be created. If a modality type is being changed
			 * to <code>TOOLKIT_MODAL</code> and this permission is not granted, a
			 * <code>SecurityException</code> will be thrown, and the modality type will
			 * be left unchanged.
			 */
			ToolkitModal
		}
	}
}
