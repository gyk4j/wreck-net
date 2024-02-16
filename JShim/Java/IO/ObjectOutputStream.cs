
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Java.IO
{
	/// <summary>
	/// Description of ObjectOutputStream.
	/// </summary>
	public class ObjectOutputStream
	{
		public void DefaultWriteObject()
		{
			
		}
		
		public void WriteObject(object o)
		{
			Stream s = new MemoryStream();
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(s, o);
			s.Close();
		}
	}
}
