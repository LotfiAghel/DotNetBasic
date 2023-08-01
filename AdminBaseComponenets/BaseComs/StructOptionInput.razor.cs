


using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;


namespace AdminBaseComponenets.BaseComs
{

    public partial class StructOptionInput<ENUM> : ValueInput<ENUM>
        where ENUM : IEquatable<ENUM>
    {


        [Parameter]
        public IEnumerable<ENUM> optionGenerator { get; set; } = null;


        public Dictionary<int, ENUM> optionMap { get; set; } = new Dictionary<int, ENUM>();

        public int value0 { get; set; } = 0;


        [Parameter]
        public ComponentBase itemComponenet { get; set; } = null;

        
        







        protected override async Task OnInitializedAsync()
        {
            

        }

        protected async Task cc(ENUM vs)
        {
            await Task.Delay(30);

            StateHasChanged();
        }
        public async Task Click2(int vs0)
        {
            Console.WriteLine($"onClick2 onClick2 onClick2 {vs0}");
            ENUM vs = optionMap[vs0];
            Console.WriteLine($"onClick2 onClick2 onClick3 {vs}");
            value = vs;
            await Click();




            cc(vs);

        }

        public void load()
        {
            if(itemComponenet==null)
                itemComponenet= Program0.createWidget(typeof(ENUM), null);
            Console.WriteLine($"load load load0 ");
            Type enumType = typeof(ENUM);

            // I will get0s all values and iterate through them

            int i = 0;
            foreach(var o in optionGenerator)
            {
                
                if(o.Equals(value))
                    value0= i;
                optionMap[i++] = o;
            }



        }







    }


}