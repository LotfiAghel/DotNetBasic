


using Models;
using System.Threading.Tasks;
using System;

using System.ComponentModel;
using Microsoft.AspNetCore.Components;


using System.Net.Http;







namespace AdminBaseComponenets.BaseComs
{

    
    public partial class ApiPage<TINP,TOUT> : ComponentBase
    {
        [Parameter]
        public string value{get;set;}


        public TINP inp { get; set; }


        public TOUT output{get;set;}


        

        
        public string ButtonState="send Action";

        async Task onChange(object x)
        {
            inp=(TINP)x;
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
                output = await ClTool.WebClient.webClient.fetch<TINP, TOUT>(value,
                    HttpMethod.Post, inp);
            }catch{
                ButtonState = "try again";                    
                return ;
            }

            ButtonState = "loaded";                
            
            
        }
        
    }
}