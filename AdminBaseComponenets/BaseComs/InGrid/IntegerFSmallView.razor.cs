using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;
using System;
using System.Net.Http;
using AdminClientViewModels;
using System.Collections.Generic;
using Models;

namespace AdminBaseComponenets.BaseComs.InGrid
{


    public partial class IntegerFSmallView<TEntity, TKEY> : ForeignKeyEditeBase<TEntity, TKEY>
         where TEntity : class, Models.IIdMapper<TKEY>
         where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        [Parameter]
        public ComponentBase viewComponenet { get; set; } = null;

        [Parameter]
        public TKEY dataId { get; set; } = default(TKEY);


        protected override async Task OnInitializedAsync()
        {
            await load();
        }


        public async Task load()
        {

            Console.WriteLine("load");


            var tmp = Program0.getEntityManager<TEntity, TKEY>();

            fValue = (await tmp.get(value.getFValue()));
            if (fValue == null)
            {
                viewComponenet = null;
                return;
            }
            //TODO not correct 
            dataId = fValue.id;
            if (!dataId.Equals( value.getFValue()) )
            {
                Console.WriteLine("bug");
            }
            if (viewComponenet == null)
            {
                viewComponenet = Program0.createWidget(fValue.GetType(), new List<Attribute>() { });
            }
            StateHasChanged();



        }
        protected override bool ShouldRender()
        {
            return true;
        }

    }
}