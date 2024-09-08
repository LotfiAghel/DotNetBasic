using AdminBaseComponenets;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq;
using Tools;

namespace AdminBaseComponenets.BaseComs
{
    public partial class PointerInput2<TItem> : ValueInput<TItem> where TItem : class
    {





        [Parameter]
        public object valueContainer { get; set; }



        [Parameter]
        public string itemName { get; set; }



        



        public Type classType { get; set; }=null;


        public List<Type> classess = new List<Type>();
      
        public PointerInput2()
        {
            classess=typeof(TItem).GetSubClasses();
        }
        protected override async Task OnInitializedAsync()
    {

        if (classType == null && value != null)
        {
            classType = value.GetType();
            widget = Program0.createForm(value.GetType(), new List<Attribute>() { });
        }
    }


       
    }


}