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
using Tools;

namespace AdminBaseComponenets.BaseComs
{
    
    public partial class SubTable<TItem, TKEY, TMKEY,TMASTER> : NullableInput2<IReadOnlyCollection<TItem>>
         where TItem : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable

         where TMASTER : class, Models.IIdMapper<TMKEY>
            where TMKEY : IEquatable<TMKEY>, IComparable<TMKEY>, IComparable
    {



        public IEntityService<TItem, TKEY> Data { get; set; } = null;
        public string ButtonState = "--";

        //[Parameter]
        //public string masterEntityName { get; set; }
        [Parameter]
        public string collectionName { get; set; }
        [Parameter]
        public TMKEY masterEnityId { get; set; }

        public TItem addingItem { get; set; }

        async Task Click()
        {
            
            await load();
        }

        
        public async Task load()
        {
            ButtonState = "loading";
            Console.WriteLine("load start");

            if (Data is null)
                Data = Program0.getEntityManager<TItem, TKEY>();
            Console.WriteLine("load getAll2");
            var masterEntityName= typeof(TMASTER).GetUrlEncodeName();
            value = await Data.getAllSubTable<TMASTER,TMKEY>(collectionName, masterEnityId) as IReadOnlyCollection<TItem>;



            Console.WriteLine("load end");

            ButtonState = "reload ";
            StateHasChanged();
            
        }

        async Task newItem()
        {
            
        }



    }


}