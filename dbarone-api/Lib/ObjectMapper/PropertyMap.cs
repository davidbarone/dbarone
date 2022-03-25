namespace dbarone_api.Lib.ObjectMapper;
using System.Reflection;

public class PropertyMap
{
    public PropertyInfo SourceProperty { get; set; } = default!;
    public PropertyInfo TargetProperty { get; set; } = default!;
}