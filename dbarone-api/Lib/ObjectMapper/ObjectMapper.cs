namespace dbarone_api.Lib.ObjectMapper;
using System.Reflection;

public class ObjectMapper<T, U> where T : class where U : class
{
    Type _from;
    Type _to;
    IList<PropertyMap> _map;
    IMapperStrategy _mapperStrategy = new NameMapperStrategy();

    internal ObjectMapper()
    {
        this._from = typeof(T);
        this._to = typeof(U);
        this._map = this._mapperStrategy.MapTypes(this._from, this._to);
    }

    public static ObjectMapper<T, U> Create()
    {
        return new ObjectMapper<T, U>();
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
            prop.TargetProperty.SetValue(target, sourceValue, null);
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