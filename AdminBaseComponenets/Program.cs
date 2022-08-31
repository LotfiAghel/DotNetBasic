﻿using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Models;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;
using AdminClientViewModels;
using Models;
using Tools;
using Tools;

public static class ExtensionMethods
{
    public static async Task<object> InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
    {
        var task = (Task)@this.Invoke(obj, parameters);
        await task.ConfigureAwait(false);
        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty.GetValue(task);
    }
}
namespace AdminBaseComponenets
{

   
    public class Program0
    {
        public static Dictionary<string, Dictionary<string, object>> parms = new Dictionary<string, Dictionary<string, object>>();
        public static Dictionary<string, object> getRouteParm(string url)
        {
            Console.WriteLine("get0s url data " + url);
            if (!parms.ContainsKey(url))
            {
                Console.WriteLine("create " + url);
                parms[url] = new Dictionary<string, object>();
            }
            return parms[url];
        }
        
        public static List<Type> resources = new List<Type>();
        public static List<Type> docEntity = new List<Type>();
        public static List<object> aa = new List<object>();
        
        

        public static Dictionary<Type, Func<object>> defultCunstroctor = new Dictionary<Type, Func<object>>();
        public static Dictionary<Type, Func<List<Attribute>, ComponentBase>> defultRenderer = new Dictionary<Type, Func<List<Attribute>, ComponentBase>>();
        public static Dictionary<Type, Func<Type, List<Attribute>, Type>> defultRenderer2 = new Dictionary<Type, Func<Type, List<Attribute>, Type>>();


        public static Dictionary<Type, Func<List<Attribute>, ComponentBase>> formRenderer = new Dictionary<Type, Func<List<Attribute>, ComponentBase>>();


        public static Dictionary<Type,bool> inPropRender = new Dictionary<Type, bool>();
        public static Dictionary<Type, Func<Type, List<Attribute>, Type>> formRenderer2 = new Dictionary<Type, Func<Type, List<Attribute>, Type>>();


        public enum ViewType
        {
            NONE = 0,
            SMALL_VIEW = 1,
            VIEW = 2,
            FORM = 3,
        }
        /* public static RenderFragment CreateDynamicComponent0(object thiz, Type gt, object vv, Action<object> v = null, List<Attribute> Attributes = null) => builder =>
              {


                  Console.WriteLine(" CreateDynamicComponent0 " + gt.Name);

                  builder.OpenComponent(0, gt);

                  if (v != null)
                  {
                      var callback = EventCallback.Factory.Create<object>(thiz, v);
                      builder.AddAttribute(1, "changeRefrence", callback);
                  }
                  if (v != null)
                  {
                      builder.AddAttribute(1, "Attributes", Attributes);
                  }

                  builder.AddAttribute(1, "value", vv);

                  builder.CloseComponent();
              };
        /**/
      private static Dictionary<Type, IEntityService00> entityManagers = new Dictionary<Type, IEntityService00>();
       public static IEntityService00 getEntityManager0(Type ty)
        {
            if (entityManagers.ContainsKey(ty))
                return entityManagers[ty];
            object b = null;
            var keyTypes = new List<Type>(){ typeof(int), typeof(string), typeof(System.Guid) };
            foreach (var KeyType in keyTypes )
                if (ty.IsAssignableTo(typeof(IdMapper<>).MakeGenericType(KeyType)))
                    b = typeof(NewEntityService<,>).MakeGenericType(new Type[] { ty, KeyType }).GetConstructor(new Type[] { }).Invoke(new object[] { });

            if (b != null)
                entityManagers[ty] = b as IEntityService00;

            return b as IEntityService00;
        }


