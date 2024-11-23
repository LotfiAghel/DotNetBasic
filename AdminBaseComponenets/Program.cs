using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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
using Newtonsoft.Json;
using Newtonsoft.Json;
using Models;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;
using AdminClientViewModels;
using Models;
using Tools;
using Tools;
using AdminBaseComponenets.BaseComs;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using AdminBaseComponenets.BaseComs.InGrid;
using Blazorise.Extensions;

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
        public static Dictionary<Type, Func<Type, List<Attribute>, ValueInput0>> defultRenderer2 = new Dictionary<Type, Func<Type, List<Attribute>, ValueInput0>>();


        public static Dictionary<Type, Func<List<Attribute>, ValueInput0>> formRenderer = new ();


        public static Dictionary<Type,bool> inPropRender = new Dictionary<Type, bool>();
        public static Dictionary<Type, Func<Type, List<Attribute>, Type>> formRenderer2 = new Dictionary<Type, Func<Type, List<Attribute>, Type>>();

        public static Dictionary<Type, Func<Type, List<Attribute>, IEnumerable >> generator2 = new ();
        public static Dictionary<Type, Func<Type, List<Attribute>, IEnumerable>> generator = new();


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
                      builder.AddAttribute(1, "OnChange", callback);
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
            var keyTypes = new List<Type>(){ typeof(int), typeof(string), typeof(System.Guid) , typeof(long) };
            foreach (var KeyType in keyTypes)
                if (ty.IsAssignableTo(typeof(IIdMapper<>).MakeGenericType(KeyType)))
                {
                    b = typeof(NewEntityService<,>).MakeGenericType(new Type[] { ty, KeyType }).GetConstructor(new Type[] { }).Invoke(new object[] { });
                    entityManagers[ty] = b as IEntityService00;
                    return b as IEntityService00;
                }
            return null;
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
        public static RenderFragment CreateDynamicComponent20(object thiz, ValueInput0 c, object vv, Action<object> OnChange = null, List<Attribute> Attributes = null, bool ReadOnly = false) => builder =>
              {
                  if (c == null)
                      return ;

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
                  
                  if (OnChange != null)
                  {
                      var callback = EventCallback.Factory.Create<object>(thiz, OnChange);
                      builder.AddAttribute(1, "OnChange", OnChange);
                  }
                  
                  if (Attributes != null)
                  {
                      builder.AddAttribute(1, "Attributes", Attributes);
                  }

                  
                  if (vv != null)
                  {
                      var valuePr = gt.GetProperty(nameof(ValueInput<int>.value));
                      var vt = vv.GetType();
                      
                      if (vt == typeof(int) && valuePr != null && valuePr.PropertyType.IsGenericInstanceOf(typeof(ForeignKey<>)))
                      {
                          vv = valuePr.PropertyType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { vv });
                      }
                      if (vt == typeof(int) && valuePr != null && valuePr.PropertyType.IsGenericInstanceOf(typeof(ForeignKey2<,>)))
                      {
                          vv = valuePr.PropertyType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { vv });
                      }
                      if (vt == typeof(Guid) && valuePr != null && valuePr.PropertyType.IsGenericInstanceOf(typeof(ForeignKey2<,>)))
                      {
                          vv = valuePr.PropertyType.GetConstructor(new Type[] { typeof(Guid) }).Invoke(new object[] { vv });
                      }

                      if (gt.GetProperty("value").PropertyType != vv.GetType())
                      {
                          Console.Error.WriteLine("type!!");
                      }

                      builder.AddAttribute(1, nameof(ValueInput0.value0), vv);
                      PropertyInfo pr = gt.GetProperty("__valueIsNull");
                      if (pr != null)
                      {
                          //var at = gti.GetCustomFirstAttributes<ParameterAttribute>();
                          builder.AddAttribute(1, "__valueIsNull", false);
                      }

                  }
                  
                  if (true)
                  {
                      PropertyInfo pr = gt.GetProperty(nameof(ValueInput0.ReadOnly));
                      if (pr != null)
                          builder.AddAttribute(1, nameof(ValueInput0.ReadOnly), ReadOnly);
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
                if (r.type.IsAssignableTo( typeof(FileString)))
                {
                    var itemAtrT = r.type;
                    
                    if(r.data == null)
                    {
                        return new List<Attribute>() { itemAtrT.GetConstructor(new Type[] { }).Invoke(new object[] { }) as Attribute };
                    }
                    var parmst = new Type[r.data.Length];
                    for (int i = 0; i < r.data.Length; i++)
                    {
                        parmst[i] = r.data[i].GetType();
                    }
                    var gtc = itemAtrT.GetConstructor(parmst);
                    itemAtr = gtc.Invoke(r.data) as Attribute;
                    return new List<Attribute>() { itemAtr };
                }


            }
            return new List<Attribute>();
        }


        public static IEnumerable createGenerator(Type type, List<Attribute> prps)
        {
            if (generator.ContainsKey(type))
            {
                return generator[type](type, prps);
            }
            
            if(type.IsGenericType && generator2.ContainsKey(type.GetGenericTypeDefinition()))
                return generator2[type.GetGenericTypeDefinition()](type, prps);
            if(type.IsEnum)
            {
                return typeof(EnumMakerdGEnerator<>).MakeGenericType(new Type[] { type}).GetConstructor(new Type[] { }).Invoke(new object[] { }) as IEnumerable;
            }
            return null;
        }

        public static ComponentBase createWidget(Type type, List<Attribute> prps, ViewType viewtype = ViewType.SMALL_VIEW)
        {
            if (defultRenderer.ContainsKey(type))
                return defultRenderer[type](prps);

            
            if (type.IsGenericType)
            {

                try
                {
                    if (defultRenderer2.ContainsKey(type.GetGenericTypeDefinition()))
                    {
                        var result  = defultRenderer2[type.GetGenericTypeDefinition()](type, prps);
                        result.Attributes = prps;
                        return result;
                    }

                }
                catch
                {

                }
            }
            foreach(var cc in defultRenderer)
            {
                if (type.IsAssignableTo(cc.Key))
                { 
                    
                    return cc.Value.Invoke( prps ) ;
                    
                }
            }
            if (type.IsClass && type.IsAssignableTo(typeof(Models.IEntity0)))
            { //TODO fix this
                Console.WriteLine("got to create GenericInSelectInt");
                Type type1 = typeof(AdminBaseComponenets.BaseComs.InGrid.GenericInSelectInt<>);


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

            if(tt.IsClass || tt.IsInterface){
                return typeof(AdminBaseComponenets.BaseComs.PointerInput2<>).MakeGenericType(tt).GetConstructor(new Type[] { }).Invoke(new object[] { }) as ComponentBase;
            }
            var wt=typeof(AdminBaseComponenets.BaseComs.PrimitiveCInput<>).MakeGenericType(tt);
            var wc = wt.GetConstructor(new Type[] { });
            return wc.Invoke(new object[] { }) as ComponentBase;

        }


        public static ComponentBase createForm4(Type type,List<Attribute> attrs = null)
        {

            //if(attrs!=null)
            //    attrs=new List<Attribute>();
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


            if (type.IsClass || type.IsInterface)
            {
                var wt = typeof(AdminBaseComponenets.BaseComs.PointerInput2<>).MakeGenericType(type);
                var wc = wt.GetConstructor(new Type[] { });
                return wc.Invoke(new object[] { }) as ComponentBase;
            }

            return createForm(type, attrs);
        }

        public static ValueInput0 createForm2<TItem>(PropertyInfo property)
        {


            var type0 =  typeof(TItem);
            var type  =  property.PropertyType;
            var attrs =  property.GetCustomAttributes<Attribute>(false).ToList();
            //ForeignKeyAttr.cacl(property.DeclaringType);
            var md=MDTypeInfo.get(type0);
            property.GetHashCode();
            var extaAtr= md.pattrs[property.GetMetadataToken()].attrs;
            attrs.AddRange(extaAtr);
            if (inPropRender.ContainsKey(type))
            {
                return createForm(property.PropertyType, attrs);
            }
            if (type.IsGenericType &&   inPropRender.ContainsKey(type.GetGenericTypeDefinition()))
            {
                return createForm(property.PropertyType, attrs);
            }

            if (type.IsArray)
            {

                var wt = typeof(AdminBaseComponenets.BaseComs.PArrayInput<>).MakeGenericType(type.GetElementType());
                //var wt = typeof(AdminBaseComponenets.BaseComs.EnumInput3<>).MakeGenericType(type);
                var wc = wt.GetConstructor(new Type[] { });
                return wc.Invoke(new object[] { }) as ValueInput0;
            }


            if (property.PropertyType.IsClass || property.PropertyType.IsInterface)
            {
                var wt = typeof(AdminBaseComponenets.BaseComs.PointerInput2<>).MakeGenericType(property.PropertyType);
                var wc = wt.GetConstructor(new Type[] { });
                return wc.Invoke(new object[] { }) as ValueInput0;
            }

            return createForm(property.PropertyType, attrs);


        }
        public static RenderFragment CreateDynamicComponentActionbar(Type entityMetaClass0, object actionValue,object value, Type[] genericArgs) => builder =>
        {

            //var gridMetaClass = typeof(ActionBar<>).MakeGenericType(new Type[] {  typeof(Models.IAction<>).MakeGenericType(new Type[]{entityMetaClass0}) });
            var gridMetaClass = typeof(ActionBar<,>).MakeGenericType(genericArgs);
            builder.OpenComponent(0, gridMetaClass);
            if (actionValue != null)
                builder.AddAttribute(1, "value", actionValue);
            if (value is Models.IEntity0 vl2)
                builder.AddAttribute(1, "entityId", vl2.getId());


            builder.CloseComponent();
        };

        public static ValueInput0 createForm(Type type, List<Attribute> attrs) { 
        
            if(type==null)
                return null;

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
                        return gtc.Invoke(new object[] { }) as ValueInput0;
                    }

                }
                catch(Exception e)
                {
                    Console.WriteLine("createForm exception on generic Fromrender2");
                    Console.WriteLine(e.Message);
                }
            }

            if (type.IsEnum)
            {

                var wt = typeof(AdminBaseComponenets.BaseComs.EnumInput3<>).MakeGenericType(type);
                var wc = wt.GetConstructor(new Type[] { });
                return wc.Invoke(new object[] { }) as ValueInput0;
            }
            if (type.IsArray)
            {

                var wt= typeof(AdminBaseComponenets.BaseComs.PArrayInput<>).MakeGenericType(type.GetElementType());
                //var wt = typeof(AdminBaseComponenets.BaseComs.EnumInput3<>).MakeGenericType(type);
                var wc = wt.GetConstructor(new Type[] { });
                return wc.Invoke(new object[] { }) as ValueInput0;
            }

            try
            {
                Console.WriteLine("go to make generic from for " + type.Name);
                var gt = typeof(AdminBaseComponenets.BaseComs.GenericForm<>).MakeGenericType(type);
                Console.WriteLine("go to make generic from for " + type.Name);
                var gtc = gt.GetConstructor(new Type[] { });
                Console.WriteLine("go to make generic from for " + type.Name);
                return gtc.Invoke(new object[] { }) as ValueInput0;
            }
            catch
            {

            }


            return null;
        }


        public static IAdminUser user;

        
        public static bool checkPermission<T2>(Type T) where T2 : ACLAtr
        {
            //return true;
            try
            {
                var adminWriteBan = T.GetCustomAttributes(typeof(T2), true).OfType<T2>().FirstOrDefault();
                if (adminWriteBan == null || adminWriteBan.kinds==null || user==null || adminWriteBan.kinds.Intersect(user.roles).Count() == 0 )
                    return false;
            }
            catch
            {
                return false;
            };
            return true;

        }
        public static async Task<bool> CheckLogin(IJSRuntime JsRuntime)
        {


            var isAuthenticated = await JsRuntime.InvokeAsync<string>("blazorExtensions.GetCookie", new[] { "isAuthenticated" }) == "true";

            try
            {
                var url = await JsRuntime.InvokeAsync<string>("blazorExtensions.GetCookie", new[] { "url" });
                ClTool.WebClient.webClient.baseUrl = url;

            }
            catch
            {

            }

            return isAuthenticated;



        }
        /*async Task CheckLogin()
        {
            var isAuthenticated = await Program0.CheckLogin(JsRuntime);
            if (!isAuthenticated)
            {
                if (!NavigationManager.Uri.Contains("/login"))
                {
                    NavigationManager.NavigateTo("/login", false);
                }
            }
        }*/

        public static Type getKeyType(Type entity){
            if (entity.IsAssignableTo(typeof(IIdMapper<string>)))
                return typeof(string);
            if (entity.IsAssignableTo(typeof(IIdMapper<Guid>)))
                return typeof(Guid);

            if (entity.IsAssignableTo(typeof(IIdMapper<int>)))
                return typeof(int);
            if (entity.IsAssignableTo(typeof(IIdMapper<long>)))
                return typeof(long);
            throw new NotImplementedException();

        }
        public static Type[] getValueKeyPair(Type type)
        {
            Type[] genericArgs = new Type[] { null, null };
            genericArgs[0] =type;

            genericArgs[1] = getKeyType(genericArgs[0]);

            return genericArgs;
        }
        public static Type[] getValueKeyPair(string entityName)
        {
            Type[] genericArgs = new Type[] { null, null };
            Console.WriteLine(entityName);
            Console.WriteLine(resources.Count());
            foreach (var t in resources)
                Console.WriteLine(t.GetUrlEncodeName());
            return getValueKeyPair(resources.FirstOrDefault(x => x.GetUrlEncodeName() == entityName));
        }


        public static void GenerateWidgetsFunctions()
        {

            defultRenderer[typeof(string)] = (prps) =>
            {
                {
                    var a = prps.GetFirst<Attribute, ForeignKeyAttr>();
                

                    if (a != null)
                    {

                        var gt = typeof(ForeignKey2<,>).MakeGenericType(a.getTypes());
                        var gtc = gt.GetConstructor(new[] { typeof(int) });

                        return createWidget(
                            gt,
                            new List<Attribute>()
                        ); /**/

                    }
                }
                {
                    var a = prps.GetFirst<Attribute, SmallPicShow>();
                

                    return new FileInGrid();
                }
                return new StringInGrid();
            };
             defultRenderer[typeof(Type)] = (prps) =>
            {
               
                
                return new TypeInGrid();
            };
            defultRenderer[typeof(Guid)] = (prps) =>
            {
                var a = prps.OfType<ForeignKeyAttr>().ToList();
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

                return null;
            };
            defultRenderer[typeof(Guid)] = (prps) =>
            {

                var a = prps.GetFirst<Attribute, ForeignKeyAttr>();

                if (a != null)
                {
                    Console.WriteLine("defultRenderer[typeof(Guid)]");
                    
                    var gt = typeof(ForeignKey2<,>).MakeGenericType(a.getTypes());
                    var gtc = gt.GetConstructor(new[] { typeof(Guid) });

                    return createWidget(
                            gt,
                            new List<Attribute>()
                    );/**/

                }

                return null;
            };

            defultRenderer[typeof(Rial)] = (prps) =>
           {

               return new RialInGrid();


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
                var z = type.GetGenericArguments().ToList();
                z.Add(typeof(int));
                return typeof(BaseComs.InGrid.IntegerFSmallView<,>).MakeGenericType(z.ToArray()).GetConstructor(new Type[]{}).Invoke(new object[]{}) as ValueInput0;

            };
            defultRenderer2[typeof(ForeignKey2<,>)] = (type, prps) =>
            {
                return typeof(BaseComs.InGrid.IntegerFSmallView<,>).MakeGenericType(type.GetGenericArguments()).GetConstructor(new Type[]{}).Invoke(new object[]{}) as ValueInput0;

            };
            defultRenderer2[typeof(List<>)] = (type, prps) =>
            {
                return typeof(BaseComs.InGrid.ListSmallView<>).MakeGenericType(type.GetGenericArguments()).GetConstructor(new Type[]{}).Invoke(new object[]{}) as ValueInput0;

            };

            



            /*defultRenderer[typeof(List<int>)] = (prps) =>
            {

                return null;

            };
            defultRenderer[typeof(List<string>)] = (prps) =>
            {

                var a = getCollectionItemAttrs(prps);
                if(a.Any(x => x.GetType().IsAssignableTo(typeof(FileString))) )
                    return new FileInGrid();
                return null;

            };*/
            




        }


        public static void GenerateForm()
        {
            defultCunstroctor[typeof(String)] = () => new String("");

            formRenderer2[typeof(FuncV<,>)] = (type, prps) =>
            {
                return typeof(AdminBaseComponenets.BaseComs.AFunc<,>).MakeGenericType(type.GetGenericArguments());
            };
            

            formRenderer2[typeof(Nullable<>)] = (type, prps) =>
            {
                {

                    var a = prps.GetFirst<Attribute, ForeignKeyAttr>();

                    if (a != null)
                    {

                        var gt = typeof(ForeignKey2<,>).MakeGenericType(a.getTypes());
                        //var gtc = gt.GetConstructor(new[] { typeof(int) });

                        

                        return typeof(AdminBaseComponenets.BaseComs.NullabeType<>).MakeGenericType(new Type[] { gt });

                    }
                }

                return typeof(AdminBaseComponenets.BaseComs.NullabeType<>).MakeGenericType(type.GetGenericArguments());
            };
            inPropRender[typeof(string)]=true;
            formRenderer[typeof(string)] = (prps) =>
            {
                Console.WriteLine("formRenderer[typeof(string)]");
                var a = prps.GetFirst<Attribute, ForeignKeyAttr>();

                if (a != null)
                {
                    Console.WriteLine("formRenderer[typeof(string)] ");
                    Console.WriteLine($"formRenderer[typeof(string)] ForeignKey2<{a.getTypes()[0]},{a.getTypes()[1]}>");
                    var gt = typeof(ForeignKey2<,>).MakeGenericType(a.getTypes());
                    //var gtc = gt.GetConstructor(new[] { typeof(int) });

                    return createForm(
                            gt,
                            new List<Attribute>()
                    );/**/

                }
                {
                    var x = prps.GetFirst<Attribute, DataTypeAttribute>();
                    if (x != null && x.DataType==DataType.MultilineText)
                        return new AdminBaseComponenets.BaseComs.StringMultiLineInput();
                        //return new AdminBaseComponenets.BaseComs.StringRichInput();
                }
                {
                    var x = prps.GetFirst<Attribute, DataTypeAttribute>();
                    if (x != null && x.DataType == DataType.ImageUrl)
                        return new AdminBaseComponenets.BaseComs.FileUploadBig();
                }
                {
                    var x = prps.GetFirst<Attribute, DataTypeAttribute>();
                    if (x != null && x.DataType == DataType.Upload)
                        return new AdminBaseComponenets.BaseComs.FileUploadBig();
                }
                {
                    var x = prps.GetFirst<Attribute, Models.SmallPicShow>();
                    if (x != null)
                        return new AdminBaseComponenets.BaseComs.FileUploadBig();
                }
                if (true)
                {
                    var x = prps.GetFirst<Attribute, Models.SmallVideoShow>();
                    if (x != null)
                        return new AdminBaseComponenets.BaseComs.FileUploadBig();
                }
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
            
            formRenderer[typeof(Guid)] = (prps) =>
            {
                Console.WriteLine("formRenderer[typeof(Guid)]");
                var a = prps.GetFirst<Attribute, ForeignKeyAttr>();

                if (a != null)
                {
                    var gt = typeof(ForeignKey2<,>).MakeGenericType(a.getTypes());


                    return createForm(
                            gt,
                            new List<Attribute>()
                    );/**/

                }
                
               
                return new AdminBaseComponenets.BaseComs.GuidInput();
            };


            



            formRenderer[typeof(long)] = (prps) =>
            {

                return new AdminBaseComponenets.BaseComs.IntInput2<long>();
            };






            formRenderer[typeof(ulong)] = (prps) =>
            {
                return new AdminBaseComponenets.BaseComs.IntInput2<ulong>();
            };


            formRenderer[typeof(Rial)] = (prps) =>
            {

                return new AdminBaseComponenets.BaseComs.RialInput();
            };

            formRenderer[typeof(JToken)]= formRenderer[typeof(JObject)]= formRenderer[typeof(JArray)] = (prps) =>
            {

                return new AdminBaseComponenets.BaseComs.JsonShow();
            };






            formRenderer[typeof(double)] = (prps) =>
           {


               return new AdminBaseComponenets.BaseComs.DoubleInput();
           };


            formRenderer[typeof(bool)] = (prps) =>
            {


                return new AdminBaseComponenets.BaseComs.BoolInput();
            };

            formRenderer[typeof(decimal)] = (prps) =>
            {
                return new AdminBaseComponenets.BaseComs.PrimitiveInput<decimal>();

                
            };

            formRenderer[typeof(float)] = (prps) =>
            {
                return new AdminBaseComponenets.BaseComs.PrimitiveInput<float>();

            };


            /*formRenderer[typeof(List<string>)] = (prps) =>
            {

                var a = getCollectionItemAttrs(prps);
                if (a != null && a.Count>0 && a.Any(x=> x is FileAttr))
                {

                }
                return new AdminBaseComponenets.BaseComs.ArrayInput<string>();


            };*/



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
            generator2[typeof(ForeignKey2<,>)] = (type, prop) =>
            {
                var generator = typeof(MarkedGenerator2<,>).MakeGenericType(type.GenericTypeArguments).GetConstructor(new Type[] { }).Invoke(new object[] { }) as IEnumerable;
                
                return generator;
            };
            formRenderer2[typeof(Models.Range<>)] = (type, prps) =>
            {
                Console.WriteLine("formRenderer2(Range<>)");
                return typeof(AdminBaseComponenets.BaseComs.RangeInput<>).MakeGenericType(type.GetGenericArguments()[0]);
            };
            
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
                if (ItemType.IsGenericType && ItemType.GetGenericTypeDefinition() == typeof(ForeignKey2<,>))
                {
                    var x = prps.GetFirst<Attribute, MultiSelect>();
                    if (x != null)
                        return typeof(AdminBaseComponenets.BaseComs.EnumMultiSelectInput<>).MakeGenericType(ItemType);
                }

                if (ItemType.IsEnum)
                {
                    var x = prps.GetFirst<Attribute, MultiSelect>();
                    if (x != null)
                        return typeof(AdminBaseComponenets.BaseComs.EnumMultiSelectInput<>).MakeGenericType(type.GetGenericArguments()[0]);
                }
                {
                    var x = prps.GetFirst<Attribute, CsvInput>();
                    if (x != null)
                    {
                        return typeof(AdminBaseComponenets.BaseComs.DataListWithCsv<,>).MakeGenericType(type.GetGenericArguments()[0],Program0.getKeyType(type.GetGenericArguments()[0]));
                    }
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
            formRenderer2[typeof(Array)] = (type, prps) =>
            {
                Console.WriteLine("formRenderer2(Array<>)");
                var ItemType = type.GetGenericArguments()[0];
                

                if (ItemType.IsEnum)
                {
                    var x = prps.GetFirst<Attribute, MultiSelect>();
                    if (x != null)
                        return typeof(AdminBaseComponenets.BaseComs.EnumMultiSelectInput<>).MakeGenericType(type.GetGenericArguments()[0]);
                }
                {
                    var x = prps.GetFirst<Attribute, CsvInput>();
                    if (x != null)
                    {
                        return typeof(AdminBaseComponenets.BaseComs.DataListWithCsv<,>).MakeGenericType(type.GetGenericArguments()[0], Program0.getKeyType(type.GetGenericArguments()[0]));
                    }
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
                    var x = prps.GetFirst<Attribute, CsvInput>();
                    if (x != null)
                    {
                        return typeof(AdminBaseComponenets.BaseComs.DataListWithCsv<,>).MakeGenericType(type.GetGenericArguments()[0], Program0.getKeyType(type.GetGenericArguments()[0]));
                    }
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
                    //if (x != null)
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

        public static void RegisterForm<T,T2>()where T2: ValueInput<T>,new()
        {
            Program0.formRenderer[typeof(T)] = (prps) =>
            {


                return new T2();
            };
            
            
        }
        public static void RegisterForm<T,T2,T3>()where T2: ValueInput<T3>,new()
        where T:T3
        {
            Program0.formRenderer[typeof(T)] = (prps) =>
            {


                return new T2();
            };
            
            
        }

        public static void showPopUp(Exception e)
        {
            //throw new NotImplementedException();
        }
    }
}
