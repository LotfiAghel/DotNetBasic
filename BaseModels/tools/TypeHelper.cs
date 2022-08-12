using System;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Reflection;
using System.Globalization;
namespace Tools
{

    public static class TypeHelper
    {

        public static Dictionary<Type, string> names = new Dictionary<Type, string>() { { typeof(string), "string" }, { typeof(List<>), "List" } };
        public static List<T> SelectTypeOf<T>(this List<object> inp) where T : class
        {
            if (inp == null)
                return null;
            var res = new List<T>();
            foreach (var t in inp)
                if (t is T)
                {
                    res.Add(t as T);
                }
            return res;


        }
        public static string GetPerisanName(this Type t)
        {


            {
                var displayName = t
              .GetCustomAttributes(typeof(Models.PersianLabel), false)
              .FirstOrDefault() as Models.PersianLabel;


                if (displayName != null)
                    return displayName.txt;
            }
            return t.Name;
        }
        public static string GetPerisanName(this PropertyInfo t)
        {


            {
                var displayName = t
              .GetCustomAttributes(typeof(Models.PersianLabel), false)
              .FirstOrDefault() as Models.PersianLabel;


                if (displayName != null)
                    return displayName.txt;
            }
            return t.Name;
        }
        public static string GetPerisanName(this MethodInfo t)
        {


            {
                var displayName = t
              .GetCustomAttributes(typeof(Models.PersianLabel), false)
              .FirstOrDefault() as Models.PersianLabel;


                if (displayName != null)
                    return displayName.txt;
            }
            return t.Name;
        }
        public static string GetDocPerisanName(this Type t)
        {
            {
                var displayName = t

              .GetCustomAttributes(typeof(Models.PersianSmallDoc), false)
              .FirstOrDefault() as Models.PersianSmallDoc;
                if (displayName != null)
                    return displayName.txt;
            }

            {
                var displayName = t
              .GetCustomAttributes(typeof(Models.PersianLabel), false)
              .FirstOrDefault() as Models.PersianLabel;


                if (displayName != null)
                    return displayName.txt;
            }
            return null;
        }
        public static string GetDocPerisanName(this PropertyInfo t)
        {
            {
                var displayName = t

              .GetCustomAttributes(typeof(Models.PersianSmallDoc), true)
              .FirstOrDefault() as Models.PersianSmallDoc;
                if (displayName != null)
                    return displayName.txt;
            }

            {
                var displayName = t
              .GetCustomAttributes(typeof(Models.PersianLabel), true)
              .FirstOrDefault() as Models.PersianLabel;


                if (displayName != null)
                    return displayName.txt;
            }
            return null;
        }
        public static string GetName(this Type t)
        {
            if (t == null)
                return "null(wtf)";
            if (names.ContainsKey(t))
                return names[t];
            if (t.GenericTypeArguments.Length == 0)
            {
                var r = "/" + t.FullName;
                r = r.Replace("/System.", "/");
                r = r.Replace("/", "");
                return r;
            }

            var s = t.GetGenericTypeDefinition().GetName().Split("`")[0] + "<";
            bool isfirst = true;
            foreach (var tmp in t.GenericTypeArguments)
            {
                if (!isfirst)
                    s += "|";
                isfirst = false;
                s += tmp.GetName();
            }
            s.Replace("+", ".");

            return s + ">";
        }
        public static string getKindName(this Type t)
        {
            if (t.IsPrimitive)
                return "Primitive";
            if (t.IsInterface)
                return "interface";
            if (t.IsAbstract)
                return "abstract";
            if (t.IsClass)
                return "class";
            if (t.IsEnum)
                return "enum";

            return "";

        }
        public static List<Type> GetSubClasses(this Type T, bool evenAbstracts = true)
        {
            List<Type> classess = new List<Type>();
            var assms = AppDomain.CurrentDomain.GetAssemblies();
            if (!T.IsAbstract)
                classess.Add(T);
            foreach (var currentAssembly in assms)
            {
                try
                {
                    //var currentAssembly = typeof(GenericTypeControllerFeatureProvider).Assembly;
                    var candidates = currentAssembly.GetExportedTypes().Where(x => x.IsAssignableTo(T));

                    foreach (var candidate in candidates)
                        if (candidate.IsClass && (!candidate.IsAbstract || evenAbstracts))
                            classess.Add(candidate);
                }
                catch
                {

                }


            }
            return classess;
        }
    }

}
