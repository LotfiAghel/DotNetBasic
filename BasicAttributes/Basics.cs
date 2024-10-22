using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Models
{


    public class FileString : Attribute
    {

    }

    public class SmallPicShow : FileString
    {

    }
    public class SmallVideoShow : FileString
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



    public class CUAT
    {


        //[Models.IgnoreDefultForm]
        [IgnoreDefultGird]
        [ReadOnly(true)]
        [Models.PersianLabel("تاریخ ساخت")]
        public DateTimeOffset? createdAt { get; set; }

        //[Models.IgnoreDefultForm]
        [IgnoreDefultGird]
        [ReadOnly(true)]
        [Models.PersianLabel("تاریخ اخرین تغیر")]
        public DateTimeOffset? updatedAt { get; set; }

        [IgnoreDefultGird]
        //[Models.IgnoreDefultForm]
        [ReadOnly(true)]
        [Models.PersianLabel("تاریخ حذف")]
        public DateTimeOffset? deletedAt { get; set; }
    }
    asdasd

    [ShowClassHirarci]
    public interface IIdMapper<T> where T : IEquatable<T>, IComparable<T>, IComparable
    {
        [Key]
        [PersianLabel("شناسه")]
        [Models.IgnoreDefultForm]
        public T id { get; set; }



    }

    [ShowClassHirarci]
    public class IdMapper<T> : CUAT, IIdMapper<T> where T : IEquatable<T>, IComparable<T>, IComparable
    {
        [Key]
        [PersianLabel("شناسه")]
        [ReadOnly(true)]
        public T id { get; set; }

        bool Equals(T? other)
        {
            return false;
        }
    }


    [ShowClassHirarci]
    public class OldIdMapperWithoutCUAt<T> where T : IEquatable<T>, IComparable<T>, IComparable
    {
        [Key]
        public T id { get; set; }

        bool Equals(T? other)
        {
            return false;
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
public class FFJ<T> where T : Models.IIdMapper<int>
{
    public int id;
    public FFJ(T t)
    {
        id = t.id;
    }
    public FFJ(int id)
    {
        this.id = id;
    }
}



public class FFJNew<T> where T : Models.Entity
{
    public int id;
    public FFJNew(T t)
    {
        id = t.id;
    }
    public FFJNew(int id)
    {
        this.id = id;
    }
}


