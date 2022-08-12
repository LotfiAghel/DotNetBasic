using System;
using System.Collections.Generic;

namespace SGSStandalone.Core.ClientMessage
{

    public class ConnectionSettingsMsg : BaseMsg
    {

        public string sessionId { get; set; }
        public int pingInterval { get; set; }
        public int pingTimeout { get; set; }
        public int sessionTimeout { get; set; }
        public ulong token { get; set; }
        public string jwt { get; set; }
        public string server { get; set; }
    }

   
}
