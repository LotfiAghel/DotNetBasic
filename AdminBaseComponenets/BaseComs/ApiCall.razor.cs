



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

        public object methodValue;
        public string ButtonState="send Action";
        public ComponentBase comp;
        public Type type;

        public void reload(){
            type=Program0.getFuncType(url);
            comp=Program0.createForm(type, new List<Attribute>() { });
            ((dynamic)comp).url = url;
            ((dynamic)comp).method = method;
            methodValue = type.GetConstructor(new Type[] {}).Invoke(new object[] { });
        }
        
        
    }
}