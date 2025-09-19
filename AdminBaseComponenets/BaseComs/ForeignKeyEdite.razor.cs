using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Tools;
using Models;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AdminBaseComponenets.BaseComs
{


    public partial class ForeignKeyEdite<TEntity,TKEY> : ForeignKeyEditeBase<TEntity,TKEY>
         where TEntity : class, Models.IIdMapper<TKEY>
         where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        
        bool panelOpenState;


        Type[] genericArgs = new Type[] { null, typeof(int) };


        private TEntity curentV=null;




        Type gridMetaClass = null;

        private Modal modalRef;

        bool showModal = false;
        [Inject] public IModalService ModalService { get; set; }
        [Inject] private IJSRuntime jsRuntime { get; set; }

        void ModalCancel() => showModal = false;
        void ModalOk()
        {
            Console.WriteLine("Modal ok");
            showModal = false;
            
        }
        protected override async Task OnInitializedAsync()
        {
            itemComponenet = Program0.createWidget(typeof(ForeignKey2<TEntity, TKEY>), new List<Attribute>());
            await load();
        }


        protected async Task Click2(TKEY vs)
        {
            showModal = false;
            Console.WriteLine($"onChange integerFUnput  {vs} ");
            value=new ForeignKey2<TEntity, TKEY>(vs);
            Console.WriteLine($"onChange integerFUnput  {value.getFValue()} ");
            var bv = fValue;
            var vv = await Click();
            if (bv != null && bv.Equals(vv))
                return;
            if (vv == null)
                return;
            //viewComponenet= new AdminBaseComponenets.BaseComs.IntegerFSmallView<Models.Coach>() { };
            //viewComponenet = Program0.createWidget(vv.GetType(),  new List<Attribute>() { new ForeignKeyAttr(typeof(TEntity)) });
            StateHasChanged();

        }
        protected async Task Click3()
        {
            Click2(value.getFValue());

        }
        public async Task load()
        {
            TEntity val = default(TEntity);
            var tmp = Program0.getEntityManager<TEntity, TKEY>();
            if (!value.Equals(default(TKEY)))
                curentV=val = await tmp.get(value);

            if (optionGenerator == null)
            {
                if (!ReadOnly)
                {
                    await tmp.getAll();
                    if (typeof(TEntity).GetCustomFirstAttributes<BigTable>() != null)
                    {
                        var l = new List<ForeignKey2<TEntity, TKEY>>();
                        if (val != null)
                            l.Add(new ForeignKey2<TEntity, TKEY>(value));
                        optionGenerator = l;

                    }

                    //else
                    {

                        try
                        {
                            tmp.get(value.Value);
                        }
                        catch
                        {

                        }

                        optionGenerator = tmp; //.ToList().ConvertAll(x => new ForeignKey2<TEntity,TKEY>(x.id));
                    }
                }
                curentV=await tmp.get(value.getFValue());

            }
            
            // StateHasChanged();



        }
        public async Task OnButtonClicked(MouseEventArgs args)
        {
            
            
            
            
            if (args.CtrlKey)
            {
                
                string url = $"{typeof(TEntity).GetUrlEncodeName()}/edit/{value.Value}";

                jsRuntime.InvokeAsync<object>("open", url, "_blank");
            }
            else if (args.AltKey)
            {
                ShowModal();
            }
            else
            {
                NavManager.NavigateTo($"{typeof(TEntity).GetUrlEncodeName()}/edit/{value.Value}");
            }
            
            
        }
        private Task SaveModal(TEntity x)
        {
            
            return InvokeAsync(StateHasChanged);
            //return modalRef.Hide();
        }
        private Task ShowModal()
        {
            //newItem = new TItem();
            //addingItem = typeof(TItem).GetConstructor(new Type[] { }).Invoke(new object[] { }) as TItem;// new genericArgs[0]();
            
                
            Console.WriteLine($"value {curentV!=null}");
            return ModalService.Show<PopupForm<TEntity>>( x =>
                {
                    //x.Add( x => x.OnValidate, FormularyValidate );
                    x.Add( x => x.OnSuccess ,SaveModal);
                    //x.Add(x=> x.OnChange,OnChange2);
                    x.Add(x=>x.value,curentV);
                    x.Add(x=>x.ReadOnly,true);
                },
                new ModalInstanceOptions()
                {
                    UseModalStructure = false,
                    Style = "overflow:visible",
                    Size = ModalSize.ExtraLarge,
                    Scrollable = true
                
                    //Stateful = true
                } );
        }


        public void OnSearchClick()
        {
            showModal = true;
            
        }

    }


    public class ForeignKeyEditeInt<TEntity> : ForeignKeyEdite<TEntity, int>
          where TEntity : class, Models.IIdMapper<int>
         
    { }
}