
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Java.Beans;
using Javax.Swing;
using log4net;
using Wreck.Entity;
using Wreck.Resources;
using WreckGui.Model;

namespace Wreck.Model
{
	/// <summary>
	/// Description of GuiModel.
	/// </summary>
	public class GuiModel : IModel, INotifyPropertyChanged
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(GuiModel));
		
//		private static readonly Pattern DIGITS = Pattern.compile("^[0-9]+$");
		
		private readonly SampleTableModel<FileBean> tableModel;
//		private readonly ButtonModel metadataModel;
		private IDictionary<SourceEnum, Boolean> sourceModel;
		private DateTime customDateTimeModel;
		
		private IDictionary<CorrectionEnum, Boolean> correctionModel;
		
//		private readonly DefaultButtonModel backupModel;
//		private readonly DefaultButtonModel restoreModel;
//		private readonly DefaultButtonModel verifyModel;
		
		private readonly SampleTableModel<FileStatisticsBean> fileStatisticsTableModel;
		private readonly SampleTableModel<MetadataStatisticsBean> metadataStatisticsTableModel;
		private readonly SampleTableModel<ExtensionStatisticsBean> extensionStatisticsTableModel;
		
//		private readonly DefaultListModel<AboutBean> aboutModel;
		
		private readonly BoundedRangeModel scanningProgressModel;
		
		private PropertyChangeSupport propertyChangeSupport;
		
		private int progress;
		private FileVisit visit;
		
		public GuiModel()
		{
			this.propertyChangeSupport = new PropertyChangeSupport(this);
			this.tableModel = new SampleTableModel<FileBean>(typeof(FileBean));
			
			this.sourceModel = new Dictionary<SourceEnum, Boolean>();
			foreach(SourceEnum s in SourceEnum.Values)
			{
				this.SourceModel.Add(s, true);
			}
			
			this.customDateTimeModel = new DateTime();
			
			this.correctionModel = new Dictionary<CorrectionEnum, Boolean>();
			foreach(CorrectionEnum c in CorrectionEnum.Values)
			{
				this.CorrectionModel.Add(c, true);
			}
			
			this.fileStatisticsTableModel = new SampleTableModel<FileStatisticsBean>(typeof(FileStatisticsBean));
			this.metadataStatisticsTableModel = new SampleTableModel<MetadataStatisticsBean>(typeof(MetadataStatisticsBean));
			this.extensionStatisticsTableModel = new SampleTableModel<ExtensionStatisticsBean>(typeof(ExtensionStatisticsBean));
			
			this.scanningProgressModel = new DefaultBoundedRangeModel();
		}
		
		public IDictionary<SourceEnum, bool> SourceModel
		{
			get { return sourceModel; }
			set { sourceModel = value; }
		}
		
		public DateTime CustomDateTimeModel
		{
			get { return customDateTimeModel; }
			set { customDateTimeModel = value; }
		}
		
		public IDictionary<CorrectionEnum, bool> CorrectionModel
		{
			get { return correctionModel; }
			set { correctionModel = value; }
		}
		
		public SampleTableModel<FileBean> TableModel
		{
			get { return tableModel; }
		}
		
		public SampleTableModel<FileStatisticsBean> FileStatisticsTableModel
		{
			get { return fileStatisticsTableModel; }
		}

		public SampleTableModel<MetadataStatisticsBean> MetadataStatisticsTableModel
		{
			get { return metadataStatisticsTableModel; }
		}
		
		public SampleTableModel<ExtensionStatisticsBean> ExtensionStatisticsTableModel
		{
			get { return extensionStatisticsTableModel; }
		}
		
		public BoundedRangeModel GetScanningProgressModel()
		{
			return scanningProgressModel;
		}
		
		protected PropertyChangeSupport GetPropertyChangeSupport()
		{
			return propertyChangeSupport;
		}
		
		public void AddPropertyChangeListener(PropertyChangeListener listener)
		{
			GetPropertyChangeSupport().AddPropertyChangeListener(listener);
		}
		
		public void RemovePropertyChangeListener(PropertyChangeListener listener)
		{
			GetPropertyChangeSupport().RemovePropertyChangeListener(listener);
		}
		
		protected void FirePropertyChange(string propertyName, object oldValue,
		                                  object newValue)
		{
			GetPropertyChangeSupport().FirePropertyChange(
				propertyName,
				oldValue, newValue);
		}
		
		public void SetProgress(int progress)
		{
			int old = this.progress;
			this.progress = progress;
			FirePropertyChange(R.Strings.PropertyProgress, old, this.progress);
		}
		
		public void SetFileVisit(FileVisit visit)
		{
			FileVisit old = this.visit;
			this.visit = visit;
			FirePropertyChange(R.Strings.PropertyVisits, old, this.visit);
		}
		
		// These fields hold the values for the public properties.
		private Guid idValue = Guid.NewGuid();
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		// This method is called by the Set accessor of each property.
		// The CallerMemberName attribute that is applied to the optional propertyName
		// parameter causes the property name of the caller to be substituted as an argument.
		private void NotifyPropertyChanged(String propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		public bool MetadataTags
		{
			get { return SourceModel[SourceEnum.METADATA]; }
			set
			{
				if (value != SourceModel[SourceEnum.METADATA])
				{
					SourceModel[SourceEnum.METADATA] = value;
					NotifyPropertyChanged("MetadataTags");
				}
			}
		}
		
		public bool FileSystemAttributes
		{
			get { return SourceModel[SourceEnum.FILE_SYSTEM]; }
			set
			{
				if (value != SourceModel[SourceEnum.FILE_SYSTEM])
				{
					SourceModel[SourceEnum.FILE_SYSTEM] = value;
					NotifyPropertyChanged("FileSystemAttributes");
				}
			}
		}
		
		public bool Custom
		{
			get { return SourceModel[SourceEnum.CUSTOM]; }
			set
			{
				if (value != SourceModel[SourceEnum.CUSTOM])
				{
					SourceModel[SourceEnum.CUSTOM] = value;
					NotifyPropertyChanged("Custom");
				}
			}
		}
		
		public bool Creation
		{
			get { return CorrectionModel[CorrectionEnum.CREATION]; }
			set
			{
				if (value != CorrectionModel[CorrectionEnum.CREATION])
				{
					CorrectionModel[CorrectionEnum.CREATION] = value;
					NotifyPropertyChanged("Creation");
				}
			}
		}
		
		public bool Modified
		{
			get { return CorrectionModel[CorrectionEnum.MODIFIED]; }
			set
			{
				if (value != CorrectionModel[CorrectionEnum.MODIFIED])
				{
					CorrectionModel[CorrectionEnum.MODIFIED] = value;
					NotifyPropertyChanged("Modified");
				}
			}
		}
		
		public bool Accessed
		{
			get { return CorrectionModel[CorrectionEnum.ACCESSED]; }
			set
			{
				if (value != CorrectionModel[CorrectionEnum.ACCESSED])
				{
					CorrectionModel[CorrectionEnum.ACCESSED] = value;
					NotifyPropertyChanged("Accessed");
				}
			}
		}
		
		public string GetBindingDataMember(SourceEnum s)
		{
			string member;
			
			if(s == SourceEnum.METADATA)
				member = "MetadataTags";
			else if(s == SourceEnum.FILE_SYSTEM)
				member = "FileSystemAttributes";
			else if(s == SourceEnum.CUSTOM)
				member = "Custom";
			else
				member = string.Empty;
			
			return member;
		}
		
		public string GetBindingDataMember(CorrectionEnum c)
		{
			string member;
			
			if(c == CorrectionEnum.CREATION)
				member = "Creation";
			else if(c == CorrectionEnum.MODIFIED)
				member = "Modified";
			else if(c == CorrectionEnum.ACCESSED)
				member = "Accessed";
			else
				member = string.Empty;
			
			return member;
		}
	}
}
