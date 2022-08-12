



using System.Threading.Tasks;
using System;
using System.ComponentModel;

using Microsoft.AspNetCore.Components;


using System.Net.Http;
using System.Collections.Generic;






namespace AdminBaseComponenets.BaseComs
{

    
    public partial class ApiCall : ComponentBase 
    {
        [Parameter]
        public string url{get;set;}


        [Parameter]
        public string method{get;set;}

        
        public string ButtonState="send Action";
        public ComponentBase comp;
        public Type type;

        public void reload(){
            type=Program0.getFuncType(url);
            comp=Program0.createForm(type, new List<Attribute>() { });
        }
        
        
    }
}