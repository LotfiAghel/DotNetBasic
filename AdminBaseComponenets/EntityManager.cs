using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using ClTool;
using System;
using Microsoft.AspNetCore.Components.Rendering;
using Newtonsoft.Json.Linq;
using Tools;
using System.Reflection;
using System.Reflection.Emit;
using Models;
using System.Collections.ObjectModel;

namespace AdminBaseComponenets
{

    
    
    public class ValueInput0 : ComponentBase
    {
        [Parameter]
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();

        [Parameter]
        public bool ReadOnly { get; set; }

        
        [Parameter]
        public Action<object> OnChange { get; set; } = null;

        [Parameter]
        public virtual object value0
        {
            get;set;
        }

        public virtual bool inRowField() => true;

    }
    public class ValueInput<T> : ValueInput0
    {


        [Parameter]
        public override object value0
        {
            get => this.value; set
            {
                try
                {
                    this.value = (T)value;
                }catch(Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(typeof(T));
                    Console.WriteLine(value.GetType());
                }
            }
        }

        [Parameter]
        public T value { get; set; }

        





        public virtual async Task Click()
        {



            //OnChange(value);
            OnChange(value);




        }

    }

    
    public class OptionsValueInput<T> : ValueInput<T>
    {
        [Parameter]
        public IEnumerable<T> optionGenerator { get; set; } = null;


        [Parameter]
        public ComponentBase itemComponenet { get; set; } = null;


    }

    public class NullableInput2<T> : ValueInput0  //where T : struct
    {

        [Parameter]
        public override object value0
        {
            get => this.value; set
            {
                
                
                try
                {
                    this.value = (T)value;
                    __valueIsNull = (value == null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(typeof(T));
                    Console.WriteLine(value.GetType());
                }
            }
        }

        public static NullableInput2<T> create(PropertyInfo prop)
        {
            return null;
        }

        [Parameter]
        public T value { get; set; }

       




        [Parameter]
        public bool __valueIsNull { get; set; } = true;




        public virtual async Task<bool> setNull()
        {
            //value=null;
            //OnChange(null);
            OnChange(null);
            __valueIsNull = true;
            return true;
        }






        public async Task<T> Click()
        {


            var classType = typeof(T);//ClassType=Class

            Console.WriteLine("classType=" + (classType == null ? "null" : classType.Name));

            //OnChange(value);
            OnChange(value);

            __valueIsNull = false;
            return value;

        }
    }

    


    public class ForeignKeyEditeBase<CT, TKEY> : OptionsValueInput<ForeignKey2<CT,TKEY>>
       where CT : class, Models.IIdMapper<TKEY>
         where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {




        public CT fValue { get; set; }

        public static TKEY getOptionValue(CT t)
        {
            if (t == null)
                return default(TKEY);
            return t.id;
        }
        /*public void setFValue0(object t)
       {
          
        //value = (ForeignKey2<CT,TKEY>)t;
        }/**/
        private TKEY pvalue = default(TKEY);
        public override async Task<CT> Click()
        {
            Console.WriteLine($"onChange integerFInput click {pvalue} {value} ");
            if (!pvalue.Equals(value))
            {
                OnChange(value);
                pvalue = value.getFValue();
            }
            fValue = (CT)(await Program0.getEntityManager<CT,TKEY>().get(value.getFValue()));
            return fValue;

        }
    }





    
    public class IntegerForeignKeyArrayInput<CT, TKEY> : EnumArrayInput<ForeignKey2<CT,TKEY>>
        where CT : class, Models.IIdMapper<TKEY>
     where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {


        //public int value0 { get; set; }

        
        


      
       
       

    }



    public class Enumerator2<CT> : IEnumerator<CT>
    {
        private MarkedGenerator<CT> parnet;
        private IEnumerator<CT> idx;

        CT IEnumerator<CT>.Current => idx.Current;

        object System.Collections.IEnumerator.Current => idx.Current;


        public Enumerator2(MarkedGenerator<CT> parnet)
        {
            this.parnet = parnet;
            idx = parnet.enumList.GetEnumerator();
        }

        public bool MoveNext()
        {
            var ph=idx.MoveNext();
            while (ph && parnet.mark.Contains(idx.Current))
            {
                ph=idx.MoveNext();
            }
            return ph;
        }
        public void Reset()
        {
            idx.Reset();
            var ph = true;
            while (ph && parnet.mark.Contains(idx.Current))
            {
                ph = idx.MoveNext();
            }
        }
        public void Dispose()
        {

        }

    }
    public class MarkedGenerator<CT> : IEnumerable<CT>
    {
        public HashSet<CT> mark = new HashSet<CT>();
        public IReadOnlyCollection<CT> enumList =null;
        //public List
       
        public IEnumerator<CT> GetEnumerator()
        {
            var x = new Enumerator2<CT>(this);
            x.Reset();
            return x;
        }
        public void Clear()
        {
            mark.Clear();
        }
        /*public void initList(IEnumerable<CT> enumValues) {


            // I will get0s all values and iterate through them

            if (enumList == null) {
                enumList = new List<CT>();
            }
            enumList.Clear();
            //for (int i=0; i<enumValues.Length; ++i)
            foreach (CT v in enumValues)
                enumList.Add(v);
        }*/
        public void load(IEnumerable<CT> value)
        {
            /*if(enumList==null){
                var z=Enum.GetValues(typeof(CT)).Cast<CT>();
                initList(z);
            }*/

            if(value!=null)foreach (var item in value)
                mark.Add(item);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return enumList.GetEnumerator(); ;// new Enumerator2<CT>() { parnet = this };
        }

