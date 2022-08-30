using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;
using System;
using System.Net.Http;
using AdminClientViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AdminBaseComponenets.BaseComs
{


    public partial class IntegerFMultiSelect<TEntity,TKEY> : IntegerForeignKeyArrayInput<TEntity, TKEY>
          where TEntity : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {


        ComponentBase itemComponenet = Program0.createWidget(typeof(ForeignKey2<TEntity,TKEY>), null);

        

        

        protected override async Task OnInitializedAsync()
        {
            await load();
            
        }

        protected async Task cc(int vs)
        {
            await Task.Delay(30);

            StateHasChanged();
        }
        


        protected async Task Click3(ForeignKey2<TEntity,TKEY> vs)
        {
            

            value.Add(new ForeignKey2<TEntity,TKEY>(vs));
            generator.onAdd(vs);    

            

        }
        protected async Task remove(TKEY vs)
        {
            Console.WriteLine("remove " + vs);
            value.RemoveAll(x => x.Value.Equals(vs));
            generator.onRemove(vs); 


            StateHasChanged();


        }
        public async Task load()
        {
            

            if (generator == null)
            {
                generator = new MarkedGenerator<ForeignKey2<TEntity,TKEY>>();
                var tmp = Program0.getEntityManager<TEntity,TKEY>();
                var x = (await tmp.getAll()).ToList();
                
                generator.initList(x.ConvertAll(x => new ForeignKey2<TEntity, TKEY>(x.id)));
                generator.load(value);
            }




        }


        int currentIndex;

        void StartDrag(TKEY itemId)
        {
            currentIndex = GetIndex(itemId);
            Console.WriteLine($"DragStart for {itemId} index {currentIndex}");
        }



        int GetIndex(TKEY itemId)
        {


            return value.FindIndex(x => x .Equals(itemId));
        }

        void Drop(TKEY itemId)
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


    public class IntegerFMultiSelect<TEntity> : IntegerFMultiSelect<TEntity, int>
            where TEntity : class, Models.IIdMapper<int>
              
    { }
}