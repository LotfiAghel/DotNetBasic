using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
#if SERVER
using ModelsManager;
#endif
using Newtonsoft.Json;
using Tools;
public interface IForeignKey20
{
    object getFValue0();
    void setFValue0(object a);
}


public interface IForeignKey2<TKEY>: IForeignKey20
{
    //public TKEY Value { get; set; }
    public void setFValue(TKEY x);
    public TKEY getFValue();


}
public interface IForeignKey10<T>
    where T : class, Models.IEntity0
{

}
public interface IForeignKey11<T, TKEY> :  IEquatable<TKEY>, IForeignKey2<TKEY>,  IForeignKey10<T>
    where T : class, Models.IIdMapper<TKEY>
     where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
{

}

public struct ForeignKey2<T,TKEY> : IComparable<ForeignKey2<T,TKEY>>, IEquatable<TKEY>, IForeignKey2<TKEY> , IForeignKey11<T, TKEY>
    where T : class,Models.IIdMapper<TKEY>
     where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
{
    public object getFValue0()
    {
        return Value;
    }
    public ForeignKey2(TKEY value)
    {
        Value = value;
    }
    public ForeignKey2(T value)
    {
        Value = value.id;
    }
    public void setFValue(TKEY x)
    {
        Value = x;
    }

    public TKEY Value { get; set; }

#if SERVER
    public T getValue()
    {
        return default(T);
        //return IEntityManager<T>.instance.get(Value);
    }
#endif
    public bool Equals(ForeignKey2<T,TKEY>? other) => false;
    public bool Equals(ForeignKey2<T,TKEY> other) => Value.Equals(other.Value);

    public bool Equals(int? other) => false;
    public bool Equals(TKEY other) => Value .Equals(other);


    public override bool Equals(object obj)
    {
        var equals =
            (obj is ForeignKey2<T,TKEY> other && Equals(other)) ||
            (obj is int intNumber) && Equals((int)intNumber) ||
            (obj is int longNumber) && Equals(longNumber);

        return equals;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public int CompareTo(ForeignKey2<T,TKEY> other)
    {
        return Value.CompareTo(other.Value);
    }

    public override string ToString()
    {
        return "" + Value;
        //return ToString(CultureInfo.InvariantCulture);
    }

    

   

    public static ForeignKey2<T,TKEY> Parse(TKEY amount) => new ForeignKey2<T,TKEY>(amount);

    public TKEY getFValue()
    {
        return Value;
    }

    public void setFValue0(object v)
    {
        Value = (TKEY)v;
        
    }

   



    /// <exception cref="Exception"></exception>





    public static implicit operator TKEY(ForeignKey2<T,TKEY> money)
    {
        return money.Value;
    }



    public static implicit operator ForeignKey2<T,TKEY>(TKEY amount) => Parse(amount);



    public static bool operator >(ForeignKey2<T,TKEY> left, ForeignKey2<T,TKEY> right)
    {
        return left.Value.CompareTo(right.Value)<0;
    }

    public static bool operator <(ForeignKey2<T,TKEY> left, ForeignKey2<T,TKEY> right)
    {
        return !(left > right);
    }

    public static bool operator >=(ForeignKey2<T,TKEY> left, ForeignKey2<T,TKEY> right)
    {
        return left.Value.CompareTo(right.Value)>=0;
    }

    public static bool operator <=(ForeignKey2<T,TKEY> left, ForeignKey2<T,TKEY> right)
    {
        return left.Value.CompareTo(right.Value)<=0;
    }

}


public class ForeignKey2Converter0<T, TKEY> : JsonConverter<ForeignKey2<T,TKEY>> 
    where T :class, Models.IIdMapper<TKEY>
    where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
{
    public override void WriteJson(JsonWriter writer, ForeignKey2<T,TKEY> value, JsonSerializer seForeignKey2izer)
    {
        writer.WriteValue(value.ToString());
    }

    public override ForeignKey2<T,TKEY> ReadJson(JsonReader reader, Type objectType, ForeignKey2<T,TKEY> existingValue, bool hasExistingValue, JsonSerializer seForeignKey2izer)
    {
        string s = (string)reader.Value;
        if (typeof(TKEY) == typeof(string))
        {
            var tmp = new ForeignKey2<T, TKEY>();
            tmp.Value = (TKEY)((object)s);
            return tmp;
        }
        if (typeof(TKEY) == typeof(int))
        {
            var tmp = new ForeignKey2<T, TKEY>();
            tmp.Value = (TKEY)((object)(int.Parse(s)));
            return tmp;
        }
        return new ForeignKey2<T, TKEY>();
    }
}


public class ForeignKey2Converter<TKEY> : JsonConverter<IForeignKey2<TKEY>>
{
    public override void WriteJson(JsonWriter writer, IForeignKey2<TKEY> value, JsonSerializer seForeignKey2izer)
    {
        writer.WriteValue(value.getFValue());
    }

    public override IForeignKey2<TKEY> ReadJson(JsonReader reader, Type objectType, IForeignKey2<TKEY> existingValue, bool hasExistingValue, JsonSerializer seForeignKey2izer)
    {
        if (typeof(TKEY) == typeof(int))
        {

            if (reader.Value is int intV)
            {
                return objectType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { intV }) as IForeignKey2<TKEY>;
            }
            if (reader.Value is Int64 intV64)
            {
                return objectType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { (int)intV64 }) as IForeignKey2<TKEY>;
            }

            if (reader.Value is string stringV)
            {
                return objectType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { Int32.Parse(stringV) }) as IForeignKey2<TKEY>;
            }
            return objectType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { -1 }) as IForeignKey2<TKEY>;
        }
        if (typeof(TKEY) == typeof(string))
        {
            return objectType.GetConstructor(new Type[] { typeof(string) }).Invoke(new object[] { reader.Value as string }) as IForeignKey2<TKEY>;
        }
        throw new Exception($"ForeignKey2Converter<TKEY> type not handled for TKEY= {typeof(TKEY).GetName()}");

        //TODO evry new type such as GUID must added this point for suport

    }
}