using System;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Reflection;
using System.Globalization;
using Tools;

namespace System
{
    public static class PersianDateExtensionMethods
    {
        private static CultureInfo _Culture;
        public static CultureInfo GetPersianCulture()
        {
            if (_Culture == null)
            {
                _Culture = new CultureInfo("fa-IR");
                DateTimeFormatInfo formatInfo = _Culture.DateTimeFormat;
                formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
                formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
                var monthNames = new[]
                {
                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
                    "اسفند",
                    ""
                };
                formatInfo.AbbreviatedMonthNames =
                    formatInfo.MonthNames =
                    formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
                formatInfo.AMDesignator = "ق.ظ";
                formatInfo.PMDesignator = "ب.ظ";
                formatInfo.ShortDatePattern = "yyyy/MM/dd hh-mm-ss";
                formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy hh-mm-ss";
                formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
                System.Globalization.Calendar cal = new PersianCalendar();

                FieldInfo fieldInfo = _Culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                    fieldInfo.SetValue(_Culture, cal);

                FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (info != null)
                    info.SetValue(formatInfo, cal);

                _Culture.NumberFormat.NumberDecimalSeparator = "/";
                _Culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
                _Culture.NumberFormat.NumberNegativePattern = 0;
            }
            return _Culture;
        }

        public static string ToPersianDateString(this DateTime date, string format = "yyyy/MM/dd hh-mm-ss")
        {

            try
            {
                return date.ToString(format, GetPersianCulture());
            }
            catch
            {
                return "err";
            }
        }
        public static DateTime toPersianDate(this string iDate, string format = "yyyy/MM/dd hh-mm-ss")
        {

            iDate = iDate.Replace("۰", "0");
            iDate = iDate.Replace("۱", "1");
            iDate = iDate.Replace("۲", "2");
            iDate = iDate.Replace("۳", "3");
            iDate = iDate.Replace("۴", "4");
            iDate = iDate.Replace("۵", "5");
            iDate = iDate.Replace("۶", "6");
            iDate = iDate.Replace("۷", "7");
            iDate = iDate.Replace("۸", "8");
            iDate = iDate.Replace("۹", "9");
            return Convert.ToDateTime(iDate, GetPersianCulture());

        }
        public static string ToPersianDateString(this DateTimeOffset date, string format = "yyyy/MM/dd hh-mm-ss")
        {
            try
            {
                return date.UtcDateTime.ToString(format, GetPersianCulture());
            }
            catch
            {
                return "err";
            }
        }
    }

    public static class PrExt
    {

        public static T GetCustomFirstAttributes<T>(this MemberInfo property) where T : Attribute
        {

            var x = property.GetCustomAttributes(typeof(T), false);
            if (x != null && x.Length > 0)
                return x[0] as T;

            return null;
        }
    }

}
namespace Tools
{
  


    public static class TypeExten
    {
        public static string getBeautyName(this Type serializedType)
        {
            if (!serializedType.IsGenericType)
                return serializedType.FullName;
            string typeName = serializedType.GetGenericTypeDefinition().FullName;
            typeName = typeName.Substring(0, typeName.Length);
            typeName += "<";// + serializedType.GetGenericArguments().Length+";";
            foreach (var i in serializedType.GetGenericArguments())
                typeName += i.getBeautyName() + ";";
            typeName += ">";
            return typeName;
        }
    }
    public class TypeNameSerializationBinder0 : ISerializationBinder
    {

        public static TypeNameSerializationBinder gloabl;
        static TypeNameSerializationBinder0()
        {
            gloabl = new TypeNameSerializationBinder();
            //gloabl.Map2<NpgsqlTypes.NpgsqlRange<DateTime>>("Range<Date>");

            //gloabl.Map2<Microsoft.AspNetCore.Mvc.FromBodyAttribute>("Microsoft.AspNetCore.Mvc.FromBodyAttribute");
        }
        public TypeNameSerializationBinder0(Dictionary<Type, string> typeNames = null)
        {
            if (typeNames != null)
            {
                foreach (var typeName in typeNames)
                {
                    Map(typeName.Key, typeName.Value);
                }
            }
        }

        readonly Dictionary<Type, string> typeToName = new Dictionary<Type, string>();
        readonly Dictionary<string, Type> nameToType = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public void Map(Type type, string name)
        {
            this.typeToName.Add(type, name);
            this.nameToType.Add(name, type);
        }
        public void Map2<T>()
        {
            Map2<T>(getName(typeof(T)));

        }
        public void Map2<T>(string name)
        {
            Map(typeof(T), name);

        }


