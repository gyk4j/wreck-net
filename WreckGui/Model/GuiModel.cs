
using System;
using Wreck.Entity;

namespace Wreck.Model
{
	/// <summary>
	/// Description of GuiModel.
	/// </summary>
	public class GuiModel
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
		
//		private readonly BoundedRangeModel scanningProgressModel;
		
		public GuiModel()
		{
			this.tableModel = new SampleTableModel<FileBean>(typeof(FileBean));
			
			this.fileStatisticsTableModel = new SampleTableModel<FileStatisticsBean>(typeof(FileStatisticsBean));
			this.metadataStatisticsTableModel = new SampleTableModel<MetadataStatisticsBean>(typeof(MetadataStatisticsBean));
			this.extensionStatisticsTableModel = new SampleTableModel<ExtensionStatisticsBean>(typeof(ExtensionStatisticsBean));
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
	}
}
