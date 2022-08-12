using Data.ServerEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
	public interface IAnalytics
	{
		public static IAnalytics instance;
		public void AddRecordInBackground(BaseEvent evnt);



		public void AddAnalyticsRecordInBackground(AnalyticEvent evnt);
		public void AddMonitoringRecordInBackground(MonitoringEvent evnt);
		public void AddLogRecordInBackground(LogEvent evnt);
	}
}
