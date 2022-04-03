namespace dbarone_api.Lib.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbarone_api.Extensions;

public class Hydrater
{
    public Hashtable GetHashTable(IDataReader dataReader)
    {
        var values = new Hashtable(StringComparer.InvariantCultureIgnoreCase);

        for (int i = 0; i < dataReader.FieldCount; i++)
        {
            var name = dataReader.GetName(i);
            var value = dataReader[name];
            /*
            var type = dataReader.GetFieldType(i);
            if (type == typeof(byte[]))
            {
                var size = (int)dataReader.GetBytes(i, 0, null, 0, 0);
                byte[] buffer = new byte[size];
                dataReader.GetBytes(i, 0, buffer, 0, size);
                value = buffer;
            }
            */
            values.Add(name, value);
        }

        return values;
    }

    public T GetEntity<T>(IDataReader reader)
    {
        return CreateEntityFromValues<T>(GetHashTable(reader));
    }

    private T CreateEntityFromValues<T>(Hashtable values)
    {
        var entity = Activator.CreateInstance<T>();
        Hydrate(entity, values);
        return entity;
    }

    private void Hydrate<T>(T entity, Hashtable values)
    {
        var tableInfo = MetaDataStore.GetTableInfoFor<T>();

        var properties = typeof(T).GetProperties();
        foreach (var propertyInfo in properties)
        {
            if (propertyInfo.CanWrite)
            {
                var columnInfo = tableInfo.GetColumn(propertyInfo);
                if (columnInfo != null)
                {
                    object value = values[columnInfo.Name];
                    if (value is DBNull) value = null;

                    // If enum column, convert database value to enum
                    if (propertyInfo.PropertyType.IsEnum)
                    {
                        value = Enum.Parse(propertyInfo.PropertyType, value.ToString());
                    }
                    if (value != null)
                    {
                        var pt = propertyInfo.PropertyType;
                        if (pt.IsNullable())
                        {
                            pt = Nullable.GetUnderlyingType(pt);
                        }
                        value = Convert.ChangeType(value, pt);
                    }
                    propertyInfo.SetValue(entity, value, null);
                }
            }
        }
    }
}
