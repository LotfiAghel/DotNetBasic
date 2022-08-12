using Newtonsoft.Json.Linq;
using System;

namespace Data.ServerEntity
{
	public class BaseEvent
	{
		public DateTime time;
		public virtual string GetTable() => "n/a";
	}

	public class AnalyticEvent : BaseEvent
	{
		public string name;
		public string instanceId;
		public string userId;
		public JObject parameters;

		public AnalyticEvent(string name, string userId, JObject parameters)
		{
			this.time = DateTime.UtcNow;
			
			this.name = name;
			this.userId = userId;
			this.parameters = parameters;
		}
		public AnalyticEvent(string name, string userId, object parameters)
		{
			this.time = DateTime.UtcNow;
			
			this.name = name;
			this.userId = userId;
			this.parameters = JObject.FromObject(parameters);
		}
		public override string GetTable() => "table_analytics";
	}

	public class MonitoringEvent : BaseEvent
	{
		public string instanceId;
		public string tag;
		public JObject data;

		public MonitoringEvent(string tag, JObject data)
		{
			this.time = DateTime.UtcNow;

			this.tag = tag;
			this.data = data;
		}

		public override string GetTable() => "table_monitorings";
	}

	public class LogEvent : BaseEvent
	{
		public string instanceId;
		public string tag;
		public JToken data;

		public LogEvent(string tag, JObject data)
		{
			this.time = DateTime.UtcNow;
			this.tag = tag;
			this.data = data;
		}

		
	}
	public class TransactionEvent : BaseEvent
	{
		public string userId;
		public string tag;

		public JToken metaData;
		public JObject before;
		public JObject after;

		public TransactionEvent() { }

		public TransactionEvent(string userId, string tag, JObject before, JObject after)
		{
			time = DateTime.UtcNow;
			this.userId = userId;
			this.tag = tag;
			this.before = before;
			this.after = after;
		}

		
	}

}
