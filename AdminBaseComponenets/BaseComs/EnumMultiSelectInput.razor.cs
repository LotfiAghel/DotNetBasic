using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;


using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;


namespace AdminBaseComponenets.BaseComs
{
    
    public partial class EnumMultiSelectInput<ENUM> : EnumArrayInput<ENUM> 
    {


        
        public int value0 { get; set; }=0;

        public ENUM nextValue { get; set; }

        
        
        



        
        protected void firstRun()
        {
            if (generator == null)
            {
                generator = Program0.createGenerator(typeof(ENUM), null) as MarkedGenerator<ENUM>; //

            }
            generator.load(value);

            if(itemComponenet==null)
                itemComponenet  = Program0.createWidget(typeof(ENUM), null);
            if(itemCreateComponenet==null)
                itemCreateComponenet = Program0.createForm4(typeof(ENUM), null) as OptionsValueInput<ENUM>;
            itemCreateComponenet.optionGenerator = generator;
            itemCreateComponenet.OnChange = async (x) =>
            {
                Console.WriteLine($"Click3 Click3 00 {x}");
                await Click3((ENUM)x);
            };

        }


        protected override async Task OnInitializedAsync()
        {

            firstRun();

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
            Console.WriteLine($"onClick2 onClick2 onClick30 {vs}");
            // ENUM vs=(ENUM)vs0;
            await Click3(vs);

        }
        public async Task Click3(ENUM vs)
        {
            
            await Click();
            
            value.Add(vs);
            

            generator.onAdd(vs);

            cc(vs);

        }
        protected async Task remove(ENUM vs)
        {
            Console.WriteLine("remove " + vs);
            value.RemoveAll(x => x.Equals(vs));
            Console.WriteLine("remove1 " + vs);
            
            generator.onRemove(vs);
            Console.WriteLine("remove2 " + vs);


            StateHasChanged();


        }


        int currentIndex;

        void StartDrag(ENUM itemId)
        {
            currentIndex = GetIndex(itemId);
            Console.WriteLine($"DragStart for {itemId} index {currentIndex}");
        }



        int GetIndex(ENUM itemId)
        {


            return value.FindIndex(x => x.Equals(itemId));
        }

        void Drop(ENUM itemId)
        {

            {
                Console.WriteLine($"Drop item { itemId}");
                var index = GetIndex(itemId);
                Console.WriteLine($"Drop index is {index}, move from {currentIndex}");
                // get0s current item
                var current = value[currentIndex];


                var currenti = value[currentIndex];
                value.RemoveAt(currentIndex);
                value.Insert(index, currenti);


                // update current selection
                currentIndex = index;

                StateHasChanged();
            }
        }


    }


}