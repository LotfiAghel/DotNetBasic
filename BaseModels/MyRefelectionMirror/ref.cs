using System;



using System.Collections.Generic;
using Tools;
namespace MyRefelctionMirror
{
    public class ItemInfo
    {

        public string name;
        public List<Attribute> attributes;
    }

    public class TypeInfo : ItemInfo
    {
        public static Dictionary<Type, TypeInfo> a = new Dictionary<Type, TypeInfo>();
        List<PropInfo> props;
        List<MathodInfo> methods;
        public static TypeInfo getFrom(Type tf)
        {
            var res = new TypeInfo()
            {
                props = new List<PropInfo>()
            };
            TypeInfo.a[tf] = res;
            foreach (var pr in tf.GetProperties())
            {
                if (!TypeInfo.a.ContainsKey(pr.PropertyType))
                {
                    TypeInfo.a[pr.PropertyType] = TypeInfo.getFrom(pr.PropertyType);
                }
                res.props.Add(new PropInfo()
                {
                    name = pr.Name,
                    typeId = TypeInfo.a[pr.PropertyType].name
                });

            }
            return res;
        }

    }
    public class PropInfo : ItemInfo
    {
        public string typeId;
        public static PropInfo getFrom(System.Reflection.PropertyInfo tf)
        {
            return null;
        }
    }

    public class MathodInfo : PropInfo
    {
        List<PropInfo> parms;

    }
    public class RouteInfo
    {
        public class ParmsInfo
        {
            public string name { get; set; }
            public string type { get; set; }
            public List<object> attrubites { get; set; }
        }
        public string url { get; set; }
        public List<string> methods{ get; set; }
        public List<ParmsInfo> parms { get; set; } = new List<ParmsInfo>();
        public string resultType { get; set; }


        

    }


    public class ControllerInfo
    {
        public string url { get; set; }
        public List<RouteInfo> routes { get; set; }
    }

    public class FromBodyAttribute
    {

    }
    public class FromFormAttribute
    {

    }
    public class FromQueryAttribute{

    }
    public class IFormFile{

    }

    public class TypeNameSerializationBinder2 : TypeNameSerializationBinder
    {
        public static new TypeNameSerializationBinder global;
        static TypeNameSerializationBinder2()
        {
            global = new TypeNameSerializationBinder2();
            //global.Map2<NpgsqlTypes.NpgsqlRange<DateTime>>("Range<Date>");
            global.Map2<FromBodyAttribute>("Microsoft.AspNetCore.Mvc.FromBodyAttribute");
            global.Map2<FromFormAttribute>("Microsoft.AspNetCore.Mvc.FromFormAttribute");
            global.Map2<FromQueryAttribute>("Microsoft.AspNetCore.Mvc.FromQueryAttribute");
            global.Map2<IFormFile>("Microsoft.AspNetCore.Http.IFormFile");
            
            
        }
        public TypeNameSerializationBinder2(){
            
        }
    }

}
