using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Tools;

namespace SGSStandalone.Core.ClientMessage
{

    public class GossipMsg
    {
    }

    public class DataContainer
    {
        public object body;
        public DataContainer(object body)
        {
            this.body = body;
        }


        public static DataContainer Deserialize(string data)
        {
            var js = JToken.Parse(data);
            return js.ToObject<DataContainer>(DataContainer2.clientSerilizer);
        }

        
        public string Serialize()
        {
            return JToken.FromObject(this, DataContainer2.clientSerilizer).ToString();
            JTokenWriter writer = new JTokenWriter();
            DataContainer2.clientSerilizer.Serialize(writer, body, body.GetType());
            var j = new JObject() { { "protocolVersion", 0 }, { "metaVersion", 0 }, { "body", writer.Token } };
            return j.ToString();
        }





    }
    public static class JTokenExtenction
    {
        public static JToken FromObject<T>(T body2, JsonSerializer clientSerilizer)
        {
            using (JTokenWriter jTokenWriter = new JTokenWriter())
            {
                DataContainer2.clientSerilizer.Serialize(jTokenWriter, body2, typeof(T));
                return jTokenWriter.Token;
            }
        }
    }
    public class DataContainer2
    {
        public int protocolVersion;
        public int metaVersion;

        public string _sessionId;
        public int msgId;
        public int ackId;
        public JToken body;
        public static JsonSerializerSettings settings;
        public static JsonSerializer clientSerilizer;
        static DataContainer2()
        {
            settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                SerializationBinder = TypeNameSerializationBinder.gloabl,
                ContractResolver = MyContractResolver.client,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
            settings.Converters.Add(new ForeignKeyConverter());
            

            clientSerilizer = JsonSerializer.Create(settings);

        }

        public DataContainer2(JToken body)
        {
            this.body = body;
        }
        public DataContainer2()
        {
        }
        public DataContainer2(BaseMsg body2)
        {
            this.body = JTokenExtenction.FromObject<BaseMsg>(body2, DataContainer2.clientSerilizer);
        }

        public static DataContainer2 Deserialize(string data)
        {
            //Debug.LogWarning("come string " + data);
            var js = JToken.Parse(data);
            var res = new DataContainer2()
            {
                protocolVersion = js["protocolVersion"].ToObject<int>(),
                metaVersion = js["metaVersion"].ToObject<int>(),
                //protocolVersion = js["protocolVersion"].ToObject<int>(),
                msgId = js["msgId"].ToObject<int>(),

                _sessionId = js["_sessionId"].ToObject<string>(),
                ackId = js["ackId"].ToObject<int>(),
                body = js["body"],
            };
            return res;

        }

        public string Serialize()
        {
            var j = new JObject() { { "protocolVersion", 0 }, { "metaVersion", 0 }, { "_sessionId", _sessionId }, { "msgId", msgId }, { "ackId", ackId }, { "body", body } };
            return j.ToString();
        }
    }


}