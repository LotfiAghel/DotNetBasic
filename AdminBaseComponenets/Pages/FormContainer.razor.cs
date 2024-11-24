using System.Reflection;
using AdminBaseComponenets;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq;
using AdminBaseComponenets.BaseComs;
using Tools;
using AdminClientViewModels;

namespace AdminBaseComponenets.Pages
{
    public partial class FormContainer : ValueInput0
    {
        [Parameter]
        public string entityName { get; set; }

        [Parameter]
        public string Id { get; set; }




      
        public static async Task<object> getValue<T, TKEY>(string Id)
                          where T : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable

        {
            if (typeof(TKEY) == typeof(int))
                return await Program0.getEntityManager<T, TKEY>().get01(int.Parse(Id));
            if (typeof(TKEY) == typeof(Guid))
                return await Program0.getEntityManager<T, TKEY>().get01(Guid.Parse(Id));
            var t = Program0.getEntityManager<T, TKEY>();
            if (t == null)
                return null;
            return await t.get01(Id);
        }
        public static T getValueFast0<T, TKEY>(string Id)
             where T : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
        {
            return Program0.getEntityManager<T, TKEY>().getFast(Id);
        }

        public static T getValueFast<T,TKEY>(string Id)
             where T : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
        {
            var tid = IEntityService00.ConvertS<TKEY>(Id);


            var z = Task.Run(async () => await Program0.getEntityManager<T, TKEY>().get(tid));
            z.Wait();
            return z.Result;
        }

        public static async Task<object> postValue<TItem>(TItem Id)
        {
            return await Program0.getEntityManager01<TItem>().post(Id);
        }

        public static async Task<object> updateValue<TItem>(TItem Id)
        {
            return await Program0.getEntityManager01<TItem>().update(Id);
        }
        //<DataList TItem2=@Coach TItem=@Coach url="/old/" url2=@entityName >
        RenderFragment CreateDynamicComponentGrid(Type entityMetaClass0, object masterId, string propName) => builder =>
        {
            try { 
                var gridMetaClass = typeof(SubTable<,,,>).MakeGenericType(new Type[] { entityMetaClass0, Program0.getKeyType(entityMetaClass0),masterId.GetType(), genericArgs[0] });
                builder.OpenComponent(0, gridMetaClass);


            
                builder.AddAttribute(1, "collectionName", propName);
                builder.AddAttribute(1, "masterEnityId", masterId);


                //builder.AddAttribute(1, "itemName", "classId");
                //builder.AddAttribute(1, "itemValue", Int32.Parse(Id));
                builder.CloseComponent();
            }catch{ 
            }
        };






        Type[] genericArgs = new Type[] { null, typeof(int) };

        string selectedTab = "value";

        private Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            return Task.CompletedTask;
        }


        object actionValue = null;

      

        private bool valueIsNew = false;
        
        private ComponentBase formView = null;

