using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
namespace ClientMsgs
{
    
    public class BooleanResponse
    {
        public bool done { get; set; }
        public string text { get; set; }
    }
    
    public class GetIds
    {
        public List<int> ids { get; set; }
    }
    public class GetIdsResponse<T> : BooleanResponse where T : Entity
    {
        public List<T> data { get; set; }
    }

}

