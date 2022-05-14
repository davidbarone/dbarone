namespace dbarone_api.Lib.ObjectMapper;
using dbarone_api.Extensions;

public class NameMapperStrategy : IMapperStrategy
{
    public IList<PropertyMap> MapTypes(Type T, Type U)
    {
        var sourceProperties = T.GetProperties();
        var targetProperties = U.GetProperties();

        var properties = (from s in sourceProperties
                          from t in targetProperties
                          where s.Name == t.Name &&
                              s.CanRead &&
                              t.CanWrite &&
                              s.PropertyType.IsPublic &&
                              t.PropertyType.IsPublic &&
                              (s.PropertyType == t.PropertyType ||
                              s.PropertyType.GetElementType() == t.PropertyType.GetElementType()) &&
                              (
                                  (s.PropertyType.IsValueType &&
                                  t.PropertyType.IsValueType
                                  ) ||
                                  (s.PropertyType == typeof(string) ||
                                  t.PropertyType == typeof(string)
                                  ) ||
                                  (
                                      // source or target is nullable type
                                      (s.PropertyType.IsNullable() && s.PropertyType.GetNullableUnderlyingType()==t.PropertyType) ||
                                      (t.PropertyType.IsNullable() && t.PropertyType.GetNullableUnderlyingType()==s.PropertyType)
                                  )
                              )
                          select new PropertyMap
                          {
                              SourceProperty = s,
                              TargetProperty = t
                          }).ToList();

        return properties;
    }
}