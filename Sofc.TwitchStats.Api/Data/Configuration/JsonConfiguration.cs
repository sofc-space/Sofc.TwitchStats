using System.Text.Json;

namespace Sofc.TwitchStats.Api.Data.Configuration;

public class JsonConfiguration(JsonSerializerOptions options)
{
    public JsonSerializerOptions JsonSerializerOptions => options;
}