        internal void onRemove(CT vs)
        {
            mark.Remove(vs);
        }

        internal void onAdd(CT vs)
        {
            mark.Add(vs);
        }
        public int Count() { return enumList.Count - mark.Count; }
    }
    
    

    public class EnumMakerdGEnerator<ENUM> : MarkedGenerator<ENUM> where ENUM : System.Enum{
        public EnumMakerdGEnerator()
        {
            var enumList = new List<ENUM>();


            var enumValues = typeof(ENUM).GetEnumValues();
            foreach (ENUM v in enumValues)
                enumList.Add(v);
            this.enumList = new ReadOnlyCollection<ENUM>(enumList);


        }
    }

    public class MarkedGenerator2<T, TKEY> : MarkedGenerator<ForeignKey2<T, TKEY>>
        where T : class, Models.IIdMapper<TKEY>
     where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        public MarkedGenerator2()
        {
            enumList = Program0.getEntityManager<T,TKEY>();
            

            //initList(x.ConvertAll(x => new ForeignKey2<T, TKEY>(x.id)));
            
        }
    }
    public class EnumArrayInput<CT> : NullableInput2<List<CT>>
    {


        [Parameter]
        public MarkedGenerator<CT> generator {get; set; }=null;

        [Parameter]
        public ComponentBase itemComponenet { get; set; }


        [Parameter]
        public OptionsValueInput<CT> itemCreateComponenet { get; set; }


        public List<CT> fValue { get; set; }


        public override async Task<bool> setNull()
        {
            var ph = await base.setNull();
            generator.Clear();
            return ph;
        }


        public async Task Click()
        {

            if (value == null)
            {
                value = new List<CT>();
                generator.Clear();
                OnChange(value);

            }


        }

    }
  

    public class ArrayInputBase<T> : NullableInput2<List<T>>
    {







        public T tmp = default(T);
        public void setValue(int idx, object x)
        {
            Console.WriteLine("setFValue0 --------" + idx + "/" + value.Count + " <= " + JToken.FromObject(x));


            value[idx] = (T)x;

        }

    }




    public class PArrayInputBase<T> : NullableInput2<T[]>
    {







        public T tmp = default(T);
        public void setValue(int idx, object x)
        {
            Console.WriteLine("setFValue0 --------" + idx + "/" + value.Length + " <= " + JToken.FromObject(x));


            value[idx] = (T)x;

        }

    }


    public class DictinaryStringKeyBase<T> : NullableInput2<Dictionary<string, T>>
    {







        public T tmp = default(T);
        public void setValue(string idx, object x)
        {
            Console.WriteLine("setFValue0 --------" + idx + "/" + value.Count + " <= " + JToken.FromObject(x));


            value[idx] = (T)x;

        }

    }
    public class DisplayInSelectBase<TKEY, T> : ComponentBase where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable where T : IIdMapper<TKEY>
    {

        [Parameter]
        public T value { get; set; }





        protected bool IsSelected { get; set; }

        [Parameter]
        public EventCallback<bool> OnEntitySelection { get; set; }

        protected async Task CheckBoxChanged(ChangeEventArgs e)
        {
            IsSelected = (bool)e.Value;
            await OnEntitySelection.InvokeAsync(IsSelected);
        }

    }
    public class DisplayInSelectBase2<T> : ComponentBase
    {

        [Parameter]
        public T value { get; set; }





        protected bool IsSelected { get; set; }

        [Parameter]
        public EventCallback<bool> OnEntitySelection { get; set; }

        protected async Task CheckBoxChanged(ChangeEventArgs e)
        {
            IsSelected = (bool)e.Value;
            await OnEntitySelection.InvokeAsync(IsSelected);
        }

    }
    public class DisplayInSelectBaseInt<T> : ValueInput<T>
    {





        /*protected bool IsSelected { get; set; }

        [Parameter]
        public EventCallback<bool> OnEntitySelection { get; set; }

        protected async Task CheckBoxChanged(ChangeEventArgs e)
        {
            IsSelected = (bool)e.Value;
            await OnEntitySelection.InvokeAsync(IsSelected);
        }*/

    }

    public class DisplayBase<TKEY, T> : ComponentBase where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable where T : IdMapper<TKEY>
    {

        [Parameter]
        public T value { get; set; }

        [Parameter]
        public bool showFooter { get; set; }

        protected bool IsSelected { get; set; }

        [Parameter]
        public EventCallback<bool> OnCoachSelection { get; set; }

        protected async Task CheckBoxChanged(ChangeEventArgs e)
        {
            IsSelected = (bool)e.Value;
            await OnCoachSelection.InvokeAsync(IsSelected);
        }
    }

    


    public class EditBase<TKEY, T> : ComponentBase where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable where T : IdMapper<TKEY>
    {
        public T value { get; set; } = null;



        [Parameter]
        public string Id { get; set; }

        protected async override Task OnInitializedAsync()
        {

            if (typeof(TKEY) == typeof(int))
                value = await Program0.getEntityManager<T, TKEY>().get1s(Id);
            
        }
    }

   
    public class ListBase<TKEY, T> : ComponentBase where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable where T : IdMapper<TKEY>
    {

        public IEnumerable<T> value { get; set; }

        public bool showFooter { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {



            value = await Program0.getEntityManager<T, TKEY>().getAll();


        }
    }


 


}
