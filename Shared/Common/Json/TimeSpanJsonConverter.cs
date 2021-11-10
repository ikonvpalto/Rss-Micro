using System;
using System.Globalization;
using Common.Exceptions;
using Common.Resources;
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

            if (string.IsNullOrWhiteSpace(stringValue)
                || !TimeSpan.TryParseExact(stringValue, DateTimeFormats.TimeSpanJsonFormat,
                    CultureInfo.InvariantCulture, out var time))
            {
                throw new BadRequestException(string.Format(Localization.IncorrectTimeFormat, stringValue, DateTimeFormats.TimeSpanJsonFormat));
            }

            return time;
        }
    }
}
