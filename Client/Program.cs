
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using Newtonsoft.Json;
using ClTool;
using Newtonsoft.Json.Linq;

namespace Client
{
    class Program
    {

        static void test()
        {
            PerimitveContainer<int> v = new PerimitveContainer<int>(10);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,

            };


            var settings1 = JsonSerializer.Create(settings);
            settings1.Converters.Add(new RialConverter());
            settings1.Converters.Add(new PerimitveContainerConvertor());
            settings1.Converters.Add(new ForeignKeyConverter());


            var s=JToken.FromObject(v, settings1);
            Console.WriteLine(s);

        }
        static void Main(string[] args)
        {
            test();

            Console.WriteLine("Hello World!");
        }
    }
}
