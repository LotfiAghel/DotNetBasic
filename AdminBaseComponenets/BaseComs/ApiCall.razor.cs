



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
        public HttpMethod method {get;set;}


        [Parameter]
        public string title { get; set; }
        [Parameter]
        public Type type { get; set; } = null;

       

        public object methodValue;
        public string ButtonState="send Action";
        public ComponentBase comp;
        
        public void reload(){
            if(type==null)
                type=Program0.getFuncType(url);
            if (type != null)
            {
                comp = Program0.createForm(type, new List<Attribute>() { });
                ((dynamic)comp).url = url;
                ((dynamic)comp).title = title;
                ((dynamic)comp).method = method;
                methodValue = type.GetConstructor(new Type[] { }).Invoke(new object[] { });
            }
        }
        
        
    }
}