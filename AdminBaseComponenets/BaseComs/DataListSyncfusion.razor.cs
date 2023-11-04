using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;


using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using Blazorise.DataGrid;
using Models;
using Tools;
using Newtonsoft.Json;

namespace AdminBaseComponenets.BaseComs
{
    
    public partial class DataListSyncfusion<TItem,TKEY> : NullableInput2<IEnumerable<TItem>>
         where TItem : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {




        [Parameter]
        public string itemName { get; set; } = null;


        [Parameter]
        public string Url { get; set; } = null;

        [Parameter]
        public TKEY itemValue { get; set; }

        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }




        bool widget = true;

        string EntityName = typeof(TItem).GetUrlEncodeName();


        public List<System.Reflection.PropertyInfo> propertis = typeof(TItem).GetProperties()
            .Where(prop =>
            {

                if (prop.PropertyType.IsGenericType
                && prop.PropertyType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.ICollection<>))
                    return false;

                
                if (prop.GetCustomFirstAttributes<Models.IgnoreDefultGird>() != null)
                    return false;
                if (prop.GetCustomFirstAttributes<JsonIgnoreAttribute>() != null)
                    return false;
                return true;
            }).ToList();


        ComponentBase w = null;
        TItem ttvalue;

        bool showModal = false;





        void ModalShow()
        {
            var pr = typeof(TItem).GetProperty(itemName);
            ttvalue = (TItem)(typeof(TItem).GetConstructor(new Type[] { }).Invoke(new object[] { }));
            w = Program0.createForm(typeof(TItem), new List<Attribute>());
            //Data.Add(ttvalue);
            showModal = true;
        }
        void ModalCancel() => showModal = false;
        void ModalOk()
        {
            Console.WriteLine("Modal ok");
            showModal = false;
        }
        public void onClick(DataGridRowMouseEventArgs<TItem> e)
        {



            var itemId = (e.Item as IIdMapper<TKEY>).id;
            if(Url is null)
                NavManager.NavigateTo($"{EntityName}/edit/{itemId}");
            else
                NavManager.NavigateTo($"{Url}/{itemId}");

        }




    }


}