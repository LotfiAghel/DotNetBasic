using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;
using System;
using System.Net.Http;
using AdminClientViewModels;
using System.Collections.Generic;
using Models;

namespace AdminBaseComponenets.BaseComs
{


    public partial class IntegerFSmallView<TEntity, TKEY> : IntegerForeignKeyInput<TEntity, TKEY>
         where TEntity : class, Models.IIdMapper<TKEY>
         where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        [Parameter]
        public ComponentBase viewComponenet { get; set; } = null;

        [Parameter]
        public TEntity Data { get; set; } = default(TEntity);

        [Parameter]
        public TKEY dataId { get; set; } = default(TKEY);


        protected override async Task OnInitializedAsync()
        {
            await load();
        }


        public async Task load()
        {

            Console.WriteLine("load");


            var tmp = Program0.getEntityManager<TEntity, TKEY>();

            Data = (await tmp.get(value.Value));
            if (Data == null)
            {
                viewComponenet = null;
                return;
            }
            //TODO not correct 
            dataId = Data.id;
            if (!dataId.Equals( value.Value) )
            {
                Console.WriteLine("bug");
            }

            viewComponenet = Program0.createWidget(Data.GetType(), new List<Attribute>() { });
            StateHasChanged();



        }
        protected override bool ShouldRender()
        {
            return true;
        }

    }
}