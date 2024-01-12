using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaskFighter.Infrastructure.Persistence;

public class TodoTaskStatusJsonConverter : JsonConverter
{
    private readonly Type[] _types;

    public TodoTaskStatusJsonConverter(params Type[] types)
    {
        _types = types;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        JToken t = JToken.FromObject(value);

        if (t.Type != JTokenType.Object)
        {
            t.WriteTo(writer);
        }
        else
        {
            JObject o = (JObject)t;
            IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

            o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

            o.WriteTo(writer);
        }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        var taskStatus = new TodoTaskStatus { Value = obj["Value"].ToString() };
        return taskStatus;
    }

    public override bool CanRead
    {
        get { return true; }
    }
    public override bool CanWrite
    {
        get { return false; }
    }

    public override bool CanConvert(Type objectType)
    {
        return _types.Any(t => t == objectType);
    }
}