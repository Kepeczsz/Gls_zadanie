

using Newtonsoft.Json.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Gls_Etykiety.Extensions;

public static class CustomJsonExtensions
{
    public static Guid? ExtractIdFromJson(string jsonString)
    {
        var jsonObject = JObject.Parse(jsonString);

        string id = (string) jsonObject.Property("id");
        Guid result;
        if (Guid.TryParse(id, out result))
        {
            return result;
        }

        throw new Exception(message: "Invalid id was passed");
    }
}
