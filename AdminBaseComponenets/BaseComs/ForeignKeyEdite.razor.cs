using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Tools;
using Models;
using Blazorise;

namespace AdminBaseComponenets.BaseComs
{


    public partial class ForeignKeyEdite<TEntity,TKEY> : ForeignKeyEditeBase<TEntity,TKEY>
         where TEntity : class, Models.IIdMapper<TKEY>
         where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        
        bool panelOpenState;


        Type[] genericArgs = new Type[] { null, typeof(int) };







        Type gridMetaClass = null;

        private Modal modalRef;

        bool showModal = false;


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
                val = await tmp.get(value);

            if (optionGenerator == null)
            {
                if (typeof(TEntity).GetCustomFirstAttributes<BigTable>() != null)
                {
                    var l = new List<ForeignKey2<TEntity, TKEY>>();
                    if(val != null)
                        l.Add(new ForeignKey2<TEntity, TKEY>(value));
                    optionGenerator = l;
                }
                

                
                await tmp.getAll();
                try
                {
                    await tmp.get(value.Value);
                }
                catch
                {
                    
                }

                optionGenerator = tmp;//.ToList().ConvertAll(x => new ForeignKey2<TEntity,TKEY>(x.id)); 
            }
            
            // StateHasChanged();



        }
        public void OnButtonClicked()
        {
            NavManager.NavigateTo($"{typeof(TEntity).GetUrlEncodeName()}/edit/{value.Value}");
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