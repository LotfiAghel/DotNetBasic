using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;
using System;
using System.Net.Http;
using AdminClientViewModels;
using System.Collections.Generic;
using Models;

namespace AdminBaseComponenets.BaseComs.InGrid
{


    public partial class NullableInGrid<T> : ValueInput<T?> where T : struct
         
    {
        [Parameter]
        public ComponentBase viewComponenet { get; set; } = null;

        

        protected override async Task OnInitializedAsync()
        {
            load();
        }

        private string dataId;

        private void load()
        {

            Console.WriteLine("load");


           
            dataId = value!=null ? $"{value.GetHashCode()}" : "null";
           
            viewComponenet ??= Program0.createWidget(typeof(T), Attributes);
            StateHasChanged();
            
        }
        protected override bool ShouldRender()
        {
            return true;
        }

    }
}