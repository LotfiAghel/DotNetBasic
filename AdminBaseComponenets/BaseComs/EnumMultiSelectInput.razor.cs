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
    
    public partial class EnumMultiSelectInput<ENUM> : EnumArrayInput<ENUM> where ENUM : System.Enum
    {


        
        public int value0 { get; set; }=0;

        public ENUM nextValue { get; set; }

        [Parameter]
        public ComponentBase itemComponenet { get; set; }

        
        



        
        protected void firstRun()
        {
            if (generator == null)
            {
                generator = new MarkedGenerator<ENUM>();
                
            }
            generator.load(value);

            if(itemComponenet==null)
                itemComponenet  = Program0.createWidget(typeof(ENUM), null);


        }


        protected override async Task OnInitializedAsync()
        {
            if (generator == null) { 
                generator = new MarkedGenerator<ENUM>();
                //generator.initList(typeof(ENUM).GetEnumValues().);
            }
            generator.load(value);

            if(itemComponenet==null)
                itemComponenet  = Program0.createWidget(typeof(ENUM), null);


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
            
            await Click();
            Console.WriteLine($"onClick2 onClick2 onClick3 {vs}");
            value.Add(vs);
            Console.WriteLine($"onClick2 onClick2 onClick4 {value.Count}");

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



        int GetIndex(Enum itemId)
        {


            return value.FindIndex(x => x.Equals(itemId));
        }

        void Drop(Enum itemId)
        {

            {
                Console.WriteLine($"Drop item { itemId}");
                var index = GetIndex(itemId);
                Console.WriteLine($"Drop index is {index}, move from {currentIndex}");
                // get current item
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