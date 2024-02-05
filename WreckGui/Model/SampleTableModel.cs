
using System;
using System.Collections.Generic;
using System.Security;

using log4net;

namespace Wreck.Model
{
	/// <summary>
	/// Description of SampleTableModel.
	/// </summary>
	public class SampleTableModel<T>
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(SampleTableModel<T>));
		
		private IList<string> columns = new List<string>();
		private IList<T> data = new List<T>();
		
		private readonly Type type;
		
		public SampleTableModel(Type type) : base()
		{
			this.type = type;
		}
		
		public Type Type
		{
			get { return this.type; }
		}

		public string[] Header
		{
			get
			{
				string[] header = new string[columns.Count];
				for(int i = 0; i < columns.Count; i++)
				{
					header[i] = columns[i].ToUpper();
				}
				
				return header;
			}
		}
		
		public IList<T> Data
		{
			get { return data; }
		}
		
		public int RowCount
		{
			get { return Data.Count; }
		}
		
		public int ColumnCount
		{
			get { return columns.Count; }
		}
		
		public object GetValueAt(int rowIndex, int columnIndex)
		{
			T bean = Data[rowIndex];
			string column = columns[columnIndex];
			object val = null;
			try
			{
//				column.SetAccessible(true);
//				val = column[bean];
			}
			catch (ArgumentException e)
			{
				LOG.Error(e.ToString());
			}
			catch (AccessViolationException e)
			{
				LOG.Error(e.ToString());
			}
			catch (SecurityException e)
			{
				LOG.Error(e.ToString());
			}
			return val;
		}
		
		public string ColumnName(int column)
		{
			string n = columns[column];
			return n.Substring(0, 1).ToUpper() + n.Substring(1);
		}
		
		public Type ColumnClass(int columnIndex)
		{

			Type clazz = null;
			
			string field;
			try
			{
				field = columns[columnIndex];
				
				if((field != null))
					clazz = field.GetType();
				
				if(clazz.IsPrimitive)
				{
					clazz = clazz.ReflectedType;
				}
//				LOG.DebugFormat("Column class: {0} -> {1}", columnIndex, clazz);
			}
			catch (SecurityException e)
			{
				LOG.Error(e.ToString());
			}
			
			return clazz;
		}
		
		public void AddRow(T row)
		{
			data.Add(row);
//			this.FireTableRowsInserted(data.Count-1, data.Count-1);
		}
		
		public void RemoveRow(int row)
		{
			data.RemoveAt(row);
//			this.FireTableRowsDeleted(row, row);
		}
		
		public void Clear() {
			if(data.Count > 0)
			{
				data.Clear();
//				this.FireTableRowsDeleted(0, this.RowCount-1);
			}
		}
	}
}
