using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;


using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Blazorise.DataGrid;
using Models;
using Tools;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using System.Threading;
using AdminClientViewModels;
using Blazorise.Charts;
using Blazorise.Charts.DataLabels;
using PSC.Blazor.Components.Chartjs;
using PSC.Blazor.Components.Chartjs.Models.Bar;
using PSC.Blazor.Components.Chartjs.Models.Common;
using PSC.Blazor.Components.Chartjs.Models.Line;

namespace AdminBaseComponenets.BaseComs
{
    
    public partial class DataListChartShow2<TItem,TKEY> : NullableInput2<IReadOnlyCollection<TItem>>
         where TItem : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {




        [Parameter]
        public string keyName { get; set; } = null;

    
       



        private LineChartConfig? _config1;
        private Chart? _chart1;

        protected override async Task OnInitializedAsync()
        {
            _config1 = new LineChartConfig()
            {
                Options = new Options()
                {
                    Responsive = true,
                    MaintainAspectRatio = false
                }
            };
            load();

            
        }
    



    private List<PropertyInfo> prs;
    public void load()
    {
        prs = typeof(TItem).GetRuntimeProperties().ToList().Where(x => x.PropertyType == typeof(decimal)).ToList();
        int i = 0;
        foreach(var pr in value)
            _config1.Data.Labels.Add(pr.id.ToString());
        foreach (var pr in prs)
            _config1.Data.Datasets.Add(new LineDataset()
            {
                Label = pr.Name,
                Data = value.Select(x => (decimal?)pr.GetValue(x)).ToList(),
                BackgroundColor = "rgba(75,192,192,0.2)",
                BorderColor = "rgba(75,192,192,1)",
                Fill = true
            });
            
    }
    
  
    
   

    


    }


}