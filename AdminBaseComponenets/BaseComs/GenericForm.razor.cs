using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;


using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace AdminBaseComponenets.BaseComs
{
    
    public partial class GenericForm<TItem> : EditBase2<TItem>
    {


        public List<System.Reflection.PropertyInfo> propertis = typeof(TItem).GetProperties(
                          BindingFlags.Public |
                          BindingFlags.NonPublic |
                          BindingFlags.Instance ).ToList();

        public override bool inRowField() => false;

    }


}