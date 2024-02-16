
using System;
using Java.Util;

namespace Javax.Swing.Event
{
	/// <summary>
	/// Description of DocumentListener.
	/// </summary>
	public interface DocumentListener : EventListener
	{
		/// <summary>
		/// Gives notification that there was an insert into the document.  The
     	/// range given by the DocumentEvent bounds the freshly inserted region.
		/// </summary>
		/// <param name="e">the document event</param>
//		void InsertUpdate(DocumentEvent e);
		
		/// <summary>
		/// Gives notification that a portion of the document has been
     	/// removed.  The range is given in terms of what the view last
     	/// saw (that is, before updating sticky positions).
		/// </summary>
		/// <param name="e">the document event</param>
//		void RemoveUpdate(DocumentEvent e);
		
		/// <summary>
		/// Gives notification that an attribute or set of attributes changed.
		/// </summary>
		/// <param name="e">the document event</param>
//		void ChangedUpdate(DocumentEvent e);
	}
}
