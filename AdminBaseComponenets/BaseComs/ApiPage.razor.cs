


using Models;
using System.Threading.Tasks;
using System;

using System.ComponentModel;
using Microsoft.AspNetCore.Components;


using System.Net.Http;

using AdminClientViewModels;





namespace AdminBaseComponenets.BaseComs
{

    
    public partial class ApiPage<TINP,TOUT> : EditBase2<FuncV<TINP, TOUT>>
    {
        [Parameter]
        public string url{get;set;}


       

        

        
        public string ButtonState="send Action";

        async Task onChange(object x)
        {
            value.inp=(TINP)x;
        }


        async Task Click(){
            await load();
        }
        public async Task load()
        {
            ButtonState = "loading";
            Console.WriteLine("load start");

            //res = await ClTool2.WebClient.webClient.fetch<AdminMsg.LoginRequest, AdminMsg.LoginResponse>("adminUser/login",
            try{
                value.output = await ClTool.WebClient.webClient.fetch<TINP, TOUT>(url,
                    HttpMethod.Post, value.inp);
            }catch{
                ButtonState = "try again";                    
                return ;
            }

            ButtonState = "loaded";                
            
            
        }
        
    }
}