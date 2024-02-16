
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Java.IO
{
	/// <summary>
	/// Description of ObjectInputStream.
	/// </summary>
	public class ObjectInputStream
	{
		public void DefaultReadObject()
		{
			
		}
		
		public object ReadObject()
		{
			Stream s = new MemoryStream();
			BinaryFormatter formatter = new BinaryFormatter();
			object o = formatter.Deserialize(s);
			s.Close();
			return o;
		}
	}
}
