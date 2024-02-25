using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{

    public class BaseUser : IdMapper<Guid>
    {

    }
    public interface IUserServiceBase
    {
        Guid getUserId();
    }
    public class UserServiceBase : IUserServiceBase
    {
        public Guid v;
        public Guid getUserId()
        {
            return v;
        }
    }

    [Models.GeneratedControllerAttribute]
    [UpdateAccess(AdminUserRole.SUPER_USER)]
    [InsertAccess(AdminUserRole.SUPER_USER)]
    public class AdminUser22 : Id4Entity
    {

        public string firstName { get; set; }
        public string LastName { get; set; }
        public string username { get; set; }

        //[CustomIgnoreTag(CustomIgnoreTag.Kind.CLIENT)]
        [JsonIgnore]
        public string password { get; set; }

        [MultiSelect]
        public List<AdminUserRole> roles { get; set; }

    }


    public class AuthenticateModel
    {
        [Required]
        public int Username { get; set; }

        [Required]
        public string Password { get; set; }
    }




}