        public static IEntityService01<T> getEntityManager01<T>()
        {
            return getEntityManager0(typeof(T)) as IEntityService01<T>;
        }
        public static IEntityService<T,TKEY> getEntityManager<T, TKEY>()
             where T : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
        {
            return getEntityManager0(typeof(T)) as IEntityService<T, TKEY>;
        }
        public static RenderFragment CreateDynamicComponent2(object thiz, ComponentBase c, object vv, Action<object> changeRefrence = null, List<Attribute> Attributes = null, bool ReadOnly = false)
        {
            try
            {
                return CreateDynamicComponent20(thiz, c as ValueInput0, vv, changeRefrence, Attributes, ReadOnly);
            }
            catch(Exception e)
            {
                Console.WriteLine( e.StackTrace);
                throw e;
            }
            
        }
        public static RenderFragment CreateDynamicComponent20(object thiz, ValueInput0 c, object vv, Action<object> changeRefrence = null, List<Attribute> Attributes = null, bool ReadOnly = false) => builder =>
              {


                  var gt = c.GetType();



                  builder.OpenComponent(0, gt);
                  foreach (var gti in gt.GetProperties())
                  {
                      
                      var pr = gti.GetCustomFirstAttributes<ParameterAttribute>();
                      if (pr != null)
                      {
                          var z=gti.GetValue(c);
                          if(z!=null) // if remove this line you have strange exception
                            builder.AddAttribute(1, gti.Name, gti.GetValue(c));

                      }/**/
                  }
                  
                  if (changeRefrence != null)
                  {
                      var callback = EventCallback.Factory.Create<object>(thiz, changeRefrence);
                      builder.AddAttribute(1, "changeRefrence", callback);
                  }
                  
                  if (Attributes != null)
                  {
                      builder.AddAttribute(1, "Attributes", Attributes);
                  }

                  
                  if (vv != null)
                  {
                      var valuePr = gt.GetProperty(nameof(ValueInput<int>.value));
                      var vt = vv.GetType();
                      Console.WriteLine("valuePr.PropertyType == " + vt.GetName());
                      if (vt == typeof(int) && valuePr != null && valuePr.PropertyType.IsGenericInstanceOf(typeof(ForeignKey<>)))
                      {
                          Console.WriteLine("valuePr.PropertyType == " + c.GetType().GetName());
                          Console.WriteLine("valuePr.PropertyType == " + valuePr.PropertyType.GetName());
                          vv = valuePr.PropertyType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { vv });
                      }
                      if (vt == typeof(int) && valuePr != null && valuePr.PropertyType.IsGenericInstanceOf(typeof(ForeignKey2<,>)))
                      {
                          Console.WriteLine("valuePr.PropertyType == " + c.GetType().GetName());
                          Console.WriteLine("valuePr.PropertyType == " + valuePr.PropertyType.GetName());
                          vv = valuePr.PropertyType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { vv });
                      }



                      builder.AddAttribute(1, "value0", vv);
                      PropertyInfo pr = gt.GetProperty("__valueIsNull");
                      if (pr != null)
                      {
                          //var at = gti.GetCustomFirstAttributes<ParameterAttribute>();
                          builder.AddAttribute(1, "__valueIsNull", false);
                      }

                  }
                  
                  if (true)
                  {
                      PropertyInfo pr = gt.GetProperty("ReadOnly");
                      if (pr != null)
                          builder.AddAttribute(1, "ReadOnly", ReadOnly);
                  }

