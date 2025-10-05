using System;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AdminBaseComponenets.BaseComs
{

    public partial class PopupForm<TItem> : ValueInput<TItem> where TItem : class
    {
        [Inject] public IModalService ModalService { get; set; }
        ComponentBase popupWidget = null;
        [Parameter] public Func<TItem, Task> OnSuccess { get; set; }
        //[Parameter] public Func<TItem, Task> oncha { get; set; }
        private void OnChange2(object x)
        {
            OnChange?.Invoke(x);
        }

        private async Task SaveModal(MouseEventArgs obj)
        {
            OnSuccess?.Invoke(value);
            await ModalService.Hide();
            //throw new NotImplementedException();
        }

        private void HideModal(MouseEventArgs obj)
        {
        
            //throw new NotImplementedException();
        }
    }
}