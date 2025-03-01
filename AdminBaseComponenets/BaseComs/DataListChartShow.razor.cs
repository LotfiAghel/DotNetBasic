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

namespace AdminBaseComponenets.BaseComs
{
    
    public partial class DataListChartShow<TItem,TKEY> : NullableInput2<IReadOnlyCollection<TItem>>
         where TItem : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {




        [Parameter]
        public string keyName { get; set; } = null;

    
       



         private LineChart<decimal> lineChart;

    // define regular chart options
    LineChartOptions lineChartOptions = new()
    {
        AspectRatio = 5d / 3d,
        Layout = new()
        {
            Padding = new()
            {
                Top = 32,
                Right = 16,
                Bottom = 16,
                Left = 8
            }
        },
        Elements = new()
        {
            Line = new()
            {
                Fill = false,
                Tension = 0.4,
            }
        },
        Scales = new()
        {
            Y = new()
            {
                Stacked = true,
            }
        },
        Plugins = new()
        {
            Legend = new()
            {
                Display = false
            }
        }
    };

    // define specific dataset styles by targeting them with the DatasetIndex
    List<ChartDataLabelsDataset> lineDataLabelsDatasets = new() {};

    private List<PropertyInfo> prs;
    public void load()
    {
        prs = typeof(TItem).GetRuntimeProperties().ToList().Where(x => x.PropertyType == typeof(decimal)).ToList();
        int i = 0;
        foreach(var pr in prs)
            lineDataLabelsDatasets.Add(new ChartDataLabelsDataset()
            {
                DatasetIndex = i++,
                Options = new()
                {
                    BackgroundColor = BackgroundColors[0],
                    BorderColor = BorderColors[0],
                    Align = "start",
                    Anchor = "start"
                }
            });
    }
    
    // some shared options for all data-labels
    ChartDataLabelsOptions lineDataLabelsOptions = new()
    {
        BorderRadius = 4,
        Color = "#ffffff",
        Font = new()
        {
            Weight = "bold"
        },
        Formatter = ChartMathFormatter.Round,
        Padding = new( 6 )
    };

    
    private static string[] BackgroundColors = new string[] { "#4bc0c0", "#36a2eb", "#ff3d88" };
    private static string[] BorderColors = new string[] { "#4bc0c0", "#36a2eb", "#ff3d88" };
    private Random random = new( DateTime.Now.Millisecond );

    protected override async Task OnAfterRenderAsync( bool firstRender )
    {
        Console.WriteLine(value.Count());
        if ( firstRender )
        {
            load();
            await lineChart.Clear();

            string[] Labels =value.Select(x=> x.id.ToString()).ToArray();
            var Data=prs.Select(pr=> new LineChartDataset<decimal>()
            {
                Label = $"# of randoms {pr.Name}",
                Data = value.Select(x => (decimal)pr.GetValue(x)).ToList()
            }).ToArray();

            int i = 0;
            //foreach (var pr in prs)
            {
                await lineChart.AddLabelsDatasetsAndUpdate(Labels,Data );
                i++;
            }

            await lineChart.Clear();

            
            i = 0;
            //foreach (var pr in prs)
            {
                await lineChart.AddLabelsDatasetsAndUpdate(Labels,Data );
                
                i++;
            }
        }
    }

   

    


    }


}