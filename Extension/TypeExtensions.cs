
using Core.Models.Manager.Model;

namespace Code.Models.Manager.Extension;

public static class TypeExtensions
{

    /// <summary>
    /// Compares to types to return the common properties. (Those with same name, same CanRead, same CanWritte and same PropertyType)
    /// </summary>
    /// <param name="sourceType">Source ComparingType</param>
    /// <param name="compareType">Target comparing Type</param>
    /// <returns>Enumerable with a list of matching properties.</returns>
    public static IEnumerable<ModelPropertyMap> GetCommonPropertiesMap(this Type sourceType, Type compareType)
        => from s in sourceType.GetProperties().ToList()
           from t in compareType.GetProperties().ToList()
           where s.Name == t.Name &&
               s.CanRead &&
               t.CanWrite &&
               s.PropertyType == t.PropertyType
           select new ModelPropertyMap
           {
               Source = s,
               Target = t
           };
}
