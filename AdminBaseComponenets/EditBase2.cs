using Microsoft.AspNetCore.Components;
using System.Linq;
using System;
using Tools;
using System.Reflection;
using System.Linq.Expressions;

namespace AdminBaseComponenets
{
    public class EditBase2<T> : ValueInput<T> 
    {
        public RenderFragment autoComp<T2>(Expression<Func<T2>> action, bool propertyReadOnly = false)
        {


            try
            {
                var expression = (MemberExpression)action.Body;
                var property = expression.Member as PropertyInfo;
                return createProp(property, propertyReadOnly);
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
        public ValueInput0 createProp0(PropertyInfo property, bool propertyReadOnly = false)
        {

            return  Program0.createForm2<T>(property);
            
        }
        public RenderFragment showProp(PropertyInfo property,ComponentBase w, bool propertyReadOnly = false)
        {

            
            if (w is null)
                return null;


            Action<object> onChange = (x) =>
            {
                Console.WriteLine($"onChange property {property.Name} : {x} ");
                object vv2 = value;//this step neserrcry for struct and valuetype data
                property.SetValue(vv2, x, null);
                value = (T)vv2;//
                Console.WriteLine($"onChange value : {value} ");
                Console.WriteLine($"onChange vv2 : {vv2} ");
                if (OnChange != null)
                    OnChange(value);
            };
            object prVal = null;
            try
            {
                prVal = property.GetValue(value);
            }
            catch
            {

            }
            if (w.GetType().IsGenericInstanceOf(typeof(AdminBaseComponenets.BaseComs.ForeignKeyEdite<,>))

                && !property.PropertyType.IsGenericInstanceOf(typeof(ForeignKey2<,>))

                )
            {
                onChange = (x) =>
                {
                    property.SetValue(value, ((IForeignKey20)x).getFValue0());
                    if (OnChange != null)
                        OnChange(value);
                };


                var types=w.GetType().GetGenericArguments();
                //var a = property.GetCustomFirstAttributes<ForeignKeyAttr>();

                //if (a != null)
                {
                    Console.WriteLine("formRenderer[typeof(int)] ");
                    //var ct = a.type;
                    //Console.WriteLine($"formRenderer[typeof(int)] ForeignKey<{ct}>");
                    prVal = typeof(ForeignKey2<,>).MakeGenericType(types).GetConstructor(new Type[] { types[1] }).Invoke(new object[] { prVal });
                    //var gtc = gt.GetConstructor(new[] { typeof(int) });



                }


            }
            if(false)foreach(var keyT in new Type[] { typeof(string), typeof(String), typeof(Guid), typeof(int) })
            if (property.PropertyType == typeof(string)
                && w.GetType().IsGenericInstanceOf(typeof(AdminBaseComponenets.BaseComs.ForeignKeyEdite<,>)))
            {
                onChange = (x) =>
                {
                    property.SetValue(value, ((IForeignKey20)x).getFValue0());
                    if (OnChange != null)
                        OnChange(value);
                };



                var a = property.GetCustomFirstAttributes<ForeignKeyAttr>();

                //if (a != null)
                {
                    Console.WriteLine("formRenderer[typeof(int)] ");

                    Console.WriteLine($"formRenderer[typeof(int)] ForeignKey<{a.type}>");
                    prVal = typeof(ForeignKey2<,>).MakeGenericType(a.getTypes()).GetConstructor(new Type[] { keyT }).Invoke(new object[] { prVal });
                    //var gtc = gt.GetConstructor(new[] { typeof(int) });



                }


            }
            if (property.PropertyType == typeof(int)
                && w.GetType().IsGenericInstanceOf(typeof(AdminBaseComponenets.BaseComs.ForeignKeyEditeInt<>)))
            {
                onChange = (x) =>
                {
                    property.SetValue(value, ((IForeignKey20)x).getFValue0());
                    if (OnChange != null)
                        OnChange(value);
                };



                var types0 = w.GetType().GetGenericArguments().ToList();
                types0.Add(typeof(int));
                var types = types0.ToArray();
                //if (a != null)
                {
                    Console.WriteLine("formRenderer[typeof(int)] ");

                    Console.WriteLine($"formRenderer[typeof(int)] ForeignKey<{types[0]}>");
                    prVal = typeof(ForeignKey2<,>).MakeGenericType(types).GetConstructor(new Type[] { types[1] }).Invoke(new object[] { prVal });
                    //var gtc = gt.GetConstructor(new[] { typeof(int) });



                }


            }


            if (property.PropertyType.IsGenericType 
                 && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) 
                 && w.GetType().IsGenericType
                 && w.GetType().GetGenericTypeDefinition() == typeof(AdminBaseComponenets.BaseComs.NullabeType<>)
                 //&& w.GetType().GetGenericArguments()[0].IsGenericType
                 && w.GetType().GetGenericArguments()[0].IsAssignableTo(typeof(IForeignKey2<>).MakeGenericType(property.PropertyType.GenericTypeArguments[0])))
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



            propertyReadOnly |= attrs != null && attrs.Any(a => (a is System.ComponentModel.ReadOnlyAttribute) && (a as System.ComponentModel.ReadOnlyAttribute).IsReadOnly);
            if (setMethod == null)
            {
                propertyReadOnly = true;
            }

            Console.WriteLine($" go to create {w} {prVal}");
            return Program0.CreateDynamicComponent2(this, w, prVal, onChange, attrs, ReadOnly || propertyReadOnly);

        }

        public RenderFragment createProp(PropertyInfo property,bool propertyReadOnly = false)
        {
            return showProp(property, createProp0(property, propertyReadOnly), propertyReadOnly);
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


 


}
