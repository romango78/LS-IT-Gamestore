using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Gamestore.DataProvider.Steam.Extensions;

internal sealed class UnixEpochDateTimeConverter : JsonConverter<DateTime>
{
    static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0);
    static readonly Regex Regex = new("^/Date\\(([+-]*\\d+)\\)/$", RegexOptions.CultureInvariant);

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                var unixTime = reader.GetInt64();
                return ConvertUnixEpochDate(unixTime);
            case JsonTokenType.String:
                var unixTimeString = reader.GetString()!;
                return ConvertUnixEpochDate(unixTimeString);
            default:
                throw new JsonException();
        }
    }
    
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        long unixTime = Convert.ToInt64((value - Epoch).TotalMilliseconds);

        string formatted = string.Create(CultureInfo.InvariantCulture, $"/Date({unixTime})/");
        writer.WriteStringValue(formatted);
    }

    private DateTime ConvertUnixEpochDate(long unixTime)
    {
        return Epoch.AddMilliseconds(unixTime);
    }

    private DateTime ConvertUnixEpochDate(string unixTime)
    {
        Match match = Regex.Match(unixTime);

        if (
            !match.Success
            || !long.TryParse(match.Groups[1].Value, System.Globalization.NumberStyles.Integer,
                CultureInfo.InvariantCulture, out long unixTimeLong))
        {
            throw new JsonException();
        }

        return Epoch.AddMilliseconds(unixTimeLong);
    }

}
