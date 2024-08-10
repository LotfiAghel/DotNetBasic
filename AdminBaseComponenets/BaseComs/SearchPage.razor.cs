using AdminBaseComponenets;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace AdminBaseComponenets.BaseComs
{
    public partial class SearchPage<TMODEL, TKEY> where TMODEL : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {

       

    }
}