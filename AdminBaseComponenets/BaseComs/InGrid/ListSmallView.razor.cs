using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;
using System;
using System.Net.Http;
using AdminClientViewModels;
using System.Collections.Generic;
using Models;

namespace AdminBaseComponenets.BaseComs.InGrid
{


    public partial class ListSmallView<TEntity> : ValueInput<List<TEntity>>
    {
        [Parameter]
        public ComponentBase viewComponenet { get; set; } = null;

        

        protected override async Task OnInitializedAsync()
        {
            await load();
        }


        public async Task load()
        {

            Console.WriteLine("load");


           
            if (viewComponenet == null)
            {
                viewComponenet = Program0.createWidget(typeof(TEntity), new List<Attribute>() { });
            }
            StateHasChanged();



        }
        protected override bool ShouldRender()
        {
            return true;
        }

    }
}