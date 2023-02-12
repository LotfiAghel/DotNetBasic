using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;
using System;
using System.Net.Http;
using AdminClientViewModels;
namespace AdminBaseComponenets.BaseComs
{
    public interface IAFunc0
    {
         void setUrl(string u);
    };

    public partial class AFunc<TINP, TOUT> : EditBase2<FuncV<TINP, TOUT> >, IAFunc0
    {
        [Parameter]
        public string url { get; set; }


        


        

        bool done=false;



        public string ButtonState = "send Action";
        public void setUrl(string u)
        {
            url = u;
        }
        public void reload()
        {
            
            
        }
        public void onChangeInp(object x)
        {
            value.inp =(TINP)x;
        }



       


        async Task Click()
        {
            await load();
        }
        public async Task load() {

         
            ButtonState = "loading";
            Console.WriteLine("load start");
            try
            {
                value.output = await ClTool.WebClient.webClient.fetch<TINP, TOUT>(url,
                    HttpMethod.Post, value.inp);
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