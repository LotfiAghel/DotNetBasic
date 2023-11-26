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
using Microsoft.JSInterop;
using System.Threading;
using AdminClientViewModels;

namespace AdminBaseComponenets.BaseComs
{
    
    public partial class DataListSyncfusion<TItem,TKEY> : NullableInput2<IReadOnlyCollection<TItem>>
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


        TItem selectedTItem;

        bool widget = true;

        string EntityName = typeof(TItem).GetUrlEncodeName();


        public List<System.Reflection.PropertyInfo> propertis = typeof(TItem).GetProperties()
            .Where(prop =>
            {

                if (prop.GetCustomFirstAttributes<Models.IgnoreDefultGird>() != null)
                    return !prop.GetCustomFirstAttributes<Models.IgnoreDefultGird>().isIgnore;


                if (prop.PropertyType.IsGenericType
                && prop.PropertyType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.ICollection<>))
                    return false;

                
                
                if (prop.GetCustomFirstAttributes<JsonIgnoreAttribute>() != null)
                    return false;
                return true;
            }).ToList();


        ComponentBase w = null;
        TItem ttvalue;

        bool showModal = false;

        List<TItem> value2;

        private int totalTItems;

        private async Task OnReadData(DataGridReadDataEventArgs<TItem> e)
        {
            if (!e.CancellationToken.IsCancellationRequested)
            {
                if (value is List<TItem> li) { 
                    value2 = li.GetRange((e.Page - 1) * e.PageSize, Math.Min(e.PageSize,li.Count - (e.Page - 1) * e.PageSize));
                    totalTItems = li.Count;
                    return;
                }
                if (value is PaginateList<TItem,TKEY> pl)
                {
                    value2 = pl.GetRange((e.Page-1)*e.PageSize, e.PageSize);
                    totalTItems = pl.Count;
                    return;
                }
                
            }
        }

        void ModalShow(TItem t )
        {
            //@Program0.CreateDynamicComponent2(this, w, prop.GetValue(context))
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
        public async Task onClick2(DataGridRowMouseEventArgs<TItem> e)
        {


            selectedTItem = e.Item;
            //var itemId = (e.Item as IIdMapper<TKEY>).id;
            //object value1 = await JSRuntime.InvokeAsync<object>("open","blank");
            

        }




    }


}