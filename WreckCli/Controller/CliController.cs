
using System;
using System.Collections.Generic;
using System.IO;

using Java.Beans;
using Javax.Swing;
using log4net;
using Wreck.Entity;
using Wreck.IO;
using Wreck.IO.Reader;
using Wreck.IO.Task;
using Wreck.IO.Writer;
using Wreck.Logging;
using Wreck.Model;
using Wreck.Resources;
using Wreck.Service;
using Wreck.Util.Logging;
using Wreck.View;

namespace Wreck.Controller
{
	/// <summary>
	/// Description of CliController.
	/// </summary>
	public class CliController : AbstractController
	{
		private CliWorker worker;
		
		private readonly ConsoleModel model;
		private readonly ConsoleView view;
		
		public CliController(ConsoleView view) : base()
		{
			this.model = new ConsoleModel();
			this.view = view;
		}
		
		public ConsoleModel Model
		{
			get { return model; }
		}
		
		public ConsoleView View
		{
			get { return view; }
		}
		
		public CliWorker Worker
		{
			get { return worker; }
			set { worker = value; }
		}
		
		// Event Handlers
		
		public override void Analyze()
		{
			Run(CorrectionMode.Analyze);
		}
		
		public override void Backup()
		{
			Run(CorrectionMode.BackupAttributes);
		}
		
		public override void Repair()
		{
			Run(CorrectionMode.SaveAttributes);
		}
		
		public override void Restore()
		{
			Run(CorrectionMode.RestoreAttributes);
		}
		
		public override void Verify()
		{
			Run(CorrectionMode.VerifyAttributes);
		}
		
		private void Run(CorrectionMode mode)
		{
			string[] args = Environment.GetCommandLineArgs();
			string[] dirs;
			
			if(args.Length > 1)
			{
				dirs = new string[args.Length-1];
				Array.Copy(args, 1, dirs, 0, args.Length-1);
			}
			else
			{
				dirs = new string[]
				{
					Directory.GetCurrentDirectory()
				};
			}
			
			foreach(string path in dirs)
			{
				FileSystemInfo fsi;
				
				if(Directory.Exists(path))
					fsi = new DirectoryInfo(path);
				else if(File.Exists(path))
					fsi = new FileInfo(path);
				else
				{
					view.UnknownPathType(path);
					continue;
				}
				
				Dictionary<SourceEnum, bool> sources = new Dictionary<SourceEnum, bool>();
				foreach(SourceEnum s in SourceEnum.Values)
				{
					sources.Add(s, true);
				}
				
				Dictionary<CorrectionEnum, bool> corrections = new Dictionary<CorrectionEnum, bool>();
				foreach(CorrectionEnum c in CorrectionEnum.Values)
				{
					corrections.Add(c, true);
				}
				
				DateTime customDateTime = DateTime.Now;
				
				ITask task = Service.Run(fsi, mode, sources, corrections, customDateTime);
				
				CliWorker pw = new CliWorker(task, fsi);
				pw.Run();
				Done();
			}
		}
		
		public override void Done()
		{
			base.Done();
			Worker = null;
		}
	}
}
