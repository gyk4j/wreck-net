
using System;
using System.ComponentModel;
using System.Windows.Forms;

using log4net;
using log4net.Config;
using Wreck.Controller;
using Wreck.Model;
using WreckGui.Model;
using WreckGui.View;

namespace Wreck
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		private const string LOG4NET_XML = "log4net.xml";
		
		private static readonly ILog log = LogManager.GetLogger(typeof(Program));
		
		private static GuiModel model;
		public static IModel Model
		{
			get { return model; }
		}
		
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
			//BasicConfigurator.Configure();
			XmlConfigurator.Configure(new System.IO.FileInfo(LOG4NET_XML));
			
			model = new GuiModel();
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			main = new MainForm();
			scan = new ScanningDialogForm();
			view = new GuiView(main, scan);
			
			controller = new GuiController(model, view);
			
			model.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
			{
				bool? v = new bool?();
				if(e.PropertyName.Equals("MetadataTags"))
					v = model.MetadataTags;
				else if(e.PropertyName.Equals("FileSystemAttributes"))
					v = model.FileSystemAttributes;
				else if(e.PropertyName.Equals("Custom"))
					v = model.Custom;
				else if(e.PropertyName.Equals("Creation"))
					v = model.Creation;
				else if(e.PropertyName.Equals("Modified"))
					v = model.Modified;
				else if(e.PropertyName.Equals("Accessed"))
					v = model.Accessed;
				else
					log.ErrorFormat("Unknown property: {0}", e.PropertyName);
										
				log.DebugFormat("{0} = {1}", 
				                e.PropertyName, 
				                v.HasValue? v.Value.ToString(): "?");
			};
			
			Application.Run(view.GetMain());
		}
		
	}
}
