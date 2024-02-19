
using System;
using Wreck;

namespace WreckGui.View
{
	/// <summary>
	/// Description of IView.
	/// </summary>
	public interface IView
	{
		MainForm GetMain();
		ScanningDialogForm GetScanningDialog();
	}
}
