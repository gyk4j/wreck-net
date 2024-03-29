﻿
using System;
using Wreck;

namespace WreckGui.View
{
	/// <summary>
	/// Description of GuiView.
	/// </summary>
	public class GuiView : IView
	{
		private readonly MainForm main;
		
		private readonly ScanningDialogForm scanningDialog;
		
		public GuiView(MainForm main, ScanningDialogForm scanningDialog)
		{
			this.main = main;
			this.scanningDialog = scanningDialog;
		}
		
		public MainForm GetMain()
		{
			return main;
		}
		
		public ScanningDialogForm GetScanningDialog()
		{
			return scanningDialog;
		}
	}
}
