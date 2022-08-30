using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Models
{


    public class SmallPicShow : Attribute
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

    public class Master : Attribute
    {


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
    public ForeignKeyAttr(Type t)
    {
        type = t;
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

