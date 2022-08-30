using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;
using System;
using System.Net.Http;
using AdminClientViewModels;
using System.Collections.Generic;
using Models;

namespace AdminBaseComponenets.BaseComs
{


    public partial class IntegerFSmallView<TEntity> : IntegerForeignKeyInput<TEntity,int>
         where TEntity : class, Models.IIdMapper<int>
    {
        [Parameter]
        public ComponentBase w { get; set; } = null;
        [Parameter]
        public TEntity Data { get; set; } = default(TEntity);
        [Parameter]
        public int dataId { get; set; } = -1;


        protected override async Task OnInitializedAsync()
        {
            await load();
        }


        public async Task load()
        {

            Console.WriteLine("load");


            var tmp = Program0.getEntityManager<TEntity,int>();

            Data = (await tmp.get(value.Value));
            if (Data == null)
            {
                w = null;
                return;
            }
            //TODO not correct 
            dataId = Data.id;
            if (dataId != value.Value)
            {
                Console.WriteLine("bug");
            }

            w = Program0.createWidget(Data.GetType(), new List<Attribute>() { });
            StateHasChanged();



        }
        protected override bool ShouldRender()
        {
            return true;
        }

    }
}