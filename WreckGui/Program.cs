
using System;
using System.Windows.Forms;
using Wreck.Controller;
using WreckGui.View;

namespace Wreck
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		private static GuiView view;
		public static IView View
		{
			get { return view; }
		}
		
		private static MainForm main;
		private static ScanningDialogForm scan;
		
		private static IController controller;
		public static IController Controller
		{
			get { return controller; }
		}
		
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			main = new MainForm();
			scan = new ScanningDialogForm();
			
			view = new GuiView(main, scan);
			
			controller = new GuiController(view);
			
			Application.Run(view.MainForm());
		}
		
	}
}
