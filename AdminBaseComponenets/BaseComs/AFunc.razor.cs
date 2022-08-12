using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;
using System;
using System.Net.Http;
using AdminClientViewModels;
namespace AdminBaseComponenets.BaseComs
{


    public partial class AFunc<TINP, TOUT> : ComponentBase //: ValueInput<FuncV<TINP, TOUT> >
    {
        [Parameter]
        public string value { get; set; }


        public FuncV<TINP, TOUT> inpOut { get; set; }=new FuncV<TINP, TOUT>();


        

        bool done=false;



        public string ButtonState = "send Action";

        async Task onChange(object x)
        {
            inpOut.inp =(TINP)x;
        }


        async Task Click()
        {
            await load();
        }
        public async Task load()
        {
            ButtonState = "loading";
            Console.WriteLine("load start");
            try
            {
                inpOut.output = await ClTool.WebClient.webClient.fetch<TINP, TOUT>(value,
                    HttpMethod.Post, inpOut.inp);
            }
            catch
            {
                ButtonState = "try again"; //TODO show notification
                return;
            }
            done=true;
            ButtonState = "done()";


        }

    }
}