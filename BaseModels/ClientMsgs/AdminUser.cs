using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminMsg
{
    public class LoginRequest
    {
        public string userName { get; set; }
        public string pass { get; set; }
    }
    public class LoginResponse<T> : ClientMsgs.BooleanResponse where T: Models.IAdminUser
    {
        public T user { get; set; }
    }



}
