using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
#if !SERVER
namespace System.ComponentModel.DataAnnotations
{

    public class Key : Attribute
    {
    }


   

}
 namespace System.ComponentModel.DataAnnotations.Schema
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ForeignKeyAttribute : Attribute
    {

        public string Name
        {
            get
            {
                throw null;
            }
        }

        public ForeignKeyAttribute(string name)
        {
        }
    }
}
#endif
