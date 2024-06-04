using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace Brp.AutorisatieEnProtocollering.Proxy.Helpers;

public static class JObjectHelpers
{

    public static IEnumerable<(string Name, string[] Value)> BepaalElementNrVanZoekParameters(this JObject input, ReadOnlyDictionary<string, string> FieldElementNrDictionary)
    {
        return from property in input.Properties()
               where !new[] { "type", "fields", "inclusiefOverledenPersonen" }.Contains(property.Name)
               select (property.Name, Value: FieldElementNrDictionary[property.Name].Split(' '));
    }
}