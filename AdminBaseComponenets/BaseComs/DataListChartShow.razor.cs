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
        public string keyNAme { get; set; } = null;

    
       



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


    public void load()
    {
        int i = 0;
        foreach(var pr in typeof(TItem).GetProperties())
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

    private static string[] Labels = new string[] { "1", "2", "3", "4", "5", "6" };
    private static string[] BackgroundColors = new string[] { "#4bc0c0", "#36a2eb", "#ff3d88" };
    private static string[] BorderColors = new string[] { "#4bc0c0", "#36a2eb", "#ff3d88" };
    private Random random = new( DateTime.Now.Millisecond );

    protected override async Task OnAfterRenderAsync( bool firstRender )
    {
        if ( firstRender )
        {
            await lineChart.Clear();

            int i = 0;
            foreach (var pr in typeof(TItem).GetProperties())
            {
                await lineChart.AddLabels(i, value.Select(x => (decimal)pr.GetValue(x)).ToArray());
                await lineChart.AddData(i, value.Select(x => (decimal)pr.GetValue(x)).ToArray());
                i++;
            }

            await lineChart.Clear();

            
            
            i = 0;
            foreach (var pr in typeof(TItem).GetProperties())
            {
                await lineChart.AddLabels(i, value.Select(x => (decimal)pr.GetValue(x)).ToArray());
                await lineChart.AddData(i, value.Select(x => (decimal)pr.GetValue(x)).ToArray());
                i++;
            }
        }
    }

    private async Task HandleRedraw<TDataSet, TItem, TOptions, TModel>( Blazorise.Charts.BaseChart<TDataSet, TItem, TOptions, TModel> chart, Func<int, TDataSet> getDataSet )
        where TDataSet : ChartDataset<TItem>
        where TOptions : ChartOptions
        where TModel : ChartModel
    {
      
    }

    private LineChartDataset<decimal> GetLineChartDataset( int colorIndex ,PropertyInfo pi,IReadOnlyCollection<TItem> items)
    {
        return new()
        {
            Label = "# of randoms",
            Data = items.Select(x=> (decimal)pi.GetValue(x)).ToList(),
            BackgroundColor = BackgroundColors[colorIndex],
            BorderColor = BorderColors[colorIndex],
        };
    }

    


    }


}