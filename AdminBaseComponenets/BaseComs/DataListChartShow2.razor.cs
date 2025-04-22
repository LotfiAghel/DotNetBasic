using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using PSC.Blazor.Components.Chartjs;
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



        private static Dictionary<string, string> colormp = new();
        private static Random rr = new Random();
        
    private List<PropertyInfo> prs;
    //private List<PropertyInfo> prs2;
    public void load()
    {
        prs = typeof(TItem).GetRuntimeProperties().ToList().Where(x => x.PropertyType == typeof(decimal) || x.PropertyType == typeof(int) ).ToList();
        //prs2 = typeof(TItem).GetRuntimeProperties().ToList().Where(x => x.PropertyType == typeof(decimal) ).ToList();
        int i = 0;
        foreach(var pr in value)
            if(pr.id is DateTime dt)
                _config1.Data.Labels.Add(dt.ToPersianDateString());
            else
                _config1.Data.Labels.Add(pr.id.ToString());
        
        foreach (var pr in prs)
            _config1.Data.Datasets.Add(new LineDataset()
            {
                Label = pr.Name,
                Data = (pr.PropertyType==typeof(decimal))? value.Select(x => (decimal?)pr.GetValue(x)).ToList() : value.Select(x => (int)pr.GetValue(x)).Select(x=> (decimal?)x).ToList(),
                BackgroundColor = "#00000000",// getColor(pr.Name),
                BorderColor =getColor(pr.Name),
                Fill = true
            });
        _chart1.Height = "500";
        _chart1.Style = "display: block; box-sizing: border-box; height: 450px; width: 753px;";

    }

    private static int t = 0;

    private static string[] colors =
    [
        "#0072B2ff", "#D55E00ff", "#009E7388", "#CC79A788", "#E69F0088", 
        "#F0E442ff", "#56B4E9ff", "#99999988", "#80800088", "#80000088"
    ];
    

    private static string getColor(string prName)
    {
        if (!colormp.ContainsKey(prName))
            colormp[prName] = colors[(t++)%(colors.Length)];
                //$"rgba({rr.Next(255)},{rr.Next(255)},{rr.Next(255)},0.2)";
        return colormp[prName];
    }
    }


}