
using System;
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
//		private readonly IDictionary<SourceEnum, ButtonModel> sourceModel;
//		private readonly Document dateTimeDocument;
//		private readonly SpinnerNumberModel customDateTimeYearModel;
//		private readonly SpinnerNumberModel customDateTimeMonthModel;
//		private readonly SpinnerNumberModel customDateTimeDayModel;
//		private readonly SpinnerDateModel customDateTimeModel;
		
//		private readonly IDictionary<CorrectionEnum, ButtonModel> correctionModel;
		
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
			
			this.fileStatisticsTableModel = new SampleTableModel<FileStatisticsBean>(typeof(FileStatisticsBean));
			this.metadataStatisticsTableModel = new SampleTableModel<MetadataStatisticsBean>(typeof(MetadataStatisticsBean));
			this.extensionStatisticsTableModel = new SampleTableModel<ExtensionStatisticsBean>(typeof(ExtensionStatisticsBean));
		
			this.scanningProgressModel = new DefaultBoundedRangeModel();
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
