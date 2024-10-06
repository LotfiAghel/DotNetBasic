



using System.Threading.Tasks;
using System;
using System.ComponentModel;

using Microsoft.AspNetCore.Components;


using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using CsvHelper.Configuration;
using CsvHelper;
using System.Linq;






namespace AdminBaseComponenets.BaseComs
{

    
    public partial class DataListWithCsv<TItem, TKEY> : NullableInput2<IReadOnlyCollection<TItem>>
         where TItem : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {



        [Parameter]
        public string title { get; set; }
        [Parameter]
        public Type type { get; set; } = null;

       

        public object methodValue;
        public string ButtonState="send Action";
        public ComponentBase comp;

        static List<T> read<T>(Stream reader0) 
        {
            try
            {
                
                var config = CsvConfiguration.FromAttributes<TItem>();
                config.HeaderValidated = null;
                config.MissingFieldFound = null;
                using (var reader = new StreamReader(reader0))
                using (var csv = new CsvReader(reader, config))
                {
                   
                        List<T> records = csv.GetRecords<T>().ToList();
                        return records;
                   

                }
                   
            }
            catch
            {
                return new List<T>();
            }

        }

        private Guid inputFileId = Guid.NewGuid();
        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var selectedFile = e.GetMultipleFiles()[0];


            

            MemoryStream ms = new MemoryStream();


            await selectedFile.OpenReadStream().CopyToAsync(ms);
            ms.Position = 0;
            try
            {
                var x = read<TItem>(ms);
                value = x;
                if (OnChange != null)
                    OnChange(value);
            }
            catch(Exception ex)
            {
                Program0.showPopUp(ex);
            }
        }
    }
}