namespace dbarone_api.Lib.ObjectMapper;

public interface IMapperStrategy
{
    public IList<PropertyMap> MapTypes(Type T, Type U);
}