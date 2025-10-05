using System;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AdminBaseComponenets.BaseComs
{

    public partial class PopupGenral<TCOMP> : ValueInput<TCOMP> where TCOMP : ComponentBase
    {
        [Inject] public IModalService ModalService { get; set; }
        
      
        
        [Parameter] public TCOMP popupWidget { get; set; }= null;
        [Parameter] public Func<Task> OnCreate { get; set; }
        //[Parameter] public Func<TItem, Task> oncha { get; set; }
        private void OnChange2(object x)
        {
            OnChange?.Invoke(x);
        }

      
    }
}