using System;
using System.Collections.Generic;

namespace SGSStandalone.Core.ClientMessage
{    
    public class StartMsg : BaseMsg
    {
        public string userId { get; set; }
        public string loginToken { get; set; }
        public string jwt { get; set; }
        public string sessionId { get; set; }
        public ulong token { get; set; }


    }
}