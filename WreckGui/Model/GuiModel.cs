
using System;
using System.Collections.Generic;
using Java.Beans;
using Javax.Swing;
using Wreck.Entity;
using Wreck.Resources;
using WreckGui.Model;

namespace Wreck.Model
{
	/// <summary>
	/// Description of GuiModel.
	/// </summary>
	public class GuiModel : IModel
	{
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
			this.customDateTimeModel = new DateTime();
			this.correctionModel = new Dictionary<CorrectionEnum, Boolean>();
			
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
	}
}