                  builder.CloseComponent();
              };

        public static List<Attribute> getCollectionItemAttrs(List<Attribute> collectionAttr)
        {
            var a = collectionAttr.FindAll(x => x is CollectionAttr).ConvertAll<CollectionAttr>(x => x as CollectionAttr);
            foreach (var r in a)
            {
                Attribute itemAtr = null;//Attribute
                if (r.type == typeof(ForeignKeyAttr))
                {
                    var itemAtrT = r.type;
                    var parmst = new Type[r.data.Length];
                    for (int i = 0; i < r.data.Length; i++)
                    {
                        parmst[i] = r.data[i].GetType();
                    }
                    var gtc = itemAtrT.GetConstructor(parmst);
                    itemAtr = gtc.Invoke(r.data) as Attribute;
                    return new List<Attribute>() { itemAtr };
                    //a[0].data //new itemAtr(a[0].data)
                }


            }
            return new List<Attribute>();
        }

        public static ComponentBase createWidget(Type type, List<Attribute> prps, ViewType viewtype = ViewType.SMALL_VIEW)
        {
            if (defultRenderer.ContainsKey(type))
                return defultRenderer[type](prps);


            if (type.IsGenericType)
            {

                try
                {
                    Console.WriteLine("createWidget type.IsGenericType ");
                    if (defultRenderer2.ContainsKey(type.GetGenericTypeDefinition()))
                    {
                        Console.WriteLine("defultRenderer2.ContainsKey(type.GetGenericTypeDefinition()) ");
                        Type type1 = defultRenderer2[type.GetGenericTypeDefinition()](type, prps);
                        Console.WriteLine(type1);
                        var gt = type1;
                        Console.WriteLine(gt);
                        var gtc = gt.GetConstructor(new Type[] { });
                        Console.WriteLine(gtc);
                        return gtc.Invoke(new object[] { }) as ComponentBase;
                    }

                }
                catch
                {

                }
            }
            if (type.IsClass && type.IsAssignableTo(typeof(Models.Entity)))
            { //TODO fix this
                Console.WriteLine("got to create GenericInSelectInt");
                Type type1 = typeof(AdminBaseComponenets.BaseComs.GenericInSelectInt<>);


                var gt = type1.MakeGenericType(type);
                var gtc = gt.GetConstructor(new Type[] { });
                return gtc.Invoke(new object[] { }) as ComponentBase;

            }

            if (type.IsEnum)
            {

                var wt = typeof(AdminBaseComponenets.BaseComs.EnumInputOption3<>).MakeGenericType(type);
                var wc = wt.GetConstructor(new Type[] { });
                return wc.Invoke(new object[] { }) as ComponentBase;
            }

            return null;
        }
        public static ComponentBase createForm3(Type tt)
        {
            Console.WriteLine("createForm3");


            return typeof(AdminBaseComponenets.BaseComs.PointerInput2<>).MakeGenericType(tt).GetConstructor(new Type[] { }).Invoke(new object[] { }) as ComponentBase;

        }
         public static ComponentBase createFormWithPointer(Type tt)
        {
            Console.WriteLine("createFormWithPointer");

            if(tt.IsClass){
                return typeof(AdminBaseComponenets.BaseComs.PointerInput2<>).MakeGenericType(tt).GetConstructor(new Type[] { }).Invoke(new object[] { }) as ComponentBase;
            }
            var wt=typeof(AdminBaseComponenets.BaseComs.PrimitiveCInput<>).MakeGenericType(tt);
            var wc = wt.GetConstructor(new Type[] { });
            return wc.Invoke(new object[] { }) as ComponentBase;

        }


        public static ComponentBase createForm4(Type type,List<Attribute> attrs = null)
        {

            if(attrs!=null)
                attrs=new List<Attribute>();
            Console.WriteLine("createForm4 " + type.GetName());
            if (formRenderer.ContainsKey(type))
            {
                var tmp = formRenderer[type](attrs);
                if (tmp != null)
                    return tmp;
            }

            if (type.IsGenericType)
            {

                try
                {
                    Console.WriteLine("type.IsGenericType " + type.GetName());
                    if (formRenderer2.ContainsKey(type.GetGenericTypeDefinition()))
                    {
                        Console.WriteLine("formRenderer2.ContainsKey(type.GetGenericTypeDefinition()) " + type.GetName());
                        Type type1 = formRenderer2[type.GetGenericTypeDefinition()](type, attrs);
                        Console.WriteLine(type1);
                        var gt = type1;
                        Console.WriteLine(gt);
                        var gtc = gt.GetConstructor(new Type[] { });
                        Console.WriteLine(gtc);
                        return gtc.Invoke(new object[] { }) as ComponentBase;
                    }

                }
                catch
                {
                    Console.WriteLine("createForm exception on generic Fromrender2");
                }
            }


            if (type.IsClass)
            {
                var wt = typeof(AdminBaseComponenets.BaseComs.PointerInput2<>).MakeGenericType(type);
                var wc = wt.GetConstructor(new Type[] { });
                return wc.Invoke(new object[] { }) as ComponentBase;
            }

            return createForm(type, attrs);
        }

        public static ComponentBase createForm2(PropertyInfo property)
        {



            var type = property.PropertyType;
            var attrs = property.GetCustomAttributes(typeof(object), false).ToList().ConvertAll<Attribute>(x => x as Attribute);

            if (inPropRender.ContainsKey(type))
            {
                return createForm(property.PropertyType, attrs);
            }
            if (type.IsGenericType &&   inPropRender.ContainsKey(type.GetGenericTypeDefinition()))
            {
                return createForm(property.PropertyType, attrs);
            }




            if (property.PropertyType.IsClass)
            {
                var wt = typeof(AdminBaseComponenets.BaseComs.PointerInput2<>).MakeGenericType(property.PropertyType);
                var wc = wt.GetConstructor(new Type[] { });
                return wc.Invoke(new object[] { }) as ComponentBase;
            }

            return createForm(property.PropertyType, attrs);


        }

        public static ComponentBase createForm(Type type, List<Attribute> attrs)
        {

            Console.WriteLine("createForm " + type.Name);
            if (attrs == null)
            {
                Console.WriteLine("attrs==null ");
                attrs = new List<Attribute>();
            }

            if (formRenderer.ContainsKey(type))
            {
                var tmp = formRenderer[type](attrs);
                if (tmp != null)
                    return tmp;
            }

            if (type.IsGenericType)
            {

                try
                {
                    Console.WriteLine("type.IsGenericType " + type.GetName());
                    if (formRenderer2.ContainsKey(type.GetGenericTypeDefinition()))
                    {
                        Console.WriteLine("formRenderer2.ContainsKey(type.GetGenericTypeDefinition()) " + type.GetName());
                        Type type1 = formRenderer2[type.GetGenericTypeDefinition()](type, attrs);
                        Console.WriteLine(type1);
                        var gt = type1;
                        Console.WriteLine(gt);
                        var gtc = gt.GetConstructor(new Type[] { });
                        Console.WriteLine(gtc);
                        return gtc.Invoke(new object[] { }) as ComponentBase;
                    }

                }
                catch
                {
                    Console.WriteLine("createForm exception on generic Fromrender2");
                }
            }

            if (type.IsEnum)
            {

                var wt = typeof(AdminBaseComponenets.BaseComs.EnumInput3<>).MakeGenericType(type);
                var wc = wt.GetConstructor(new Type[] { });
                return wc.Invoke(new object[] { }) as ComponentBase;
            }

            try
            {
                Console.WriteLine("go to make generic from for " + type.Name);
                var gt = typeof(AdminBaseComponenets.BaseComs.GenericForm<>).MakeGenericType(type);
                Console.WriteLine("go to make generic from for " + type.Name);
                var gtc = gt.GetConstructor(new Type[] { });
                Console.WriteLine("go to make generic from for " + type.Name);
                return gtc.Invoke(new object[] { }) as ComponentBase;
            }
            catch
            {

            }


            return null;
        }
        public static Type[] getValueKeyPair(string entityName)
        {
            Type[] genericArgs = new Type[] { null, null };
            genericArgs[0] =  resources.FirstOrDefault(x => x.FullName.Replace(".", "__") == entityName);


            if (genericArgs[0].IsSubclassOf(typeof(IdMapper<string>)))
                genericArgs[1] = typeof(string);
            if (genericArgs[0].IsSubclassOf(typeof(IdMapper<Guid>)))
                genericArgs[1] = typeof(Guid);

            if (genericArgs[0].IsSubclassOf(typeof(IdMapper<int>)))
                genericArgs[1] = typeof(int);

            return genericArgs;
        }

        public static void GenerateWidgetsFunctions()
        {

            defultRenderer[typeof(string)] = (prps) =>
            {
                var a = prps.FindAll(x => x is ForeignKeyAttr).ConvertAll<ForeignKeyAttr>(x => x as ForeignKeyAttr);
                if (a.Count > 0)
                {
                    var ct = a[0].type;
                    var gt = typeof(FFJ<>).MakeGenericType(ct);
                    var gtc = gt.GetConstructor(new[] { ct });
                }
                return null;
            };

            defultRenderer[typeof(int)] = (prps) =>
            {

                var a = prps.GetFirst<Attribute, ForeignKeyAttr>();

                if (a != null)
                {
                    Console.WriteLine("defultRenderer[typeof(int)]");
                    
                    var gt = typeof(ForeignKey2<,>).MakeGenericType(a.getTypes());
                    var gtc = gt.GetConstructor(new[] { typeof(int) });

                    return createWidget(
                            gt,
                            new List<Attribute>()
                    );/**/

                }

                return createWidget(
                                typeof(string),
                                new List<Attribute>()
                        );
            };

            defultRenderer[typeof(Rial)] = (prps) =>
           {

               return new AdminBaseComponenets.BaseComs.RialInGrid();


           };



            defultRenderer[typeof(double)] = (prps) =>
            {


                return createWidget(
                                typeof(string),
                                new List<Attribute>()
                        );
            };


            defultRenderer[typeof(bool)] = (prps) =>
            {


                return createWidget(
                                typeof(int),
                                new List<Attribute>()
                        );
            };

            

          

            defultRenderer2[typeof(ForeignKey<>)] = (type, prps) =>
            {
                Console.WriteLine("defultRenderer2[ForeignKey<>]");
                Console.WriteLine($"defultRenderer2[ForeignKey<>] IntegerFSmallView<{type.GetGenericArguments()[0]}>");
                var z = type.GetGenericArguments().ToList();
                z.Add(typeof(int));
                return typeof(AdminBaseComponenets.BaseComs.IntegerFSmallView<,>).MakeGenericType(z.ToArray());

            };
            defultRenderer2[typeof(ForeignKey2<,>)] = (type, prps) =>
            {
                Console.WriteLine("defultRenderer2[ForeignKey<>]");
                Console.WriteLine($"defultRenderer2[ForeignKey<>] IntegerFSmallView<{type.GetGenericArguments()}>");
                return typeof(AdminBaseComponenets.BaseComs.IntegerFSmallView<,>).MakeGenericType(type.GetGenericArguments());

            };





            defultRenderer[typeof(List<int>)] = (prps) =>
            {

                var a = getCollectionItemAttrs(prps);
                /*foreach (var zz in z)
                {

                    createWidget(
                            typeof(int),
                            a
                    );
                }/**/
                return null;

            };




        }


        public static void GenerateForm()
        {
            defultCunstroctor[typeof(String)] = () => new String("");

            formRenderer2[typeof(Func<,>)] = (type, prps) =>
            {
                return typeof(AdminBaseComponenets.BaseComs.AFunc<,>).MakeGenericType(type.GetGenericArguments());
            };


            formRenderer2[typeof(Nullable<>)] = (type, prps) =>
            {
                {

                    var a = prps.GetFirst<Attribute, ForeignKeyAttr>();

                    if (a != null)
                    {
                        var ct = a.type;
                        var gt = typeof(ForeignKey<>).MakeGenericType(ct);


                        return typeof(AdminBaseComponenets.BaseComs.NullabeType<>).MakeGenericType(new Type[] { gt });

                    }
                }

                return typeof(AdminBaseComponenets.BaseComs.NullabeType<>).MakeGenericType(type.GetGenericArguments());
            };
            inPropRender[typeof(string)]=true;
            formRenderer[typeof(string)] = (prps) =>
            {
                /*{
                    var x = prps.GetFirst<Attribute, AdminBaseComponenets.BaseComs.SmallPicShow>();
                    if (x != null)
                        return new AdminBaseComponenets.BaseComs.FileUpload();
                }
                if (true)
                {
                    var x = prps.GetFirst<Attribute, AdminBaseComponenets.BaseComs.SmallVideoShow>();
                    if (x != null)
                        return new AdminBaseComponenets.BaseComs.VideoUpload();
                }*/
                return new AdminBaseComponenets.BaseComs.StringInput();
            };
            formRenderer[typeof(DateTime)] = (prps) =>
            {
                return new AdminBaseComponenets.BaseComs.PersianDateTimePicker();
            };
            formRenderer[typeof(DateTime)] = (prps) =>
             {
                 return new AdminBaseComponenets.BaseComs.PersianDateTimePicker();
             };

            formRenderer[typeof(DateTimeOffset)] = (prps) =>
            {
                return new AdminBaseComponenets.BaseComs.PersianDateTimePicker();
            };
            formRenderer[typeof(DateTimeOffset)] = (prps) =>
            {
                return new AdminBaseComponenets.BaseComs.PersianDateTimeOffsetPicker();
            };

           

            formRenderer[typeof(int)] = (prps) =>
            {
                Console.WriteLine("formRenderer[typeof(int)]");
                var a = prps.GetFirst<Attribute, ForeignKeyAttr>();

                if (a != null)
                {
                    Console.WriteLine("formRenderer[typeof(int)] ");
                    Console.WriteLine($"formRenderer[typeof(int)] ForeignKey2<{a.getTypes()}>");
                    var gt = typeof(ForeignKey2<,>).MakeGenericType(a.getTypes());
                    //var gtc = gt.GetConstructor(new[] { typeof(int) });

                    return createForm(
                            gt,
                            new List<Attribute>()
                    );/**/

                }
                return new AdminBaseComponenets.BaseComs.IntInput();
            };



            formRenderer[typeof(long)] = (prps) =>
            {

                return new AdminBaseComponenets.BaseComs.PrimitiveInput<long>();
            };






            formRenderer[typeof(ulong)] = (prps) =>
            {
                return new AdminBaseComponenets.BaseComs.PrimitiveInput<ulong>();
            };


            formRenderer[typeof(Rial)] = (prps) =>
            {

                return new AdminBaseComponenets.BaseComs.RialInput();
            };







            formRenderer[typeof(double)] = (prps) =>
           {


               return new AdminBaseComponenets.BaseComs.PrimitiveInput<double>();
           };


            formRenderer[typeof(bool)] = (prps) =>
            {


                return new AdminBaseComponenets.BaseComs.BoolInput();
            };

           

           
            


            /*formRenderer[typeof(Models.OldDataCoachList)] = (props) =>
            {


                return new AdminClient.Components.OldDataCoachList.Edit()
                {

                };

            };/**/


            formRenderer2[typeof(ForeignKey<>)] = (type, prps) =>
            {
                Console.WriteLine("formRenderer2(ForeignKey<>)");
                Console.WriteLine($"formRenderer2(ForeignKey<>) ForeignKeyEdite<{type.GetGenericArguments()[0]}>");
                return typeof(AdminBaseComponenets.BaseComs.ForeignKeyEditeInt<>).MakeGenericType(type.GetGenericArguments()); ;
            };
            formRenderer2[typeof(ForeignKey2<,>)] = (type, prps) =>
            {
                Console.WriteLine("formRenderer2(ForeignKey<>)");
                Console.WriteLine($"formRenderer2(ForeignKey<>) ForeignKeyEdite<{type.GetGenericArguments()[0]}>");
                return typeof(AdminBaseComponenets.BaseComs.ForeignKeyEdite<,>).MakeGenericType(type.GetGenericArguments()); ;
            };





            formRenderer2[typeof(PerimitveContainer<>)] = (type, prps) =>
            {

                var ItemType = type.GetGenericArguments()[0];
                return typeof(AdminBaseComponenets.BaseComs.PrimitiveCInput<>).MakeGenericType(ItemType);
            };


            formRenderer2[typeof(Dictionary<,>)] = (type, prps) =>
            {
                Console.WriteLine("formRenderer2(Dictionary<,>)");
                var ItemType = type.GetGenericArguments()[1];
                
                {
                    var x = prps.GetFirst<Attribute, GridShow>();
                    if (x != null)
                    {
                        return typeof(AdminBaseComponenets.BaseComs.DictinaryStringKeyInput<>).MakeGenericType(type.GetGenericArguments()[1]);
                    }
                }

                return typeof(AdminBaseComponenets.BaseComs.ArrayInput<>).MakeGenericType(type.GetGenericArguments()[0]);
            };
            inPropRender[typeof(List<>)]=true;
            formRenderer2[typeof(List<>)] = (type, prps) =>
            {
                Console.WriteLine("formRenderer2(List<>)");
                var ItemType = type.GetGenericArguments()[0];
                if (ItemType.IsGenericType && ItemType.GetGenericTypeDefinition() == typeof(ForeignKey<>))
                {
                    var x = prps.GetFirst<Attribute, MultiSelect>();
                    if (x != null)
                        return typeof(AdminBaseComponenets.BaseComs.IntegerFMultiSelect<>).MakeGenericType(type.GetGenericArguments()[0].GetGenericArguments()[0]);
                }

                if (ItemType.IsEnum)
                {
                    var x = prps.GetFirst<Attribute, MultiSelect>();
                    if (x != null)
                        return typeof(AdminBaseComponenets.BaseComs.EnumMultiSelectInput<>).MakeGenericType(type.GetGenericArguments()[0]);
                }
                {
                    var x = prps.GetFirst<Attribute, GridShow>();
                    if (x != null)
                    {
                        return typeof(AdminBaseComponenets.BaseComs.DataListSyncfusion<,>).MakeGenericType(type.GetGenericArguments()[0]);
                    }
                }

                return typeof(AdminBaseComponenets.BaseComs.ArrayInput<>).MakeGenericType(type.GetGenericArguments()[0]);
            };
            inPropRender[typeof(HashSet<>)]=true;
            formRenderer2[typeof(HashSet<>)] = (type, prps) =>
            {
                Console.WriteLine("formRenderer2(HashSet<>)");
                var ItemType = type.GetGenericArguments()[0];
                if (ItemType.IsGenericType && ItemType.GetGenericTypeDefinition() == typeof(ForeignKey<>))
                {
                    var x = prps.GetFirst<Attribute, MultiSelect>();
                    if (x != null)
                        return typeof(AdminBaseComponenets.BaseComs.IntegerFMultiSelect<>).MakeGenericType(type.GetGenericArguments()[0].GetGenericArguments()[0]);
                }

                if (ItemType.IsEnum)
                {
                    var x = prps.GetFirst<Attribute, MultiSelect>();
                    if (x != null)
                        return typeof(AdminBaseComponenets.BaseComs.EnumMultiSelectInput<>).MakeGenericType(type.GetGenericArguments()[0]);
                }
                {
                    var x = prps.GetFirst<Attribute, GridShow>();
                    if (x != null)
                    {
                        return typeof(AdminBaseComponenets.BaseComs.DataListSyncfusion<,>).MakeGenericType(type.GetGenericArguments()[0]);
                    }
                }

                return typeof(AdminBaseComponenets.BaseComs.ArrayInput<>).MakeGenericType(type.GetGenericArguments()[0]);
            };

            formRenderer2[typeof(Dictionary<,>)] = (type, prps) =>
            {
                Console.WriteLine("Program0.formRenderer2(Dictionary<,>)");
                var ItemType = type.GetGenericArguments()[1];

                {
                    var x = prps.GetFirst<Attribute, GridShow>();
                    if (x != null)
                    {
                        return typeof(AdminBaseComponenets.BaseComs.DictinaryStringKeyInput<>).MakeGenericType(type.GetGenericArguments()[1]);
                    }
                }

                return typeof(AdminBaseComponenets.BaseComs.ArrayInput<>).MakeGenericType(type.GetGenericArguments()[0]);
            };









        }

        public static string GetDisplayName<T>()
        {
            var displayName = typeof(T)
              .GetCustomAttributes(typeof(DisplayNameAttribute), true)
              .FirstOrDefault() as DisplayNameAttribute;

            return displayName != null ? displayName.DisplayName : string.Empty;
        }
        


        public static string ConvertUrl(string url)
        {
            return url.Replace("/", "__slash__");
        }
        public static string PConvertUrl(string url)
        {
            return url.Replace("__slash__", "/");
        }
        public static Dictionary<string, Type> apis = new Dictionary<string, Type>();
        public static Dictionary<Regex, Type> apis2 = new Dictionary<Regex, Type>();

        public static void addApiRegex(string url, Type t)
        {
            var rr = "";
            int b = 0;
            foreach (var c in url)
            {
                if (c == '{')
                    ++b;
                if (c == '}')
                {
                    --b;
                    if (b == 0)
                    {
                        rr += "[0-9]+";
                    }
                    continue;
                }
                if (b == 0)
                {
                    rr += c;
                }
            }

            Regex rgx = new Regex(rr);
            apis2[rgx] = t;

        }
        public static Type getFuncType(string url)
        {

            if (apis.ContainsKey(url))
                return apis[url];
            foreach (var e in apis2)
            {
                Match match = e.Key.Match(url);
                if (match.Success)
                    return e.Value;
            }
            return null;

            //return typeof(System.Func<AdminMsgs.TimeRangeRequest,AdminMsgs.MaliCoach>);
        }
        public static async Task Main(string[] args)
        {

         

            
            //Register Syncfusion license
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjM3OEAzMTM5MmUzMTJlMzBVeS82aFZBTTBzSG56NU1iekJscW9VN0s1UGJMcHBMRlFYMGduOUgxaUFvPQ==");

            GenerateWidgetsFunctions();
            GenerateForm();
            //enitits.Add(typeof(Models.Class));



           

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            
            Console.WriteLine(builder.HostEnvironment.BaseAddress);
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            

            /*{
                builder.Services.Configure<CookiePolicyOptions>(options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });
                builder.Services.AddHttpContextAccessor();
            }/**/



            await builder.Build().RunAsync();

        }
    }
}