        Action<object> onChangeRefrence0 = null;
        protected override void OnInitialized()
        {

            base.OnInitialized();
            genericArgs = Program0.getValueKeyPair(entityName);
            if (value0 == null || !value0.GetType().IsInstanceOfType(genericArgs[0]))
            {
                Console.WriteLine("go to load value of form");
                if (Id == "new")
                {
                    value0 = genericArgs[0].GetConstructor(new Type[] { }).Invoke(new object[] { });// new genericArgs[0]();
                    valueIsNew = true;
                    if (ButtonState.Length < 3)
                        ButtonState = "save0";
                }
                else
                {
                    value0 = (typeof(FormContainer).GetMethod(nameof(getValueFast0), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?
                        .MakeGenericMethod(genericArgs)
                      .Invoke(this, new object[] { Id }));
                    //Console.WriteLine($"get value {value.GetType().Name}");
                    StateHasChanged();
                }

            }
        }

        public void Initial()
        {
            
            genericArgs = Program0.getValueKeyPair(entityName);
                
            if (genericArgs[0].IsAbstract)
            {
                formView = (ComponentBase)(typeof(AdminBaseComponenets.BaseComs.PointerInput2<>).MakeGenericType(new Type[] { genericArgs[0] }).GetConstructor(new Type[] { }).Invoke(new object[] { }));//new PointerInput2<genericArgs[0]>()
                onChangeRefrence0 = (x) =>
                {

                    value0 = x;

                };

            }else
                formView = Program0.createForm(genericArgs[0], new List<Attribute>() { });        
        }
        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
            genericArgs = Program0.getValueKeyPair(entityName);

            Initial();
            
            ReadOnly = !Program0.checkPermission<Models.UpdateAccess>(genericArgs[0]);

            

            //formView=(ComponentBase)(typeof(GenericForm<>).MakeGenericType(new Type[] {  genericArgs[0] }).GetConstructor(new Type[]{}).Invoke(new object[]{}));//new GenericForm<genericArgs[0]>()




            //await Program0.CheckLogin();

            if (value0 == null || !value0.GetType().IsInstanceOfType(genericArgs[0]))
            {
                Console.WriteLine("go to load value of form");
                Console.WriteLine($"{genericArgs[0].Name}");
                Console.WriteLine($"{genericArgs[1].Name}");
                if (Id == "new")
                {
                    value0 = genericArgs[0].GetConstructor(new Type[] { }).Invoke(new object[] { });// new genericArgs[0]();
                    valueIsNew = true;
                    if (ButtonState.Length < 3)
                        ButtonState = "save0";
                }
                else
                {
                    value0 = await (Task<object>)(typeof(FormContainer).GetMethod(nameof(getValue), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?
                        .MakeGenericMethod(genericArgs)
                      .Invoke(this, new object[] { Id }));
                    Console.WriteLine($"get value {value0.GetType().Name}");
                    StateHasChanged();
                }

            }
            Console.WriteLine("value " + value0.GetType().Name);
            if (value0 is Models.Entity en)
            {

                if (en.onChanges == null)
                    en.onChanges = new Models.ChangeEventList();
                en.onChanges.Connect(this, () =>
                {
                    StateHasChanged();
                    //NavigationManager.NavigateTo($"/{entityName}/edit/{en.id}", false);
                });
            }
        }

        string ButtonState = "save";
        public async Task Click()
        {
            try
            {
                
                ButtonState = "sending";
                //StateHasChanged();
                await save();
                ButtonState = "update";
                //StateHasChanged();

                valueIsNew = false;

                var pr = genericArgs[0].GetProperty(nameof(Models.Id4Entity.id));
                var id = pr.GetValue(value0);
                
                NavigationManager.NavigateTo($"/{entityName}/edit/{value0}", false);



            }
            catch
            {
                Console.WriteLine("FormC exception ");
                ButtonState = "cant save try again";
                //StateHasChanged();
                return;
            }


        }



        public async Task update()
        {
            ButtonState = "sending";
            //Data = await Http.GetFromJsonAsync<List<TItem>>(Config.serverUrl+url+url2);
            try
            {
                await save2();
                ButtonState = "save";

            }
            catch
            {
                ButtonState = "cant update try again";
            }
            //StateHasChanged();

        }
        public async Task save()
        {

            genericArgs[0] = Program0.resources.FirstOrDefault(x => x.GetUrlEncodeName() == entityName);

            value0 = await (Task<object>)(typeof(FormContainer).GetMethod(nameof(postValue), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?.MakeGenericMethod(genericArgs[0])
                .InvokeAsync(this, new object[] { value0 })); // await EditBaseContainer.postValue()

            StateHasChanged();
        }
        public async Task save2()
        {

            genericArgs[0] = Program0.resources.FirstOrDefault(x => x.GetUrlEncodeName() == entityName);

            value0 = await (Task<object>)(typeof(FormContainer).GetMethod(nameof(updateValue), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?.MakeGenericMethod(genericArgs[0])
                .InvokeAsync(this, new object[] { value0 }));// await EditBaseContainer.updateValue()

            StateHasChanged();
        }



    }
}