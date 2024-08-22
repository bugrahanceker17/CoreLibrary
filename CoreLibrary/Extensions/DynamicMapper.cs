using System.Dynamic;
using System.Reflection;

namespace CoreLibrary.Extensions;

public static class DynamicMapper
{
    public static T MapToType<T>(dynamic source) where T : new()
    {
        var destination = new T();
        var destinationType = typeof(T);

        var sourceProperties = GetDynamicProperties(source);

        foreach (var kvp in sourceProperties)
        {
            var destProp = destinationType.GetProperty(kvp.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (destProp != null && destProp.CanWrite)
            {
                var value = kvp.Value;
                var convertedValue = Convert.ChangeType(value, destProp.PropertyType);
                destProp.SetValue(destination, convertedValue);
            }
        }

        return destination;
    }
    
    private static IDictionary<string, object> GetDynamicProperties(dynamic obj)
    {
        var properties = new Dictionary<string, object>();

        if (obj is ExpandoObject expando)
        {
            var expandoDict = (IDictionary<string, object>)expando;
            foreach (var kvp in expandoDict)
            {
                properties[kvp.Key] = kvp.Value;
            }
        }
        else
        {
            var objType = obj.GetType();
            foreach (var prop in objType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                properties[prop.Name] = prop.GetValue(obj);
            }
        }

        return properties;
    }
}