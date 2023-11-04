using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Data;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tools;

namespace SGSStandalone.Core.ClientMessage
{

   
    /*class Serilizer
    {
        SerilizingIgnoreTag tag;
        public static Serilizer ClusterSerilizer = new Serilizer(SerilizingIgnoreTag.CLUSTER);
        public static Serilizer ClientSerilizer = new Serilizer(SerilizingIgnoreTag.CLIENT);
        public static Serilizer DbSerilizer = new Serilizer(SerilizingIgnoreTag.DATABASE);
        public Serilizer(SerilizingIgnoreTag tag) {
            this.tag = tag;

        }
        public static Dictionary<Type, Func<Serilizer,object, JToken>> serilizerMap = new Dictionary<Type, Func<Serilizer, object, JToken>>();
        public static Dictionary<Type, Func<Serilizer, JToken, object>> deserilizerMap = new Dictionary<Type, Func<Serilizer, JToken, object>>();


        public static Dictionary<Type, Func<Serilizer,object, JToken>> serilizerGenericMap = new Dictionary<Type, Func<Serilizer, object, JToken>>();
        public static Dictionary<Type, Func<Serilizer,JToken, object, Type, object>> deserilizerGenericMap = new Dictionary< Type, Func<Serilizer, JToken,object, Type,object>>();
        public static Dictionary<Type, Func<Serilizer,Type, object>> constructor = new Dictionary<Type, Func<Serilizer, Type,object>>();



        public JToken serilizerAll(object data, Type refrenceBaseType)
        {
            if (data == null)
                return null;
            if (refrenceBaseType == typeof(string))
                return JToken.FromObject(data);
            if (refrenceBaseType == typeof(String))
                return JToken.FromObject(data);
            if (serilizerMap.ContainsKey(data.GetType()))
                return serilizerMap[data.GetType()](this,data);

           

            if (data.GetType().IsGenericType &&  serilizerGenericMap.ContainsKey(data.GetType().GetGenericTypeDefinition()))
                return serilizerGenericMap[data.GetType().GetGenericTypeDefinition()](this,data);


            JToken result;
            var enumerable = data as IEnumerable;
            if (enumerable != null && false)//TODO fix Dictionary moust map to jsonobject
            {


                Type elementType = null;

                Type[] interfaces = refrenceBaseType.GetInterfaces();

                foreach (Type i in interfaces)

                    if (i.IsGenericType && i.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))

                        elementType = i.GetGenericArguments()[0];



                var jArray = new JArray(); ;
                result = jArray;


                if (refrenceBaseType.IsArray)
                    elementType = refrenceBaseType.GetElementType();
                else
                    elementType = refrenceBaseType.GetGenericArguments()[0];

                foreach (object element in enumerable)
                    jArray.Add(serilizerAll(element, elementType));
                return result;

            }
            if (refrenceBaseType.IsArray)
            {


                Type elementType = null;

                Type[] interfaces = refrenceBaseType.GetInterfaces();

                foreach (Type i in interfaces)

                    if (i.IsGenericType && i.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))

                        elementType = i.GetGenericArguments()[0];



                var jArray = new JArray(); ;
                result = jArray;


                if (refrenceBaseType.IsArray)
                    elementType = refrenceBaseType.GetElementType();
                else
                    elementType = refrenceBaseType.GetGenericArguments()[0];

                foreach (object element in enumerable)
                    jArray.Add(serilizerAll(element, elementType));
                return new JObject() { { "$type", data.GetType().ToString() }, { "value", result } };
            }
            if (refrenceBaseType.IsClass)
            {
                var dtype = data.GetType();
                while (true)
                {
                    var igs = dtype.GetCustomAttributes<Ignore>().ToList();
                    if (igs.Count() == 0)
                        break;

                    if (!igs[0].tags[(int)this.tag])
                        break;

                    dtype = dtype.BaseType;
                }
                return new JObject() { { "$type", dtype.ToString() }, { "value", serilizeChild(data, dtype) } };
            }
            if (refrenceBaseType.IsValueType && serilizerMap.ContainsKey(refrenceBaseType))//TODO refrenceBaseType or data.getType()
            {
                return serilizerMap[refrenceBaseType](this,data);
            }
            return JToken.FromObject(data);
        }
        public JToken serilizeChild(object thiz,Type myType =null)
        {
            if (thiz == null)
                return null;
            if (myType  == null)
                myType  = thiz.GetType();

            JToken res = new JObject();

            if (serilizerMap.ContainsKey(myType ))
                return serilizerMap[myType ](this,thiz);

            

            var attrs = myType.GetCustomAttributes<SetDefultIgnore>();

            bool defultIgnore = attrs.Count() > 0;
            
            var props = new List<PropertyInfo>(myType.GetProperties());


            foreach (var prop in props)
            {
                


                
                //if (prop.IsStatic)
                //    continue;
                if (defultIgnore)
                {
                    var atrs2 = prop.GetCustomAttributes<NotIgnore>(false).ToList();
                    bool notIgnore = atrs2.Count() > 0 && atrs2[0].tags[(int)this.tag];
                    if (!notIgnore)
                        continue;
                }
                else
                {
                    var atrs3 = prop.GetCustomAttributes<Ignore>(false).ToList();
                    bool ignore = atrs3.Count() > 0 && atrs3[0].tags[(int)this.tag];
                    if (ignore)
                        continue;
                }


                

                object propValue = prop.GetValue(thiz); ;
                _ = prop.Name;
                string name = getName(myType, prop);
                if (propValue != null)
                    res[name] = serilizerAll(propValue, prop.PropertyType);

            }
            return res;

        }

        public static string getName(Type c,MemberInfo f)
        {
            return f.Name;

        }
        public  object deserilizerAll(JToken data, Type refrenceBaseType)
        {
            if (deserilizerMap.ContainsKey(refrenceBaseType))
                return deserilizerMap[refrenceBaseType](this,data);
            if (refrenceBaseType == typeof(string))
                return data.ToObject(refrenceBaseType);
            if (refrenceBaseType == typeof(String))
                return data.ToObject(refrenceBaseType);
            if (refrenceBaseType.IsArray)// || refrenceBaseType.GetInterfaces().Contains(typeof(IEnumerable)))
            {

                Type elementType = null;

                Type[] interfaces = refrenceBaseType.GetInterfaces();

                foreach (Type i in interfaces)

                    if (i.IsGenericType && i.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))

                        elementType = i.GetGenericArguments()[0];

                if (refrenceBaseType.IsArray)
                    elementType = refrenceBaseType.GetElementType();
                var value2 = data["value"];
                var ar = value2.ToObject<List<JToken>>();

                IList ar2;
                if (refrenceBaseType.IsArray)
                    ar2 = Activator.CreateInstance(refrenceBaseType, new object[] { ar.Count }) as IList;
                else
                    ar2 = Activator.CreateInstance(refrenceBaseType, new object[] { ar.Count }) as IList;

                for (int i = 0; i < ar.Count; ++i)
                {
                    ar2[i]=deserilizerAll(ar[i], elementType);
                }
                return ar2;

            }
            if (refrenceBaseType.IsClass)
            {
                string typeName = (string)data["$type"];
                JToken value2 = data;
                Type mainType = refrenceBaseType;
                if (typeName != null)
                {
                    mainType = Type.GetType(typeName);
                    value2 = data["value"];
                }
                if (mainType == null)
                    mainType = refrenceBaseType;




               
                if (mainType.IsGenericType && deserilizerGenericMap.ContainsKey(mainType.GetGenericTypeDefinition()))
                {
                    ConstructorInfo ci = mainType.GetConstructor(new Type[] { });
                    var res0 = ci.Invoke(new object[] { });
                    return deserilizerGenericMap[mainType.GetGenericTypeDefinition()](this,data,res0, mainType);
                }

                var res = Activator.CreateInstance(mainType);
                return deserilizerChilds(value2, res);

            }
            if (refrenceBaseType.IsGenericType && deserilizerGenericMap.ContainsKey(refrenceBaseType.GetGenericTypeDefinition()))
            {
                object res0 = null;
                if (refrenceBaseType.IsGenericType && constructor.ContainsKey(refrenceBaseType.GetGenericTypeDefinition()))
                {
                    res0 = constructor[refrenceBaseType.GetGenericTypeDefinition()](this,refrenceBaseType);
                }
                else
                {
                    KeyValuePair<int, int> a;
                    ConstructorInfo ci = refrenceBaseType.GetConstructor(new Type[] { });
                    res0 = ci.Invoke(new object[] { });
                }
                return deserilizerGenericMap[refrenceBaseType.GetGenericTypeDefinition()](this,data, res0, refrenceBaseType);
            }




            return data.ToObject(refrenceBaseType);
        }

          public object deserilizerChilds(JToken value, object res)
        {
            var type = res.GetType();
            var props = new List<PropertyInfo>(type.GetProperties());
            var attrs=res.GetType().GetCustomAttributes<SetDefultIgnore>();
            
            bool defultIgnore = attrs.Count()>0;
            foreach (var prop in props)
            {
               
                //if (prop.IsStatic)
                //    continue;
                var atrs2 = prop.GetCustomAttributes<NotIgnore>(false).ToList();
                var atrs3 = prop.GetCustomAttributes<Ignore>(false).ToList();
                if ((atrs3.Count > 0) || (defultIgnore && atrs2.Count() == 0))
                    continue;
                string name = getName(type, prop); 
                if (value[name] == null)
                    continue;
                try
                {
                    prop.SetValue(res, deserilizerAll(value[name], prop.PropertyType));
                }
                catch
                {

                }
            }
            return res;
        }

        
        
        internal static void RegisterGenericIEnumerable()
        {
            Serilizer.serilizerGenericMap[typeof(IEnumerable<>)] = (Serilizer thiz,object data) =>
            {
                Type TValue = data.GetType().GetGenericArguments()[0];
                var d = data as IEnumerable;
                JArray res = new JArray();
                foreach (var item in d)
                    res.Add(thiz.serilizerAll(item, item.GetType()));
                return new JObject() { { "$type", data.GetType().ToString() }, { "value", res } };

            };
            Serilizer.deserilizerGenericMap[typeof(IEnumerable<>)] = (Serilizer s,JToken data,object res0, Type mainType) =>
            {
                Type[] interfaces = mainType.GetInterfaces();
                Type ItemType= mainType.GetGenericArguments()[0];
                foreach (Type i in interfaces)

                    if (i.IsGenericType && i.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))

                        ItemType = i.GetGenericArguments()[0];

                //var ItemType=mainType.GetGenericArguments()[0];
                var d = data["value"].ToObject<List<JToken>>();

                Type listGenericType = typeof(List<>);

                Type list = listGenericType.MakeGenericType(ItemType);
                
                ConstructorInfo ci = list.GetConstructor(new Type[] { });
                //var res = ci.Invoke(new object[] { }) as IList; // Activator.CreateInstance(typeof(List<>, new object[] {0 });//as IList;  new List<TValue>();
                var res = res0 as IList;
                //HashSet<int >
                foreach (var item in d)
                {
                    var ii = s.deserilizerAll(item, ItemType);
                    //var tt = Convert.ChangeType(ii, ItemType);
                    res.Add(ii);

                }
                //return new JObject() { { "$type", data.GetType().ToString() }, { "value", serilizeChild(data) } };
                return res;
            };
        }
        internal static void RegisterGenericHashSet()
        {
            Serilizer.serilizerGenericMap[typeof(HashSet<>)] = Serilizer.serilizerGenericMap[typeof(IEnumerable<>)];
            Serilizer.deserilizerGenericMap[typeof(HashSet<>)] = (Serilizer serilizer,JToken data,object res0, Type mainType) =>
            {
                var d = data["value"].ToObject<HashSet<JToken>>();

                 Type mytype = mainType.GetGenericArguments()[0];
                
                    

                    Type listGenericType = typeof(List<>);

                    Type list = listGenericType.MakeGenericType(mytype);

                    ConstructorInfo ci0 = list.GetConstructor(new Type[] { });
                    var resl = ci0.Invoke(new object[] { }) as IList; // Activator.CreateInstance(typeof(List<>, new object[] {0 });//as IList;  new List<TValue>();
                    foreach (var item in d)
                    {
                        var tt = Convert.ChangeType(serilizer.deserilizerAll(item, mytype), mytype);
                        resl.Add(tt);

                    }
                    //return new JObject() { { "$type", data.GetType().ToString() }, { "value", serilizeChild(data) } };
                    
                
                

                Type setGenericType = typeof(HashSet<>);

                Type _set = setGenericType.MakeGenericType(mytype);

                
                Type enGenericType = typeof(IEnumerable<>);
                Type _en = enGenericType.MakeGenericType(mytype);
                

                ConstructorInfo ci = _set.GetConstructor(new Type[] { _en });
                
                var res = ci.Invoke(new object[] { resl}); // Activator.CreateInstance(typeof(List<>, new object[] {0 });//as IList;  new List<TValue>();
                
                //return new JObject() { { "$type", data.GetType().ToString() }, { "value", serilizeChild(data) } };
                return res;
            };
        }
        
        static Serilizer()
        {
            //RegisterGenericList();
            
            RegisterGenericIEnumerable();
            RegisterGenericHashSet();
            Serilizer.serilizerGenericMap[typeof(List<>)] = Serilizer.serilizerGenericMap[typeof(IEnumerable<>)];
            Serilizer.deserilizerGenericMap[typeof(List<>)] = Serilizer.deserilizerGenericMap[typeof(IEnumerable<>)];
            Serilizer.serilizerGenericMap[typeof(HashSet<>)] = Serilizer.serilizerGenericMap[typeof(IEnumerable<>)];


            Serilizer.serilizerMap[typeof(JObject)]=Serilizer.serilizerMap[typeof(JValue)]=Serilizer.serilizerMap[typeof(JToken)]= (Serilizer serilizer, object data) =>
            {
                var d = data as JToken;
                return d;
            };
            Serilizer.deserilizerMap[typeof(JObject)] = Serilizer.deserilizerMap[typeof(JValue)]=Serilizer.deserilizerMap[typeof(JToken)] = (Serilizer serilizer, JToken data) =>
            {
                return data;
            };

            RegisterGenericMap();
        }
        public interface IMyPair
        {
            object getKey();
            object getValue();
        }
        public class MyPair<TKey, TValue>:IMyPair
        {
            public TKey Key;
            public TValue Value;

            public object getKey()
            {
                return Key;
            }

            public object getValue()
            {
                return Value;
            }
        }

        public static void RegisterGenericMap()
        {


            Serilizer.serilizerGenericMap[typeof(KeyValuePair<,>)] = (Serilizer serilizer, object data) =>
            {
                Type TKey = data.GetType().GetGenericArguments()[0];
                Type TValue = data.GetType().GetGenericArguments()[1];
                var d = data as IEnumerable;
                var res = new JArray();
                res.Add(0);
                res.Add(0);




                Type listGenericType = typeof(KeyValuePair<,>);

                Type keyValuePairType = listGenericType.MakeGenericType(new[] { TKey, TValue });


                
                IList<PropertyInfo> props = new List<PropertyInfo>(keyValuePairType.GetProperties());

                foreach (var prop in props)
                {
                    object propValue = prop.GetValue(data); ;
                    _ = prop.Name;

                    if(prop.Name=="Key")
                        res[0] = serilizer.serilizerAll(propValue, prop.PropertyType);
                    if (prop.Name == "Value")
                        res[1] = serilizer.serilizerAll(propValue, prop.PropertyType);

                }

                //return Serilizer.serilizerAll(data, keyValuePairType);
                return new JObject() { { "$type", data.GetType().ToString() }, { "value", res} };
            };
            Serilizer.deserilizerGenericMap[typeof(KeyValuePair<,>)] = (Serilizer serilizer, JToken data, object res0, Type mainType) =>
            {
                var d = data["value"].ToObject<List<JToken>>();


                Type listGenericType = typeof(KeyValuePair<,>);
                var TKey = mainType.GetGenericArguments()[0];
                var TValue = mainType.GetGenericArguments()[1];
                Type listType = listGenericType.MakeGenericType(new[] { TKey, TValue });


                

                

                ConstructorInfo ci = listType.GetConstructor(new Type[] { TKey, TValue });
                var res = ci.Invoke(new object[] { serilizer.deserilizerAll(d[0],TKey), serilizer.deserilizerAll(d[1], TValue) }); // Activator.CreateInstance(typeof(List<>, new object[] {0 });//as IList;  new List<TValue>();


                
               
                return res;
            };
            Serilizer.constructor[typeof(KeyValuePair<,>)] = (Serilizer serilizer, Type mainType) =>
            {
                return null;
                //ConstructorInfo ci = mainType.GetConstructor(new Type[] { mainType.GetGenericArguments()[0], mainType.GetGenericArguments()[1] });
                //return ci.Invoke(new object[] { });

                
            };


            Serilizer.serilizerGenericMap[typeof(Dictionary<,>)] = Serilizer.serilizerGenericMap[typeof(IEnumerable<>)];
            //Serilizer.deserilizerGenericMap[typeof(Dictionary<,>)] = Serilizer.deserilizerGenericMap[typeof(IEnumerable<>)];
            
            Serilizer.deserilizerGenericMap[typeof(Dictionary<,>)] = (Serilizer serilizer,JToken data,object res0,Type mainType) =>
            {
                var d = data["value"].ToObject<List<JToken>>();
                Type TKey = mainType.GetGenericArguments()[0];
                Type TValue = mainType.GetGenericArguments()[1];

                Type listGenericType = typeof(Dictionary<,>);

                Type listType = listGenericType.MakeGenericType(new[] { TKey, TValue });


                Type pairGenericType = typeof(KeyValuePair<,>);

                Type keyValuePairType = pairGenericType.MakeGenericType(new[] { TKey, TValue });

                ConstructorInfo ci = listType.GetConstructor(new Type[] { });
                var res = ci.Invoke(new object[] { }) as IDictionary; // Activator.CreateInstance(typeof(List<>, new object[] {0 });//as IList;  new List<TValue>();

                //Dictionary<>
                foreach (var item in d)
                {
                    
                        //var dd = ;
                        var key = serilizer.deserilizerAll(item["value"][0], TKey);
                        var value = serilizer.deserilizerAll(item["value"][1], TValue);
                        res.Add(key, value);
                        






                }
                return res;
            };



           
        }







}/**/

    public class BaseMsg
    {
       
        public bool ackRequire { get; set; }

        public BaseMsg()
        {
        }

    }
 
   


}
