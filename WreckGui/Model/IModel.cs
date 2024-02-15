
using System;
using Javax.Swing;

namespace WreckGui.Model
{
	/// <summary>
	/// Description of IModel.
	/// </summary>
	public interface IModel
	{
		BoundedRangeModel GetScanningProgressModel();
	}
}
