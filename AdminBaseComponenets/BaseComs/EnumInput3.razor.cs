


using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;


namespace AdminBaseComponenets.BaseComs
{

    public partial class EnumInput3<ENUM> : OptionsValueInput<ENUM> where ENUM : System.Enum
    {


        
        public int value0 { get; set; } = 0;


        
        







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
            ENUM vs = (ENUM)Convert.ChangeType(vs0, typeof(int));
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
            
            if (optionGenerator == null)
            {
                var enumValues = enumType.GetEnumValues();

                List<ENUM> ee = new List<ENUM>();
                optionGenerator = ee;
                Console.WriteLine($"load optionGenerator load0 ");
                foreach (ENUM value in enumValues)
                {
                    ee.Add(value);
                    if (value.Equals(this.value))
                    {
                        value0 = ee.Count-1;
                    }

                    



                }

            }
            //for (int i=0; i<enumValues.Length; ++i)



        }







    }


}