using System;
using Newtonsoft.Json;

public readonly struct Rial : IComparable<Rial>, IEquatable<Rial>, IEquatable<long>
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
        public Rial(long value)
        {
            Value = value;
        }

        public long Value { get; }

        public Rial AddAmount(long amount)
        {
            return new Rial(Value + amount);
        }

        public bool Equals(Rial? other) => false;
        public bool Equals(Rial other) => Value == other.Value;

        public bool Equals(long? other) => false;
        public bool Equals(long other) => (long)Value == other;

        
        public override bool Equals(object obj)
        {
            var equals =
                (obj is Rial other && Equals(other)) ||
                (obj is int intNumber) && Equals((long)intNumber) ||
                (obj is long longNumber) && Equals(longNumber) ;

            return equals;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public int CompareTo(Rial other)
        {
            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return ""+Value;
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

        public static Rial Parse(long amount) => new Rial(amount);

        

        /// <exception cref="Exception"></exception>
        public static Rial Parse(string amount)
        {
            if (!long.TryParse(amount, out var testValue))
            {
                throw new Exception($"Cannot parse {amount} to Rial.");
            }

            return testValue;
        }

        public static bool TryParse(string value, out Rial money)
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

        public static implicit operator long(Rial money)
        {
            return money.Value;
        }

        

        public static implicit operator Rial(long amount) => Parse(amount);

        

        public static bool operator >(Rial left, Rial right)
        {
            return left.Value > right.Value;
        }

        public static bool operator <(Rial left, Rial right)
        {
            return !(left > right);
        }

        public static bool operator >=(Rial left, Rial right)
        {
            return left.Value >= right.Value;
        }

        public static bool operator <=(Rial left, Rial right)
        {
            return left.Value <= right.Value;
        }

        public static Rial operator +(Rial left, Rial right)
        {
            return new Rial(left.Value + right.Value);
        }

        public static Rial operator -(Rial left, Rial right)
        {
            return new Rial(left.Value - right.Value);
        }

       

        public static long operator /(Rial left, Rial right)
        {
            return left.Value / right.Value;
        }
        public static Rial operator /(Rial left, int right)
        {
            return new Rial(left.Value / right);
        }
    }


public class RialConverter : JsonConverter<Rial>
{
    public override void WriteJson(JsonWriter writer, Rial value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Value);
    }

    public override Rial ReadJson(JsonReader reader, Type objectType, Rial existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        
        if(reader.Value is int intV)
        {
            return  Rial.Parse(intV);
        }
        if (reader.Value is Int64 intV64)
        {
            return  Rial.Parse(intV64);
        }

        if (reader.Value is string stringV)
        {
            return  Rial.Parse(stringV);
        }
        


        string s = (string)reader.Value;

        return  Rial.Parse(s);
    }
}
