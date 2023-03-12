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
using System.Linq.Expressions;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Components;
using Models;
using Tools;

namespace AdminBaseComponenets
{

    public class PointerInputBase<T> : ComponentBase
    {





        [Parameter]
        public object valueContainer { get; set; }



        [Parameter]
        public string itemName { get; set; }



        public string typeName { get; set; } = null;



        public Type classType { get; set; }


        public List<Type> classess = new List<Type>();
        public void PopulateFeature()
        {
            var assms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var currentAssembly in assms)
            {
                try
                {
                    //var currentAssembly = typeof(GenericTypeControllerFeatureProvider).Assembly;
                    var candidates = currentAssembly.GetExportedTypes().Where(x => x.IsAssignableTo(typeof(T)));

                    foreach (var candidate in candidates)
                        classess.Add(candidate);
                }
                catch
                {

                }


            }
        }

        public PointerInputBase()
        {
            PopulateFeature();


        }
        protected override async Task OnInitializedAsync()
        {
            var ct = valueContainer.GetType();
            var vp = ct.GetProperty(itemName);
            var value = vp.GetValue(valueContainer);
            if (typeName == null && value != null)
            {
                typeName = value.GetType().Name;
            }
        }

        public async Task<object> Click()
        {
            Console.WriteLine("typeName = " + typeName);
            var ct = valueContainer.GetType();
            var vp = ct.GetProperty(itemName);
            var value = vp.GetValue(valueContainer);
            if (value != null)
            {
                Console.WriteLine("value= " + JToken.FromObject(value));
                Console.WriteLine("value= " + value.GetHashCode());
            }
            else
                Console.WriteLine("value= null");
            classType = classess.First(x => x.Name == typeName);//ClassType=Class
            if (classType != value.GetType())
            {
                value = classType.GetConstructor(new Type[] { }).Invoke(new object[] { });//tmp=new ClassType()
                vp.SetValue(valueContainer, value);//this.value=tmp;
            }
            Console.WriteLine(JToken.FromObject(value));
            return value;

        }
    }


    
    public class ValueInput0 : ComponentBase
    {
        [Parameter]
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();

        [Parameter]
        public bool ReadOnly { get; set; }

        //[Parameter]
        //public EventCallback<object> changeRefrence { get; set; }

        [Parameter]
        public Action<object> OnChange { get; set; } = null;

        [Parameter]
        public virtual object value0
        {
            get;set;
        }

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

    public class PointerInputBase2<T> : NullableInput2<T>
    {


        public string typeName { get; set; } = null;



        public List<Type> classess = new List<Type>();

        public void PopulateFeature()
        {
            classess = typeof(T).GetSubClasses();
        }

        public PointerInputBase2()
        {
            PopulateFeature();


        }


        public async Task<T> Click()
        {

            Console.WriteLine("typeName = " + typeName);

            Console.WriteLine("value= " + (value == null ? "null" : value.GetType().Name));

            var classType = classess.FirstOrDefault(x => x.Name == typeName);//ClassType=Class

            Console.WriteLine("classType=" + (classType == null ? "null" : classType.Name));
            if (classType == null)
            {
                value = default(T);

                return value;
            }
            if (value == null || classType != value.GetType())
            {

                var vb = value;
                if (Program0.defultCunstroctor.ContainsKey(classType))
                {
                    value = (T)Program0.defultCunstroctor[classType]();
                }
                else
                {
                    var ct = classType.GetConstructor(new Type[] { });
                    value = (T)ct.Invoke(new object[] { });//tmp=new ClassType()
                    Console.WriteLine("value= new ClassType()");
                }

                if (vb != null)
                {

                    try
                    {
                        var vj = JToken.FromObject(vb);
                        Console.WriteLine("vb =" + vj);
                        value = (T)(vj.ToObject(classType));
                        Console.WriteLine("value =" + JToken.FromObject(value));
                    }
                    catch
                    {

                    }

                }


            }

            return value;

        }
    }


    public class ForeignKeyEditeBase<CT, TKEY> : ValueInput<IForeignKey11<CT,TKEY>>
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
        public MarkedGenerator<CT> parnet;
        public int idx;

        CT IEnumerator<CT>.Current => parnet.enumList[idx];

        object System.Collections.IEnumerator.Current => parnet.enumList[idx];

        public bool MoveNext()
        {
            idx++;
            while (idx < parnet.enumList.Count && parnet.mark.Contains(parnet.enumList[idx]))
            {
                idx++;
            }
            return idx < parnet.enumList.Count;
        }
        public void Reset()
        {
            idx = 0;
            while (idx < parnet.enumList.Count && parnet.mark.Contains(parnet.enumList[idx]))
            {
                idx++;
            }
        }
        public void Dispose()
        {

        }

    }
    public class MarkedGenerator<CT> : IEnumerable<CT>
    {
        public HashSet<CT> mark = new HashSet<CT>();
        public List<CT> enumList =null;

       
        public IEnumerator<CT> GetEnumerator()
        {
            var x= new Enumerator2<CT>(){parnet=this};
            x.Reset();
            return x;
        }
        public void Clear()
        {
            mark.Clear();
        }
        public void initList(IEnumerable<CT> enumValues) {


            // I will get0s all values and iterate through them

            if (enumList == null) {
                enumList = new List<CT>();
            }
            enumList.Clear();
            //for (int i=0; i<enumValues.Length; ++i)
            foreach (CT v in enumValues)
                enumList.Add(v);
        }
        public void load(IEnumerable<CT> value)
        {
            if(enumList==null){
                var z=Enum.GetValues(typeof(CT)).Cast<CT>();
                initList(z);
            }

            if(value!=null)foreach (var item in value)
                mark.Add(item);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator2<CT>() { parnet = this };
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
    public class EnumArrayInput<CT> : NullableInput2<List<CT>>
    {


        [Parameter]
        public MarkedGenerator<CT> generator {get; set; }=null;


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
    public class DisplayInSelectBaseInt<T> : ComponentBase
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

    
    public class EditBase2<T> : ValueInput<T> 
    {
        public RenderFragment autoComp<T2>(Expression<Func<T2>> action)
        {


            try
            {
                var expression = (MemberExpression)action.Body;
                var property = expression.Member as PropertyInfo;
                return createProp(property);
            }
            catch (Exception e)
            {

            }


            try
            {
                var expression = (MemberExpression)action.Body;
                var property = expression.Member as MethodInfo;
                return createMethod(property);
            }
            catch (Exception e)
            {

            }
            return null;




        }
        public RenderFragment createProp(PropertyInfo property)
        {

            ComponentBase w = Program0.createForm2(property);
            if (w is null)
                return null;

            /*Action<T> onChange0 = null;
            if (typeof(T).IsValueType)
            {
                onChange0 = (x) =>
                {
                    Console.WriteLine($"onChange00 property {property.Name} : {x} ");
                    if (OnChange != null)
                        OnChange(this);

                };
            }/**/


            Action<object> onChange = (x) =>
            {
                Console.WriteLine($"onChange property {property.Name} : {x} ");
                object vv2 = value;//this step neserrcry for struct and valuetype data
                property.SetValue(vv2, x,null);
                value = (T)vv2;//
                Console.WriteLine($"onChange value : {value} ");
                Console.WriteLine($"onChange vv2 : {vv2} ");
                if (OnChange != null)
                    OnChange(value);
            };
            var prVal = property.GetValue(value);
            if (w.GetType().IsGenericInstanceOf(typeof(AdminBaseComponenets.BaseComs.ForeignKeyEdite<,>))
                    
                && ! property.PropertyType.IsGenericInstanceOf(typeof(ForeignKey2<,>)) 
                    
                )
            {
                onChange = (x) =>
                {
                    property.SetValue(value, ((IForeignKey20)x).getFValue0());
                    if (OnChange != null)
                        OnChange(value);
                };



                var a = property.GetCustomFirstAttributes<ForeignKeyAttr>();

                if (a != null)
                {
                    Console.WriteLine("formRenderer[typeof(int)] ");
                    var ct = a.type;
                    Console.WriteLine($"formRenderer[typeof(int)] ForeignKey<{ct}>");
                    prVal = typeof(ForeignKey2<,>).MakeGenericType(a.getTypes()).GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { prVal });
                    //var gtc = gt.GetConstructor(new[] { typeof(int) });



                }


            }
            if (property.PropertyType == typeof(int)
                && w.GetType().IsGenericInstanceOf ( typeof(AdminBaseComponenets.BaseComs.ForeignKeyEditeInt<>)) )
            {
                onChange = (x) =>
                {
                    property.SetValue(value, ((IForeignKey20)x).getFValue0());
                    if (OnChange != null)
                        OnChange(value);
                };



                var a = property.GetCustomFirstAttributes<ForeignKeyAttr>();

                if (a != null)
                {
                    Console.WriteLine("formRenderer[typeof(int)] ");
                        
                    Console.WriteLine($"formRenderer[typeof(int)] ForeignKey<{a.type}>");
                    prVal = typeof(ForeignKey2<,>).MakeGenericType(a.getTypes()).GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { prVal });
                    //var gtc = gt.GetConstructor(new[] { typeof(int) });



                }


            }

            if (property.PropertyType == typeof(Nullable<int>) && w.GetType().IsGenericType
                    && w.GetType().GetGenericTypeDefinition() == typeof(AdminBaseComponenets.BaseComs.NullabeType<>)
                    && w.GetType().GetGenericArguments()[0].IsGenericType
                    && w.GetType().GetGenericArguments()[0].GetGenericTypeDefinition() == typeof(ForeignKey<>))
            {
                onChange = (x) =>
                {
                    if (x == null)
                    {
                        property.SetValue(value, null);
                        return;
                    }
                    property.SetValue(value, ((IForeignKey20)x).getFValue0());
                    if (OnChange != null)
                        OnChange(value);

                };
            }



            var attrs = property.GetCustomAttributes(typeof(object), false).ToList().ConvertAll<Attribute>(x => x as Attribute);
            var setMethod = property.GetSetMethod();



            bool propertyReadOnly = attrs != null && attrs.Any(a => (a is System.ComponentModel.ReadOnlyAttribute) && (a as System.ComponentModel.ReadOnlyAttribute).IsReadOnly);
            if (setMethod == null)
            {
                propertyReadOnly = true;
            }

            Console.WriteLine($" go to create {w} {prVal}");
            return Program0.CreateDynamicComponent2(this, w, prVal, onChange, attrs, ReadOnly || propertyReadOnly);

        }

        public RenderFragment createMethod(MethodInfo property)
        {
            return null;
            /*ComponentBase w = Program.createForm2(property);


            if (w != null)
            {
                var x = property.GetCustomFirstAttributes<Models.PersianLabel>();


                Action<object> onClick = (x) =>
                {
                    property.Invoke(value);
                };





                var attrs = property.GetCustomAttributes(typeof(object), false).ToList().ConvertAll<Attribute>(x => x as Attribute);




                //bool propertyReadOnly = attrs!=null && attrs.Any(a => (a is System.ComponentModel.ReadOnlyAttribute)&& (a as System.ComponentModel.ReadOnlyAttribute).IsReadOnly);
                // TODO check this


                return Program0.CreateDynamicComponent2(this, w, property.GetValue(value), onClick, attrs,ReadOnly||propertyReadOnly);

            }
            return null;/**/
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
