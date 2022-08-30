using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;


using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using Blazorise.DataGrid;
using Models;
using AdminClientViewModels;

namespace AdminBaseComponenets.BaseComs
{
    
    public partial class SubTable<TItem, TKEY> : NullableInput2<IEnumerable<TItem>>
         where TItem : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {



        public IEntityService01<TItem> Data { get; set; } = null;
        public string ButtonState = "--";

        [Parameter]
        public string masterEntityName { get; set; }
        [Parameter]
        public string collectionName { get; set; }
        [Parameter]
        public int masterEnityId { get; set; }
        async Task Click()
        {
            await load();
        }
        public async Task load()
        {
            ButtonState = "loading";
            Console.WriteLine("load start");

            if (Data is null)
                Data = Program0.getEntityManager01<TItem>();
            Console.WriteLine("load getAll2");
            value = await Data.getAllSubTable(masterEntityName, collectionName, masterEnityId) as IEnumerable<TItem>;



            Console.WriteLine("load end");

            ButtonState = "reload ";
        }



    }


}