using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;
using System;
using System.Net.Http;
using AdminClientViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AdminBaseComponenets.BaseComs
{


    public partial class IntegerFMultiSelect<TEntity> : IntegerForeignKeyArrayInput<TEntity> where TEntity:Models.IIdMapper<int>
    {


        ComponentBase itemComponenet = Program0.createWidget(typeof(ForeignKey<TEntity>), null);

        

        

        protected override async Task OnInitializedAsync()
        {
            await load();
            
        }

        protected async Task cc(int vs)
        {
            await Task.Delay(30);

            StateHasChanged();
        }
        


        protected async Task Click3(ForeignKey<TEntity> vs)
        {
            

            value.Add(new ForeignKey<TEntity>(vs));
            generator.onAdd(vs);    

            

        }
        protected async Task remove(int vs)
        {
            Console.WriteLine("remove " + vs);
            value.RemoveAll(x => x.Value == vs);
            generator.onRemove(vs); 


            StateHasChanged();


        }
        public async Task load()
        {
            

            if (generator == null)
            {
                generator = new MarkedGenerator<ForeignKey<TEntity>>();
                var tmp = Program0.getEntityManager<TEntity,int>();
                var x = (await tmp.getAll()).ToList();
                
                generator.initList(x.ConvertAll(x => new ForeignKey<TEntity>(x.id)));
                generator.load(value);
            }




        }


        int currentIndex;

        void StartDrag(int itemId)
        {
            currentIndex = GetIndex(itemId);
            Console.WriteLine($"DragStart for {itemId} index {currentIndex}");
        }



        int GetIndex(int itemId)
        {


            return value.FindIndex(x => x == itemId);
        }

        void Drop(int itemId)
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