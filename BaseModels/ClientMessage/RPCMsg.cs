using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SGSStandalone.Core.ClientMessage
{
	public class RPCMsg : BaseMsg
	{
		public string reqId { get; set; }


	}
	public class RPCError : RPCMsg
	{
		public int code { get; set; }
		public string detail { get; set; }
		public string message { get; set; }
		public string meta { get; set; }


	}
}