using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;
using System;
using System.Net.Http;
using AdminClientViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AdminBaseComponenets.BaseComs
{


    public partial class MyFlatSelect<TValue> : OptionsValueInput<TValue> where  TValue:class
    {





        [Parameter]
        public List<TValue> options {get;set;}

        [Parameter]
        public Action<TValue> SelectedValueChanged{get;set;}
        protected override async Task OnInitializedAsync()
        {
            load();

        }

        protected async Task cc(int vs)
        {
            await Task.Delay(30);

            StateHasChanged();
        }



        protected void onclick(TValue vs)
        {
            this.value=vs;
            SelectedValueChanged(vs);
            

        }
       public void load()
        {

            itemComponenet = Program0.createWidget(typeof(TValue), null);
          

        }



       


      
    }


}