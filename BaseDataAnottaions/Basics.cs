using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{


    public class SmallPicShow : Attribute
    {

    }
    public class RichTextAttribute : Attribute
    {

    }
    public class SmallVideoShow : Attribute
    {

    }
    public class IgnoreDefultGird : Attribute
    {
        public bool isIgnore = true;
        public IgnoreDefultGird(bool isIgnore=true) {
            this.isIgnore = isIgnore;
        }
    }

    public class IgnoreDefultForm : Attribute
    {
        public bool ignoreDefultForm { get; set; }
        public IgnoreDefultForm(bool ignoreDefultForm=true) {
            this.ignoreDefultForm = ignoreDefultForm;
        }
    }

    public class StarRateShow : Attribute
    {
        public int x;
        public StarRateShow(int x)
        {
            this.x = x;
        }
    }
    public class PersianLabel : Attribute
    {
        public string txt;
        public PersianLabel(string x)
        {
            this.txt = x;
        }
    }
    public class PersianSmallDoc : Attribute
    {
        public string txt;
        public PersianSmallDoc(string x)
        {
            this.txt = x;
        }
    }

    
    public class PersianBigDoc : Attribute
    {
        public string txt;
        public PersianBigDoc(string x)
        {
            this.txt = x;
        }
    }



    


}

public class MDPropInfo
{
    public PropertyInfo propertyInfo;
    public List<Attribute> attrs=new();
    public MDPropInfo(PropertyInfo pr)
    {
        this.propertyInfo = pr;
    }
}
public class MDTypeInfo
{
    public static Dictionary<Type, MDTypeInfo> mp = new();
    public Type type;
    public List<Attribute> attrs=new();
    public Dictionary<PropertyInfo, MDPropInfo> pattrs=new();
    public MDTypeInfo(Type t)
    {
        type = t;
        foreach (var x in t.GetProperties())
            this.pattrs[x] = new MDPropInfo(x);
    }
    public static MDTypeInfo get(Type t)
    {
        MDTypeInfo z;
        if (!mp.TryGetValue(t, out z))
            mp[t] = z = new MDTypeInfo(t);
        return z;
    }

}

public class ForeignKeyAttr : Attribute
{
    public Type type;
    public Type tkey = typeof(int);
    public static Dictionary<System.Reflection.PropertyInfo, List<Attribute>> fpropertis = new();
    public ForeignKeyAttr(Type t)
    {
        type = t;
        var p = t.GetProperty("id");
        if (p == null)
        {
            Console.WriteLine("ff");
        }
        this.tkey=p.PropertyType;
    }
    public Type[] getTypes()
    {
        return new Type[]{ type,tkey};
    }
    public static void  cacl(Type type)
    {
         List<System.Reflection.PropertyInfo> propertis = type.GetProperties(
                          BindingFlags.Public |
                          BindingFlags.NonPublic |
                          BindingFlags.Instance).ToList();
        foreach (var pr in propertis)
        {
            var s = pr.GetCustomAttribute<ForeignKeyAttribute>();
            if (!ForeignKeyAttr.fpropertis.ContainsKey(pr))
                ForeignKeyAttr.fpropertis.Add(pr, new List<Attribute>());

            if (s != null) {
                var rv = propertis.Find(x => x.Name == s.Name);
                if (!ForeignKeyAttr.fpropertis.ContainsKey(rv))
                    ForeignKeyAttr.fpropertis.Add(rv, new List<Attribute>());
                ForeignKeyAttr.fpropertis[rv].Add(new ForeignKeyAttribute(pr.Name));
                ForeignKeyAttr.fpropertis[pr].Add(new ForeignKeyAttribute(rv.Name));
                if (pr.PropertyType.IsClass && pr.PropertyType!=typeof(String) && pr.PropertyType != typeof(string) && pr.PropertyType != typeof(Guid))
                {
                    ForeignKeyAttr.fpropertis[rv].Add(new ForeignKeyAttr(pr.PropertyType));
                }
                else
                {
                    ForeignKeyAttr.fpropertis[pr].Add(new ForeignKeyAttr(rv.PropertyType));
                }
            }
            

        }
    }
}



public class CollectionAttr : Attribute
{
    public Type type;
    public object[] data;
    public CollectionAttr(Type type)
    {
        this.type = type;
    }
    public CollectionAttr(Type type, params object[] args)
    {
        this.type = type;
        data = args;
    }
}
public class MultiSelect:Attribute{
    
}


public class GridShow:Attribute{
    
}

