using Newtonsoft.Json;
using System;



public interface IPerimitveContainer
{
    object getValue();
}
public class PerimitveContainer<T> : IPerimitveContainer
{
    public T value { get; set; }
    public PerimitveContainer(T v)
    {
        value = v;
    }
    public PerimitveContainer()
    {
        value = default(T);
    }
    public object getValue()
    {
        return value;
    }
}


public class PerimitveContainerConvertor : Newtonsoft.Json.JsonConverter<PerimitveContainer<int>>
{
    public override void WriteJson(JsonWriter writer, PerimitveContainer<int> value, JsonSerializer serializer)
    {
        writer.WriteValue(value.value);
    }

    public override PerimitveContainer<int> ReadJson(JsonReader reader, Type objectType, PerimitveContainer<int> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {

        string s = (string)reader.Value;

        return new PerimitveContainer<int>(Int32.Parse(s));
    }
}