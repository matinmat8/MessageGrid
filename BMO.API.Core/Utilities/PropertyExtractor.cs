using System.Reflection;

namespace BMO.API.Core.Utilities;

public static class PropertyExtractor
{
    public static List<KeyValuePair<string, object>> ExtractProperties<T>(T request)
    {
        var parameters = new List<KeyValuePair<string, object>>();

        foreach (PropertyInfo property in typeof(T).GetProperties())
        {
            var propertyName = property.Name;
            var propertyValue = property.GetValue(request);
            if (propertyValue != null)
            {
                parameters.Add(new KeyValuePair<string, object>(propertyName, propertyValue));
            }
        }

        return parameters;
    }
}