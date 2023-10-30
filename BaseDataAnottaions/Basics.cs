﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;

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
public class ForeignKeyAttr : Attribute
{
    public Type type;
    public Type tkey = typeof(int);
    public static Dictionary<System.Reflection.PropertyInfo, List<Attribute>> fpropertis = new();
    public ForeignKeyAttr(Type t)
    {
        type = t;
        this.tkey=t.GetProperty("id").PropertyType;
    }
    public Type[] getTypes()
    {
        return new Type[]{ type,tkey};
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

