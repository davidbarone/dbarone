namespace dbarone_api.Extensions;
using System.Xml;
using System.Xml.Serialization;

public static class XmlExtensions
{

    public static string ObjectToXmlString(this object obj)
    {
        XmlSerializer xs = new XmlSerializer(obj.GetType());
        System.IO.StringWriter sw = new System.IO.StringWriter();
        XmlWriter writer = XmlWriter.Create(sw);
        xs.Serialize(writer, obj);
        var xml = sw.ToString();
        return xml;
    }

    public static T XmlStringToObject<T>(this string str)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StreamReader reader = new StreamReader(str.ToStream());
        var obj = serializer.Deserialize(reader);
        reader.Close();
        return (T)obj;
    }

    public static object XmlStringToObject(this string str, Type type)
    {
        XmlSerializer serializer = new XmlSerializer(type);
        StreamReader reader = new StreamReader(str.ToStream());
        var obj = serializer.Deserialize(reader);
        reader.Close();
        return obj;
    }
}