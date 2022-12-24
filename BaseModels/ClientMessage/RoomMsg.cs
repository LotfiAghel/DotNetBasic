using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SGSStandalone.Core.ClientMessage
{
    public class RoomMsg : BaseMsg
    {
        public string roomId { get; set; }
        public string senderId { get; set; }
        public object msg { get; set; }
    }
}
