using System;
using Models;
#if SERVER
using ModelsManager;
#endif
using Newtonsoft.Json;




public struct ForeignKey<T> : IComparable<ForeignKey<T>>, IEquatable<int>, IForeignKey2<int>,  IForeignKey10<T>
    where T : class, Models.IIdMapper<int>
{

    public object getFValue0()
    {
        return Value;
    }
    public ForeignKey(int value)
    {
        Value = value;
    }
    public ForeignKey(T value)
    {
        Value = value.id;
    }
    public void setFValue(int x)
    {
        Value = x;
    }

    public int Value { get; set; }

#if SERVER
    public T getValue()
    {
        return IEntityManager<T, int>.instance.get(Value);
    }
#endif
    public bool Equals(ForeignKey<T>? other) => false;
    public bool Equals(ForeignKey<T> other) => Value == other.Value;

    public bool Equals(int? other) => false;
    public bool Equals(int other) => (int)Value == other;


    public override bool Equals(object obj)
    {
        var equals =
            (obj is ForeignKey<T> other && Equals(other)) ||
            (obj is int intNumber) && Equals((int)intNumber) ||
            (obj is int longNumber) && Equals(longNumber);

        return equals;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public int CompareTo(ForeignKey<T> other)
    {
        return Value.CompareTo(other.Value);
    }

    public override string ToString()
    {
        return "" + Value;
        //return ToString(CultureInfo.InvariantCulture);
    }

    public string ToString(IFormatProvider format)
    {
        return Value.ToString(format);
    }

    public string ToString(string format)
    {
        return Value.ToString(format);
    }

    public static ForeignKey<T> Parse(int amount) => new ForeignKey<T>(amount);



    /// <exception cref="Exception"></exception>
    public static ForeignKey<T> Parse(string amount)
    {
        if (!int.TryParse(amount, out var testValue))
        {
            throw new Exception($"Cannot parse {amount} to ForeignKey.");
        }

        return new ForeignKey<T>(testValue);
    }

    public static bool TryParse(string value, out ForeignKey<T> money)
    {
        try
        {
            money = Parse(value);
            return true;
        }
        catch
        {
            money = default;
            return false;
        }
    }

    public int getFValue()
    {
        return Value;
    }

    public void setFValue0(object a)
    {
        Value = (int)a;
    }

    public static implicit operator int(ForeignKey<T> money)
    {
        return money.Value;
    }


    public ForeignKey(ForeignKey2<T, int> value)
    {
        Value = value.Value;
    }

    public static implicit operator ForeignKey<T>(int amount) => Parse(amount);
    public static implicit operator ForeignKey<T>(T vl)
    {
        return Parse(vl.id);
    }
    public static implicit operator ForeignKey<T>(ForeignKey2<T, int> amount)
    {
        return Parse(amount.Value);
    }
    public static implicit operator ForeignKey2<T,int>(ForeignKey<T> amount)
    {
        return new ForeignKey2<T, int>(amount.Value);
    }

    public static bool operator >(ForeignKey<T> left, ForeignKey<T> right)
    {
        return left.Value > right.Value;
    }

    public static bool operator <(ForeignKey<T> left, ForeignKey<T> right)
    {
        return !(left > right);
    }

    public static bool operator >=(ForeignKey<T> left, ForeignKey<T> right)
    {
        return left.Value >= right.Value;
    }

    public static bool operator <=(ForeignKey<T> left, ForeignKey<T> right)
    {
        return left.Value <= right.Value;
    }
    public static explicit operator ForeignKey<T>(Int64 b) => new ForeignKey<T>((int)b);
}


public class ForeignKeyConverter0<T> : JsonConverter<ForeignKey<T>> 
    where T :class, Models.IIdMapper<int>
{
    public override void WriteJson(JsonWriter writer, ForeignKey<T> value, JsonSerializer seForeignKeyizer)
    {
        writer.WriteValue(value.ToString());
    }

    public override ForeignKey<T> ReadJson(JsonReader reader, Type objectType, ForeignKey<T> existingValue, bool hasExistingValue, JsonSerializer seForeignKeyizer)
    {
        string s = (string)reader.Value;

        return ForeignKey<T>.Parse(s);
    }
}


public class ForeignKeyConverter : JsonConverter<IForeignKey2<int>>
{
    public override void WriteJson(JsonWriter writer, IForeignKey2<int> value, JsonSerializer seForeignKeyizer)
    {
        writer.WriteValue(value.getFValue());
    }

    public override IForeignKey2<int> ReadJson(JsonReader reader, Type objectType, IForeignKey2<int> existingValue, bool hasExistingValue, JsonSerializer seForeignKeyizer)
    {

        if (reader.Value is int intV)
        {
            return objectType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { intV }) as IForeignKey2<int>;
        }
        if (reader.Value is Int64 intV64)
        {
            return objectType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { (int)intV64 }) as IForeignKey2<int>;
        }

        if (reader.Value is string stringV)
        {
            return objectType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { Int32.Parse(stringV) }) as IForeignKey2<int>;
        }
        return objectType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { -1 }) as IForeignKey2<int>;


    }
}




