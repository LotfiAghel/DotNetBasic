using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;
using System;
using System.Net.Http;
using AdminClientViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AdminBaseComponenets.BaseComs
{


    public partial class ForeignKeyEdite<TEntity,TKEY> : ForeignKeyEditeBase<TEntity,TKEY>
         where TEntity : class, Models.IIdMapper<TKEY>
         where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        ComponentBase ItemComponenet = null;
        bool panelOpenState;

        [Parameter]
        public IEnumerable<ForeignKey2<TEntity, TKEY>> generator { get; set; } =null;

        
        


        



        protected override async Task OnInitializedAsync()
        {
            ItemComponenet = Program0.createWidget(typeof(ForeignKey2<TEntity, TKEY>), new List<Attribute>());
            await load();
        }


        protected async Task Click2(TKEY vs)
        {
            Console.WriteLine($"onChange integerFUnput  {vs} ");
            value.setFValue(vs);
            Console.WriteLine($"onChange integerFUnput  {value.getFValue} ");
            var bv = fValue;
            var vv = await Click();
            if (bv != null && bv.Equals(vv))
                return;
            if (vv == null)
                return;
            //viewComponenet= new AdminBaseComponenets.BaseComs.IntegerFSmallView<Models.Coach>() { };
            //viewComponenet = Program0.createWidget(vv.GetType(),  new List<Attribute>() { new ForeignKeyAttr(typeof(TEntity)) });
            //StateHasChanged();

        }
        protected async Task Click3()
        {
            Click2(value.getFValue());

        }
        public async Task load()
        {
            var tmp = Program0.getEntityManager<TEntity, TKEY> ();


            if(generator==null)
                generator = (await tmp.getAll()).ToList().ConvertAll(x => new ForeignKey2<TEntity,TKEY>(x.id)); 
            // StateHasChanged();



        }

    }


    public class ForeignKeyEditeInt<TEntity> : ForeignKeyEdite<TEntity, int>
          where TEntity : class, Models.IIdMapper<int>
         
    { }
}