using System;
using System.Text.Json;

namespace Bugreporter.API.Helpers;

public class JsonSerializerSettings
{
    public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