public class ForeignKeyConverter2 : JsonConverter<IForeignKey2<Guid>>
{
    public override void WriteJson(JsonWriter writer, IForeignKey2<Guid> value, JsonSerializer seForeignKeyizer)
    {
        writer.WriteValue(value.getFValue());
    }

    public override IForeignKey2<Guid> ReadJson(JsonReader reader, Type objectType, IForeignKey2<Guid> existingValue, bool hasExistingValue, JsonSerializer seForeignKeyizer)
    {
        
        int t = 0;
       while(reader.TokenType != JsonToken.String)
        {
            if (reader.TokenType == JsonToken.StartObject)
                ++t;
            reader.Read();
        }
        var rV=reader.Value;
        
        while(t>0)
        {
            reader.Read();
            if (reader.TokenType == JsonToken.EndObject)
                --t;
        }
        if (rV is string stringV)
        {
            if (stringV == "Value")
            {
                
            }
            return objectType.GetConstructor(new Type[] { typeof(Guid) }).Invoke(new object[] { Guid.Parse(stringV) }) as IForeignKey2<Guid>;
        }
        return objectType.GetConstructor(new Type[] { typeof(Guid) }).Invoke(new object[] { Guid.Empty }) as IForeignKey2<Guid>;


    }
}
public class ForeignKeyConverter3 : JsonConverter<IForeignKey2<string>>
{
    public override void WriteJson(JsonWriter writer, IForeignKey2<string> value, JsonSerializer seForeignKeyizer)
    {
        writer.WriteValue(value.getFValue());
    }

    public override IForeignKey2<string> ReadJson(JsonReader reader, Type objectType, IForeignKey2<string> existingValue, bool hasExistingValue, JsonSerializer seForeignKeyizer)
    {


        if (reader.Value is string stringV)
        {
            return objectType.GetConstructor(new Type[] { typeof(string) }).Invoke(new object[] { stringV }) as IForeignKey2<string>;
        }
        return objectType.GetConstructor(new Type[] { typeof(string) }).Invoke(new object[] { null }) as IForeignKey2<string>;


    }
}

public class ForeignKeyConverter2<T> : JsonConverter<IForeignKey2<T>>
{
    public override void WriteJson(JsonWriter writer, IForeignKey2<T> value, JsonSerializer seForeignKeyizer)
    {
        writer.WriteValue(value.getFValue());
    }

    public override IForeignKey2<T> ReadJson(JsonReader reader, Type objectType, IForeignKey2<T> existingValue, bool hasExistingValue, JsonSerializer seForeignKeyizer)
    {

        if (reader.Value is int intV)
        {
            return objectType.GetConstructor(new Type[] { typeof(T) }).Invoke(new object[] { intV }) as IForeignKey2<T>;
        }
        if (reader.Value is Int64 intV64)
        {
            return objectType.GetConstructor(new Type[] { typeof(T) }).Invoke(new object[] { (int)intV64 }) as IForeignKey2<T>;
        }

        if (reader.Value is string stringV)
        {
            return objectType.GetConstructor(new Type[] { typeof(T) }).Invoke(new object[] { Int32.Parse(stringV) }) as IForeignKey2<T>;
        }
        return objectType.GetConstructor(new Type[] { typeof(T) }).Invoke(new object[] { default(T) }) as IForeignKey2<T>;


    }
}


