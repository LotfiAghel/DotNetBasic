using System.Reflection;
using AdminBaseComponenets;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using System.Linq;
using AdminBaseComponenets.BaseComs;
using Tools;


namespace AdminBaseComponenets.Pages
{
    public partial class FormContainer : ValueInput0
    {
        [Parameter]
        public string entityName { get; set; }

        [Parameter]
        public string Id { get; set; }




        [Parameter]
        public object value { get; set; }

        public static async Task<object> getValue<T, TKEY>(string Id)
                          where T : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable

        {
            if (typeof(TKEY) == typeof(int))
                return await Program0.getEntityManager<T, TKEY>().get01(int.Parse(Id));
            if (typeof(TKEY) == typeof(Guid))
                return await Program0.getEntityManager<T, TKEY>().get01(Guid.Parse(Id));
            return await Program0.getEntityManager<T, TKEY>().get01(Id);
        }

        public static object getValueFast<TItem>(string Id)
        {
            return Program0.getEntityManager01<TItem>().getFast(Id);
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
        public bool ReadOnly = false;
        private ComponentBase formView = null;

        Action<object> onChangeRefrence0 = null;
        protected override async Task OnInitializedAsync()
        {

            base.OnInitialized();
            genericArgs = Program0.getValueKeyPair(entityName);


            if (genericArgs[0].IsAbstract)
            {
                formView = (ComponentBase)(typeof(AdminBaseComponenets.BaseComs.PointerInput2<>).MakeGenericType(new Type[] { genericArgs[0] }).GetConstructor(new Type[] { }).Invoke(new object[] { }));//new PointerInput2<genericArgs[0]>()
                onChangeRefrence0 = (x) =>
                {

                    value = x;

                };

                return;
            }
            ReadOnly = !Program0.checkPermission<Models.UpdateAccess>(genericArgs[0]);



            //formView=(ComponentBase)(typeof(GenericForm<>).MakeGenericType(new Type[] {  genericArgs[0] }).GetConstructor(new Type[]{}).Invoke(new object[]{}));//new GenericForm<genericArgs[0]>()




            //await Program0.CheckLogin();

            if (value == null)
            {
                Console.WriteLine("go to load value of form");
                Console.WriteLine($"{genericArgs[0].Name}");
                Console.WriteLine($"{genericArgs[1].Name}");
                value = await (Task<object>)(typeof(FormContainer).GetMethod(nameof(getValue), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?
                    .MakeGenericMethod(genericArgs)
                  .Invoke(this, new object[] { Id }));
                Console.WriteLine($"get value {value.GetType().Name}");

            }
            Console.WriteLine("value " + value.GetType().Name);
            if (value is Models.Entity en)
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

        string ButtonState = "--";
        public async Task Click()
        {
            try
            {
                Console.WriteLine("FormC Click ");
                ButtonState = "sending";
                //StateHasChanged();
                await save();
                ButtonState = "update";
                //StateHasChanged();

                valueIsNew = false;

                var pr = genericArgs[0].GetProperty("id");
                var id = pr.GetValue(value);
                Console.WriteLine("FormC navigate ");
                NavigationManager.NavigateTo($"/{entityName}/edit/{id}", false);



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

            value = await (Task<object>)(typeof(FormContainer).GetMethod(nameof(postValue), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?.MakeGenericMethod(genericArgs[0])
                .InvokeAsync(this, new object[] { value })); // await EditBaseContainer.postValue()

            StateHasChanged();
        }
        public async Task save2()
        {

            genericArgs[0] = Program0.resources.FirstOrDefault(x => x.GetUrlEncodeName() == entityName);

            value = await (Task<object>)(typeof(FormContainer).GetMethod(nameof(updateValue), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?.MakeGenericMethod(genericArgs[0])
                .InvokeAsync(this, new object[] { value }));// await EditBaseContainer.updateValue()

            StateHasChanged();
        }



    }
}