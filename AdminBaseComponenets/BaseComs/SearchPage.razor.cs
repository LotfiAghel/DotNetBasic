using AdminBaseComponenets;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq;
using AdminClientViewModels;
using Models;

namespace AdminBaseComponenets.BaseComs
{
    public partial class SearchPage<TMODEL, TKEY> where TMODEL : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {

        [Parameter]
        public Action<TKEY> onClickCustom { get; set; } = null;

        
        [Parameter]
        public Func<IQuery<TMODEL>, Task> onchangeSerachDataRefrense { get; set; } = null;

        
        public IReadOnlyCollection<TMODEL> value { get; set; }
    
        [Parameter]
        public Models.IQuery<TMODEL> serachData { get; set; }
    
        public NewEntityService<TMODEL, TKEY> Data { get; set; } = null;
        public string ButtonState="click for serach";
        async Task Click()
        {
            await load();
        }
        public void onChange(object x)
        {
            serachData = x as Models.IQuery<TMODEL>;
            onchangeSerachDataRefrense?.Invoke(serachData);
        }
        public async Task load()
        {
            ButtonState = "searching";
            Console.WriteLine("load start");
        
            if (Data is null)
                Data = Program0.getEntityManager<TMODEL, TKEY>() as NewEntityService<TMODEL, TKEY>;
            Console.WriteLine("load getAll2");
            try{
                value = await Data.getAll2(serachData);
            }catch{
                ButtonState = "cant search try again " ;
                value=null;    
                return ;
            }
            Console.WriteLine("load end");
            Console.WriteLine("load end"+value.ToList().Count());
            ButtonState = "research " + value.ToList().Count();
        }

    }
}