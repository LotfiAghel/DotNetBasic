


using Models;
using System.Threading.Tasks;
using System;
using System.ComponentModel;
using Tools;
using Microsoft.AspNetCore.Components;
using AdminClientViewModels;

namespace AdminBaseComponenets.BaseComs
{

    
    public partial class ActionBar<TItem, TKEY> : ComponentBase
         where TItem : class, Models.IIdMapper<TKEY>
         where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        

        [Parameter]
        public Models.IAction<TItem> value { get; set; }


        TItem Data2;


        [Parameter]
        public int entityId { get; set; }


        public NewEntityService<TItem, TKEY> Data { get; set; } = null;

        public string ButtonState="send Action";

        async Task onChange(object x)
        {
            value=x as Models.IAction<TItem>;
        }


        async Task Click(){
            await load();
        }
    public async Task load()
    {
        ButtonState = "loading";
        Console.WriteLine("load start");
        
        if (Data is null)
            Data = AdminBaseComponenets.Program0.getEntityManager<TItem, TKEY>() as NewEntityService<TItem, TKEY>;
        Console.WriteLine("load getAll2");
        Data2=await Data.sendAction(entityId,value) as TItem;
        

        
        Console.WriteLine("load end");
        
        ButtonState = "reload " ;
    }
        
    }
}