using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Tools;
using Models;
using Blazorise;
using Microsoft.AspNetCore.Components;

namespace AdminBaseComponenets.BaseComs
{


    public partial class IntInput3Edite<TKEY> : ValueInput<TKEY>
         where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        [Parameter]
        public IEnumerable<TKEY> optionGenerator { get; set; } = null;
        
        bool panelOpenState;


        Type[] genericArgs = new Type[] { null, typeof(int) };







        Type gridMetaClass = null;

        private Modal modalRef;

        bool showModal = false;


        void ModalCancel() => showModal = false;
        void ModalOk()
        {
            Console.WriteLine("Modal ok");
            showModal = false;
        }
       


        protected async Task Click2(TKEY vs)
        {
            showModal = false;
            Console.WriteLine($"onChange integerFUnput  {vs} ");
            
            Console.WriteLine($"onChange integerFUnput  {value} ");
            var bv = value;
            
            if (bv != null && bv.Equals(vs))
                return;
            if (vs == null)
                return;
            //viewComponenet= new AdminBaseComponenets.BaseComs.IntegerFSmallView<Models.Coach>() { };
            //viewComponenet = Program0.createWidget(vv.GetType(),  new List<Attribute>() { new ForeignKeyAttr(typeof(TEntity)) });
            StateHasChanged();

        }
        protected async Task Click3()
        {
            Click2(value);

        }
        
        
    }


    
}