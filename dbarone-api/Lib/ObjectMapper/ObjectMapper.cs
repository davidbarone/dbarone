namespace dbarone_api.Lib.ObjectMapper;
using System.Reflection;
using dbarone_api.Extensions;

public class ObjectMapper<T, U> where T : class where U : class
{
    Type _from;
    Type _to;
    IList<PropertyMap> _map;
    IMapperStrategy _mapperStrategy = new NameMapperStrategy();

    internal ObjectMapper(IMapperStrategy mapperStrategy = null)
    {
        if (mapperStrategy == null)
        {
            mapperStrategy = new NameMapperStrategy();
        }
        _mapperStrategy = mapperStrategy;
        this._from = typeof(T);
        this._to = typeof(U);
        this._map = this._mapperStrategy.MapTypes(this._from, this._to);
    }

    public static ObjectMapper<T, U> Create(IMapperStrategy mapperStrategy = null)
    {
        return new ObjectMapper<T, U>(mapperStrategy);
    }

    public U? MapOne(T? obj)
    {
        if (obj == null)
            return null;

        U target = (U)Activator.CreateInstance(typeof(U))!;

        for (var i = 0; i < _map.Count(); i++)
        {
            var prop = _map[i];
            var sourceValue = prop.SourceProperty.GetValue(obj, null);

            if (sourceValue == null)
            {
                // null value
                prop.TargetProperty.SetValue(target, null);
            }
            else if (prop.TargetProperty.PropertyType == prop.SourceProperty.PropertyType)
            {
                // source & target same type
                prop.TargetProperty.SetValue(target, sourceValue, null);
            }
            else if (prop.TargetProperty.PropertyType == typeof(string))
            {
                // target is string - do ToString()
                prop.TargetProperty.SetValue(target, sourceValue?.ToString(), null);
            }
            else if (prop.TargetProperty.PropertyType.IsEnum && prop.SourceProperty.PropertyType == typeof(string))
            {
                // source is string & target is enum
                prop.TargetProperty.SetValue(target, Enum.Parse(prop.TargetProperty.PropertyType, (String)sourceValue), null);
            } else if (prop.SourceProperty.PropertyType.IsNullable() && prop.SourceProperty.PropertyType.GetNullableUnderlyingType()==prop.TargetProperty.PropertyType)
            {
                if (sourceValue==null){
                    prop.TargetProperty.SetValue(target, prop.TargetProperty.PropertyType.Default());
                } else {
                    prop.TargetProperty.SetValue(target, sourceValue, null);
                }
            }
        }
        return target;
    }

    public IEnumerable<U> MapMany(IEnumerable<T> obj)
    {
        foreach (var item in obj)
        {
            yield return MapOne(item);
        }
    }


}