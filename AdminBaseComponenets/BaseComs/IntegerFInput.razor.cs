using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;
using System;
using System.Net.Http;
using AdminClientViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AdminBaseComponenets.BaseComs
{


    public partial class IntegerFInput<TEntity> : IntegerForeignKeyInput<TEntity> where TEntity:Models.IIdMapper<int>
    {
        ComponentBase ItemComponenet = null;
        bool panelOpenState;

        [Parameter]
        public IEnumerable<ForeignKey<TEntity>> generator { get; set; } =null;

        
        


        public int vvv
        {
            get => value.Value;
            set
            {

            }
        }



        protected override async Task OnInitializedAsync()
        {
            ItemComponenet = Program0.createWidget(typeof(ForeignKey<TEntity>), new List<Attribute>());
            await load();
        }


        protected async Task Click2(int vs)
        {
            Console.WriteLine($"onChange integerFUnput  {vs} ");
            value = vs;
            var bv = fValue;
            var vv = await Click();
            if (bv != null && bv.Equals(vv))
                return;
            if (vv == null)
                return;
            //w= new AdminBaseComponenets.BaseComs.IntegerFSmallView<Models.Coach>() { };
            //w = Program0.createWidget(vv.GetType(),  new List<Attribute>() { new ForeignKeyAttr(typeof(TEntity)) });
            //StateHasChanged();

        }
        protected async Task Click3()
        {
            Click2(vvv);

        }
        public async Task load()
        {
            var tmp = Program0.getEntityManager<TEntity>();


            if(generator==null)
                generator = (await tmp.getAll()).ToList().ConvertAll(x => new ForeignKey<TEntity>(x.id)); 
            // StateHasChanged();



        }

    }
}