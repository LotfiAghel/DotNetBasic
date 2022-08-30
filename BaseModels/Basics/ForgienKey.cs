using System;
using Models;
#if SERVER
using ModelsManager;
#endif
using Newtonsoft.Json;


public interface IForeignKey
{
    public int Value { get; }
    public void setValue(int x);
}


public struct ForeignKey<T> : IComparable<ForeignKey<T>>, IEquatable<int>, IForeignKey where T : Models.IIdMapper<int>
{

    public ForeignKey(int value)
    {
        Value = value;
    }
    public ForeignKey(T value)
    {
        Value = value.id;
    }
    public void setValue(int x)
    {
        Value = x;
    }

    public int Value { get; set; }

#if SERVER
    public T getValue()
    {
        return IEntityManager<T>.instance.get(Value);
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

    public static implicit operator int(ForeignKey<T> money)
    {
        return money.Value;
    }



    public static implicit operator ForeignKey<T>(int amount) => Parse(amount);



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

}


public class ForeignKeyConverter0<T> : JsonConverter<ForeignKey<T>> where T : Models.IIdMapper<int>
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


public class ForeignKeyConverter : JsonConverter<IForeignKey>
{
    public override void WriteJson(JsonWriter writer, IForeignKey value, JsonSerializer seForeignKeyizer)
    {
        writer.WriteValue(value.Value);
    }

    public override IForeignKey ReadJson(JsonReader reader, Type objectType, IForeignKey existingValue, bool hasExistingValue, JsonSerializer seForeignKeyizer)
    {

        if (reader.Value is int intV)
        {
            return objectType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { intV }) as IForeignKey;
        }
        if (reader.Value is Int64 intV64)
        {
            return objectType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { (int)intV64 }) as IForeignKey;
        }

        if (reader.Value is string stringV)
        {
            return objectType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { Int32.Parse(stringV) }) as IForeignKey;
        }
        return objectType.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { -1 }) as IForeignKey;


    }
}




