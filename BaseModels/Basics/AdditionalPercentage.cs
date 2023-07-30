using Newtonsoft.Json;
using System;


public readonly struct AdditionalPercentage : IComparable<AdditionalPercentage>, IEquatable<AdditionalPercentage>, IEquatable<double>
{
    /// <summary>
    /// Defines money unit.
    /// <para>
    /// Note: The official unit of currency in Iran is the Iranian rial (IR).
    /// It means the amount of the invoice will be sent to Iranian gateways automatically
    /// as <see cref="Int64"/> by Parbad.
    /// </para>
    /// </summary>
    /// <param name="value">The amount of money.</param>
    public AdditionalPercentage(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public AdditionalPercentage AddAmount(double amount)
    {
        return new AdditionalPercentage(Value + amount);
    }

    public bool Equals(AdditionalPercentage? other) => false;
    public bool Equals(AdditionalPercentage other) => Value == other.Value;

    public bool Equals(double? other) => false;
    public bool Equals(double other) => (double)Value == other;


    public override bool Equals(object obj)
    {
        var equals =
            (obj is AdditionalPercentage other && Equals(other)) ||
            (obj is int intNumber) && Equals((double)intNumber) ||
            (obj is double doubleNumber) && Equals(doubleNumber);

        return equals;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public int CompareTo(AdditionalPercentage other)
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

    public static AdditionalPercentage Parse(double amount) => new AdditionalPercentage(amount);



    /// <exception cref="Exception"></exception>
    public static AdditionalPercentage Parse(string amount)
    {
        if (!double.TryParse(amount, out var testValue))
        {
            throw new Exception($"Cannot parse {amount} to AdditionalPercentage.");
        }

        return testValue;
    }

    public static bool TryParse(string value, out AdditionalPercentage money)
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

    public static implicit operator double(AdditionalPercentage money)
    {
        return money.Value;
    }



    public static implicit operator AdditionalPercentage(double amount) => Parse(amount);



    public static bool operator >(AdditionalPercentage left, AdditionalPercentage right)
    {
        return left.Value > right.Value;
    }

    public static bool operator <(AdditionalPercentage left, AdditionalPercentage right)
    {
        return !(left > right);
    }

    public static bool operator >=(AdditionalPercentage left, AdditionalPercentage right)
    {
        return left.Value >= right.Value;
    }

    public static bool operator <=(AdditionalPercentage left, AdditionalPercentage right)
    {
        return left.Value <= right.Value;
    }

    public static AdditionalPercentage operator +(AdditionalPercentage left, AdditionalPercentage right)
    {
        return new AdditionalPercentage(left.Value + right.Value);
    }

    public static AdditionalPercentage operator -(AdditionalPercentage left, AdditionalPercentage right)
    {
        return new AdditionalPercentage(left.Value - right.Value);
    }



    public static double operator /(AdditionalPercentage left, AdditionalPercentage right)
    {
        return left.Value / right.Value;
    }
    public static AdditionalPercentage operator /(AdditionalPercentage left, int right)
    {
        return new AdditionalPercentage(left.Value / right);
    }
}


public class AdditionalPercentageConverter : JsonConverter<AdditionalPercentage>
{
    public override void WriteJson(JsonWriter writer, AdditionalPercentage value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Value);
    }

    public override AdditionalPercentage ReadJson(JsonReader reader, Type objectType, AdditionalPercentage existingValue, bool hasExistingValue, JsonSerializer serializer)
    {

        if (reader.Value is double intV)
        {
            return AdditionalPercentage.Parse(intV);
        }
        
        if (reader.Value is string stringV)
        {
            return AdditionalPercentage.Parse(stringV);
        }



        string s = (string)reader.Value;

        return AdditionalPercentage.Parse(s);
    }
}

