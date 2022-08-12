using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SGSStandalone.Core.ClientMessage
{
    public class RoomNotFoundMsg : BaseMsg
    {
        public string roomId { get; set; }


    }
}
