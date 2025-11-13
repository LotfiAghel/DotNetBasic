using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Blazorise.Charts;

namespace AdminBaseComponenets.BaseComs
{
    public partial class DataListChartShow2<TItem, TKEY> : NullableInput2<IReadOnlyCollection<TItem>>
        where TItem : class, Models.IIdMapper<TKEY>
        where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        private LineChart<decimal> lineChart;
        private List<PropertyInfo> decimalProperties;
        private static Dictionary<string, string> colorMap = new();
        private static int colorIndex = 0;
        private static readonly string[] colors = new[]
        {
            "#0072B2ff", "#D55E00ff", "#009E7388", "#CC79A788", "#E69F0088",
            "#F0E442ff", "#56B4E9ff", "#99999988", "#80800088", "#80000088"
            , "#99999988", "#80800088", "#80000088"
        };
        List<string> Labels = new List<string>();

        // Add chart options to control x-axis ticks
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
                X = new()
                {
                    Ticks = new()
                    {
                        Count = 0 // 0 means no limit on max ticks
                    }
                },
                Y = new()
                {
                    Stacked = true,
                }
            },
            Plugins = new()
            {
                Legend = new()
                {
                    Display = true
                }
            }
        };

        async Task HandleRedraw()
        {
            await lineChart.Clear();
            Labels.Clear();
            
            var datasets = GetLineChartDatasets();
            await lineChart.AddLabelsDatasetsAndUpdate(Labels, datasets.ToArray());
        }
        LineChartDataset<decimal>[] GetLineChartDatasets()
        {
            var prs = typeof(TItem).GetRuntimeProperties().ToList().Where(x => x.PropertyType == typeof(decimal) || x.PropertyType == typeof(int) ).ToList();
            //prs2 = typeof(TItem).GetRuntimeProperties().ToList().Where(x => x.PropertyType == typeof(decimal) ).ToList();
            int i = 0;
            Labels.Clear();
            foreach(var pr in value)
                if(pr.id is DateTime dt)
                    Labels.Add(dt.ToPersianDateString());
                else
                    Labels.Add(pr.id.ToString());

            Console.WriteLine($"Labels count: {Labels.Count}");
            Console.WriteLine($"Properties count: {prs.Count}");

            var res = new List<LineChartDataset<decimal>>();
            foreach (var pr in prs)
            {
                var dataList = (pr.PropertyType==typeof(decimal))? value.Select(x => (decimal)pr.GetValue(x)!).ToList() : value.Select(x => (int)pr.GetValue(x)).Select(x=> (decimal)x).ToList();
                Console.WriteLine($"Property {pr.Name} data count: {dataList.Count}");
                res.Add(new LineChartDataset<decimal>
                {
                    Label = pr.Name,
                    Data = dataList,
                    
                    BackgroundColor = dataList.Select(x=> "transparent").ToList(),
                    BorderColor = dataList.Select(x=> GetColor(pr.Name)).ToList(),
                    Fill = true,
                    PointRadius = 3,
                    CubicInterpolationMode = "monotone",
                });
            }

            return res.ToArray();
        }


        protected override async Task OnAfterRenderAsync( bool firstRender )
        {
            //if ( firstRender )
            {
                await HandleRedraw();
                await HandleRedraw2();
            }
        }

      

        private string GetColor(string propName)
        {
            if (!colorMap.ContainsKey(propName))
            {
                colorMap[propName] = colors[colorIndex % colors.Length];
                colorIndex++;
            }
            return colorMap[propName];
        }
    }
}