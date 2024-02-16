
using System;
using Java.Lang;

namespace Javax.Swing
{
	/// <summary>
	/// Description of Document.
	/// </summary>
	public interface Document
	{
		/// <summary>
		/// Returns number of characters of content currently
		/// in the document.
		/// </summary>
		/// <returns>number of characters &gt;= 0</returns>
		int GetLength();
		
		/// <summary>
		/// Registers the given observer to begin receiving notifications
		/// when changes are made to the document.
		/// </summary>
		/// <param name="listener">the observer to register</param>
		/// <see cref="Document#removeDocumentListener"/>
//		void AddDocumentListener(DocumentListener listener);
		
		/// <summary>
		/// Unregisters the given observer from the notification list
		/// so it will no longer receive change updates.
		/// </summary>
		/// <param name="listener">the observer to register</param>
		/// <see cref="Document#AddDocumentListener"/>
//		void RemoveDocumentListener(DocumentListener listener);
		
		/// <summary>
		/// Registers the given observer to begin receiving notifications
		/// when undoable edits are made to the document.
		/// </summary>
		/// <param name="listener">the observer to register</param>
		/// <see cref="javax.swing.event.UndoableEditEvent"/>
//		void AddUndoableEditListener(UndoableEditListener listener);
		
		/// <summary>
		/// Unregisters the given observer from the notification list
		/// so it will no longer receive updates.
		/// </summary>
		/// <param name="listener">the observer to register</param>
		/// <see cref="javax.swing.event.UndoableEditEvent" />
//		void RemoveUndoableEditListener(UndoableEditListener listener);
		
		/// <summary>
		/// Gets the properties associated with the document.
		/// </summary>
		/// <param name="key">a non-<c>null</c> property key</param>
		/// <returns>the properties</returns>
		object GetProperty(object key);
		
		/// <summary>
		/// Associates a property with the document.  Two standard
		/// property keys provided are: <see cref="#StreamDescriptionProperty">
     	/// <c>StreamDescriptionProperty</c></see> and
     	/// <see cref="#TitleProperty"><c>TitleProperty</c></see>.
     	/// Other properties, such as author, may also be defined.
		/// </summary>
		/// <param name="key">the non-<c>null</c> property key</param>
		/// <param name="value">the property value</param>
		/// <see cref="#getProperty(Object)" />
		void PutProperty(object key, object value);
		
		/// <summary>
		/// Removes a portion of the content of the document.
     	/// This will cause a DocumentEvent of type
     	/// DocumentEvent.EventType.REMOVE to be sent to the
     	/// registered DocumentListeners, unless an exception
     	/// is thrown.  The notification will be sent to the
     	/// listeners by calling the removeUpdate method on the
     	/// DocumentListeners.
     	/// 
     	/// <para>
     	/// To ensure reasonable behavior in the face
     	/// of concurrency, the event is dispatched after the
     	/// mutation has occurred. This means that by the time a
     	/// notification of removal is dispatched, the document
     	/// has already been updated and any marks created by
     	/// <c>createPosition</c> have already changed.
     	/// For a removal, the end of the removal range is collapsed
     	/// down to the start of the range, and any marks in the removal
     	/// range are collapsed down to the start of the range.
     	/// </para>
     	/// 
     	/// <para>
     	/// If the Document structure changed as result of the removal,
     	/// the details of what Elements were inserted and removed in
     	/// response to the change will also be contained in the generated
     	/// DocumentEvent. It is up to the implementation of a Document
     	/// to decide how the structure should change in response to a
     	/// remove.
     	/// </para>
     	/// 
     	/// <para>
     	/// If the Document supports undo/redo, an UndoableEditEvent will
     	/// also be generated.
     	/// </para>
     	/// 
		/// </summary>
		/// <param name="offs">the offset from the beginning &gt;= 0</param>
		/// <param name="len">the number of characters to remove &gt;= 0</param>
		/// <see cref="javax.swing.event.DocumentEvent" />
		/// <see cref="javax.swing.event.DocumentListener" />
		/// <see cref="javax.swing.event.UndoableEditEvent" />
		/// <see cref="javax.swing.event.UndoableEditListener" />
		void Remove(int offs, int len);
		
		/// <summary>
		/// Inserts a string of content.  This will cause a DocumentEvent
     	/// of type DocumentEvent.EventType.INSERT to be sent to the
     	/// registered DocumentListers, unless an exception is thrown.
     	/// The DocumentEvent will be delivered by calling the
     	/// insertUpdate method on the DocumentListener.
     	/// The offset and length of the generated DocumentEvent
     	/// will indicate what change was actually made to the Document.
     	/// 
     	/// <para>
     	/// If the Document structure changed as result of the insertion,
     	/// the details of what Elements were inserted and removed in
     	/// response to the change will also be contained in the generated
     	/// DocumentEvent.  It is up to the implementation of a Document
     	/// to decide how the structure should change in response to an
     	/// insertion.
     	/// </para>
     	/// 
     	/// <para>
     	/// If the Document supports undo/redo, an UndoableEditEvent will
     	/// also be generated.
     	/// </para>
		/// </summary>
		/// <param name="offset">the offset into the document to insert the content &gt;= 0.
		/// All positions that track change at or after the given location
		/// will move.
		/// </param>
		/// <param name="str">the string to insert</param>
		/// <param name="a">the attributes to associate with the inserted
		/// content.  This may be null if there are no attributes.
		/// </param>
		/// <see cref="javax.swing.event.DocumentEvent" />
		/// <see cref="javax.swing.event.DocumentListener" />
		/// <see cref="javax.swing.event.UndoableEditEvent" />
		/// <see cref="javax.swing.event.UndoableEditListener" />
//		void InsertString(int offset, String str, AttributeSet a);
		
