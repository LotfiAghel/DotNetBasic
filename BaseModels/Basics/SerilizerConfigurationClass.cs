using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Tools;

namespace Models
{
    public class CustomIgnoreTag : Attribute
    {
        public enum Kind
        {
            CLIENT = 1,

            DB = 2,
            CLUSTER = 3,
            DB_ANALYSIS = 4,
            ADMIN = 5,
            GO_CLINT = 1000,
        }
        public HashSet<Kind> kinds;
        public CustomIgnoreTag(params Kind[] args)
        {
            kinds = new HashSet<Kind>();
            foreach (var t in args)
                kinds.Add(t);
        }

    }
 
    public class MyContractResolver : DefaultContractResolver
    {

        public static MyContractResolver client = new MyContractResolver() { tag = CustomIgnoreTag.Kind.CLIENT };
        public static MyContractResolver admin = new MyContractResolver() { tag = CustomIgnoreTag.Kind.ADMIN };


        public static MyContractResolver db = new MyContractResolver() { tag = CustomIgnoreTag.Kind.DB };
        public static MyContractResolver cluster = new MyContractResolver() { tag = CustomIgnoreTag.Kind.CLUSTER };
        public static MyContractResolver db_analysis = new MyContractResolver() { tag = CustomIgnoreTag.Kind.DB_ANALYSIS };



     


       

        [CustomIgnoreTag(CustomIgnoreTag.Kind.CLIENT)]
        public CustomIgnoreTag.Kind tag;


        public IEnumerable<T> GetCustomAttributes<T>(Type type, string name) where T : Attribute
        {
            {
                var pi = type.GetProperty(name);
                if (pi != null)
                    return pi.GetCustomAttributes<T>(false);
            }
            {
                var pi = type.GetField(name);
                if (pi != null)
                    return pi.GetCustomAttributes<T>(false);
            }
            return null;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {

            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
            var pp = type.GetProperties();
            foreach (var prop in props)
            {

                var pi = type.GetProperty(prop.UnderlyingName);
                
                prop.Ignored = false;
                {
                    var aa =pi.GetCustomAttributes<CustomIgnoreTag>();

                    if (aa != null && aa.ToList().Count > 0)
                    {
                        if (aa.ToList()[0].kinds.Contains(tag))
                            prop.Ignored = true;
                    }
                    {
                        var bb = pi.GetCustomAttributes<JsonIgnoreAttribute>();
                        if (bb != null && bb.Count() > 0)
                        {
                            prop.Ignored = true;
                        }
                    }
                }
                {
                    var aa = pi.GetCustomAttributes<JsonIgnoreAttribute>();
                    if (aa != null && aa.ToList().Count > 0)
                    {
                        prop.Ignored = true;
                    }
                }
                {
                    if(pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition()==typeof(ICollection<>))
                    {
                        prop.Ignored = true;
                    }
                }
                {
                    List<Attribute> bb;
                    if (!ForeignKeyAttr.fpropertis.TryGetValue(pi, out bb))
                        ForeignKeyAttr.cacl(type);
                }
                {
                    if (pi.PropertyType.IsClass)
                    {
                        List<Attribute> bb;
                        if (!ForeignKeyAttr.fpropertis.TryGetValue(pi, out bb))
                            ForeignKeyAttr.cacl(type);
                        var aa = ForeignKeyAttr.fpropertis[pi].OfType<ForeignKeyAttr>();
                        if (aa != null && aa.ToList().Count > 0)
                        {
                            prop.Ignored = true;
                        }
                    }
                }

                prop.Converter = null;
                prop.PropertyName = prop.UnderlyingName;
               

            }

            return props;
        }
    }

    public class MyContractResolver2 : MyContractResolver
    {

        public static MyContractResolver2 client2 = new MyContractResolver2() { tag = CustomIgnoreTag.Kind.CLIENT };
        




        
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {

            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
            var pp = type.GetProperties();
            foreach (var prop in props)
            {
                //prop.PropertyName = prop.UnderlyingName;
                prop.PropertyName = Char.ToLowerInvariant(prop.PropertyName[0]) + prop.PropertyName.Substring(1);
            }

            return props;
        }
    }

}
