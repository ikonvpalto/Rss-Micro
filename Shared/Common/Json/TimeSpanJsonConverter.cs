using System;
using Newtonsoft.Json;

namespace Common.Json
{
    public sealed class TimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(DateTimeFormats.TimeSpanJsonFormat));
        }

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var stringValue = (string)reader.Value;
            return string.IsNullOrWhiteSpace(stringValue)
                ? TimeSpan.Zero
                : TimeSpan.Parse(stringValue);
        }
    }
}