		/// <summary>
		/// Fetches the text contained within the given portion
     	/// of the document.
		/// </summary>
		/// <param name="offset">the offset into the document representing the desired
		/// start of the text &gt;= 0
		/// </param>
		/// <param name="length">the length of the desired string &gt;= 0</param>
		/// <returns>the text, in a String of length &gt;= 0</returns>
		string GetText(int offset, int length);
		
		/// <summary>
		/// Fetches the text contained within the given portion
     	/// of the document.
     	/// 
     	/// <para>
     	/// If the partialReturn property on the txt parameter is false, the
     	/// data returned in the Segment will be the entire length requested and
     	/// may or may not be a copy depending upon how the data was stored.
     	/// If the partialReturn property is true, only the amount of text that
     	/// can be returned without creating a copy is returned.  Using partial
     	/// returns will give better performance for situations where large
     	/// parts of the document are being scanned.  The following is an example
     	/// of using the partial return to access the entire document:
     	/// </para>
     	/// 
     	/// <code>
     	/// &nbsp; int nleft = doc.getDocumentLength();
     	/// &nbsp; Segment text = new Segment();
     	/// &nbsp; int offs = 0;
     	/// &nbsp; text.setPartialReturn(true);
     	/// &nbsp; while (nleft &gt; 0) {
     	/// &nbsp;     doc.getText(offs, nleft, text);
     	/// &nbsp;     // do someting with text
     	/// &nbsp;     nleft -= text.count;
     	/// &nbsp;     offs += text.count;
     	/// &nbsp; }
     	/// </code>
		/// </summary>
		/// <param name="offset">the offset into the document representing the desired
		/// start of the text &gt;= 0
		/// </param>
		/// <param name="length">the length of the desired string &gt;= 0</param>
		/// <param name="txt">the Segment object to return the text in</param>
//		void GetText(int offset, int length, Segment txt);
		
		/// <summary>
		/// Returns a position that represents the start of the document.  The
     	/// position returned can be counted on to track change and stay
     	/// located at the beginning of the document.
		/// </summary>
		/// <returns>the position</returns>
//		Position GetStartPosition();
		
		/// <summary>
		/// Returns a position that represents the end of the document.  The
     	/// position returned can be counted on to track change and stay
     	/// located at the end of the document.
		/// </summary>
		/// <returns>the position</returns>
//		Position GetEndPosition();
		
		/// <summary>
		/// This method allows an application to mark a place in
     	/// a sequence of character content. This mark can then be
     	/// used to tracks change as insertions and removals are made
     	/// in the content. The policy is that insertions always
     	/// occur prior to the current position (the most common case)
     	/// unless the insertion location is zero, in which case the
     	/// insertion is forced to a position that follows the
     	/// original position.
		/// </summary>
		/// <param name="offs">the offset from the start of the document &gt;= 0</param>
		/// <returns>the position</returns>
//		Position CreatePosition(int offs);
		
		/// <summary>
		/// Returns all of the root elements that are defined.
		/// <para>
		/// Typically there will be only one document structure, but the interface
     	/// supports building an arbitrary number of structural projections over the
     	/// text data. The document can have multiple root elements to support
     	/// multiple document structures.  Some examples might be:
		/// </para>
		/// <list type="bulleted">
		/// 	<item><description>Text direction.</description></item>
		/// 	<item><description>Lexical token streams.</description></item>
		/// 	<item><description>Parse trees.</description></item>
		/// 	<item><description>Conversions to formats other than the native format.</description></item>
		/// 	<item><description>Modification specifications.</description></item>
		/// 	<item><description>Annotations.</description></item>
		/// </list>
		/// 
		/// </summary>
		/// <returns></returns>
//		Element[] GetRootElements();
		
		/// <summary>
		/// Returns the root element that views should be based upon,
     	/// unless some other mechanism for assigning views to element
     	/// structures is provided.
		/// </summary>
		/// <returns>the root element</returns>
//		Element GetDefaultRootElement();
		
		/// <summary>
		/// Allows the model to be safely rendered in the presence
     	/// of concurrency, if the model supports being updated asynchronously.
     	/// The given runnable will be executed in a way that allows it
     	/// to safely read the model with no changes while the runnable
     	/// is being executed.  The runnable itself may NOT
     	/// make any mutations.
		/// </summary>
		/// <param name="r">a <c>Runnable</c> used to render the model</param>
		void Render(Runnable r);
		
		/**
     	 * The property name for the description of the stream
    	 * used to initialize the document.  This should be used
    	 * if the document was initialized from a stream and
    	 * anything is known about the stream.
    	 */
//		public const string StreamDescriptionProperty = "stream";
		
		/**
    	 * The property name for the title of the document, if
    	 * there is one.
    	 */
//    	public const string TitleProperty = "title";
	}
}