        //void BindToName(Type serializedType, out string? assemblyName, out string? typeName);
        public void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
        {

            assemblyName = null;
            typeName = getName(serializedType);


        }
        public string getName(Type serializedType)
        {
            string name;
            if (typeToName.TryGetValue(serializedType, out name))
                return name;
            name = serializedType.getBeautyName();
            return typeToName[serializedType] = name;
        }
        public Type findType(string typeName)
        {


            var x = Type.GetType(typeName, false);
            if (x == null)
            {
                var assms = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var currentAssembly in assms)
                {
                    try
                    {
                        x = currentAssembly.GetTypes().Where(x => x.getBeautyName() == typeName).FirstOrDefault();
                        if (x != null)
                            break;
                    }
                    catch
                    {
                    }
                    //var currentAssembly = typeof(GenericTypeControllerFeatureProvider).Assembly;

                }
            }
            nameToType[typeName] = x;
            return x;

        }
        public Type getTypeWithName(string typeName)
        {
            Type type;
            if (this.nameToType.TryGetValue(typeName, out type))
                return type;


            var tmp = typeName.IndexOf("<");

            if (tmp == -1)
                return nameToType[typeName] = findType(typeName);

            var gtypename = typeName.Substring(0, tmp);
            Type res = findType(gtypename);
            var gtArgs = typeName.Substring(tmp + 1, typeName.Length - tmp - 2);
            int openLt = 0;
            string par = "";
            List<Type> args = new List<Type>();
            foreach (var ch in gtArgs)
            {
                if (ch == '<')
                    ++openLt;

                if (ch == '>')
                    --openLt;
                if (ch == ';' && openLt == 0)
                {
                    var z = getTypeWithName(par);
                    args.Add(z);
                    par = "";
                }
                else
                {
                    par += ch;
                }
            }
            return nameToType[typeName] = res.MakeGenericType(args.ToArray());


        }

        public Type BindToType(string? assemblyName, string typeName)
        {
            if (assemblyName == null)
            {
                return getTypeWithName(typeName);

            }
            return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName), false);
        }
    }
    public class TypeNameSerializationBinder : ISerializationBinder
    {

        public static TypeNameSerializationBinder gloabl;
        static TypeNameSerializationBinder(){
            gloabl=new TypeNameSerializationBinder();
            //gloabl.Map2<NpgsqlTypes.NpgsqlRange<DateTime> >("Range<Date>");
            //gloabl.Map2<Microsoft.AspNetCore.Mvc.FromBodyAttribute>("Microsoft.AspNetCore.Mvc.FromBodyAttribute");
        }
        public string getName(Type serializedType)
        {
            string name;
            if (typeToName.TryGetValue(serializedType, out name))
                return name;
            name = serializedType.getBeautyName();
            return typeToName[serializedType] = name;
        }
        public TypeNameSerializationBinder(Dictionary<Type, string> typeNames = null)
        {
            if (typeNames != null)
            {
                foreach (var typeName in typeNames)
                {
                    Map(typeName.Key, typeName.Value);
                }
            }
        }

        readonly Dictionary<Type, string> typeToName = new Dictionary<Type, string>();
        readonly Dictionary<string, Type> nameToType = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public void Map(Type type, string name)
        {
            {
                this.typeToName.TryAdd(type, name);
                this.nameToType.TryAdd(name, type);
            }
            
        }
        public void Map2<T>()
        {
            Map2<T>(typeof(T).getBeautyName());

        }
        public void Map2<T>(string name)
        {
            Map(typeof(T), name);

        }


        //void BindToName(Type serializedType, out string? assemblyName, out string? typeName);
        public void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
        {
            string name;

            if (typeToName.TryGetValue(serializedType, out name))
            {
                assemblyName = null;
                typeName = name;
            }
            else
            {
                assemblyName = null;//serializedType.Assembly.FullName;
                typeName = serializedType.getBeautyName();
            }
        }
        public static string getGenericTypeName(string name)
        {
            var x = name.IndexOf("<");
            if (x < 0)
                return name;
            var name2=name.Substring(0, x);
            return name2;
        }
        public static List<string> getGenericArguments(string name)
        {
            var x = name.IndexOf("<");
            if (x < 0)
                return new List<string>();
            var name2 = name.Substring(x + 1, name.Count() - x - 2);
            return new List<string>();
        }
        public Type BindToType(string? assemblyName, string typeName)
        {
            if (assemblyName == null)
            {
                Type type;

                if (this.nameToType.TryGetValue(typeName, out type))
                {
                    return type;
                }
                else
                {
                    {
                        
                        var assms = AppDomain.CurrentDomain.GetAssemblies();
                        foreach (var currentAssembly in assms)
                        {
                            try
                            {
                                var candidates = currentAssembly.GetExportedTypes();
                                //var candidates=new List<Type>(){typeof(Dashboard)}
                                var tt = currentAssembly.GetType(typeName);
                                if (tt != null)
                                {
                                    Map(tt, tt.getBeautyName());
                                    return tt;
                                }

                            }
                            catch
                            {

                            }
                            //var currentAssembly = typeof(GenericTypeControllerFeatureProvider).Assembly;

                        }
                        //Map(null, typeName);
                        var tmp=Type.GetType(string.Format("{0}, {1}", typeName, assemblyName), false);
                        this.nameToType.Add(typeName, tmp);
                    }
                    //return Type.GetType(typeName, false);
                }
            }
            return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName), false);
        }

    }






}