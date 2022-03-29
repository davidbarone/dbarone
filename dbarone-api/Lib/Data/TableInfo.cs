namespace dbarone_api.Lib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public class TableInfo
{
    /// <summary>
    /// Name of the table.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Entity type bound to table.
    /// </summary>
    public Type EntityType { get; set; }

    /// <summary>
    /// Columns for the table (dictionary)
    /// </summary>
    private Dictionary<string, ColumnInfo> columns = null;

    /// <summary>
    /// Read only array of columns.
    /// </summary>
    public IEnumerable<ColumnInfo> Columns
    {
        get
        {
            return columns != null ? columns.Values : null;
        }
    }

    /// <summary>
    /// Adds a column to the metadata.
    /// </summary>
    /// <param name="column"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddColumn(ColumnInfo column)
    {
        if (columns == null)
        {
            columns = new Dictionary<string, ColumnInfo>();
        }
        if (columns.ContainsKey(column.Name))
        {
            throw new InvalidOperationException(string.Format("An item with key {0} has already been added", column.Name));
        }

        columns.Add(column.Name, column);
    }

    /// <summary>
    /// Gets a column by PropertyInfo.
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    public ColumnInfo GetColumn(PropertyInfo propertyInfo)
    {
        return columns.Values.FirstOrDefault(c => c.PropertyInfo == propertyInfo);
    }

    public void RemoveColumn(PropertyInfo propertyInfo)
    {
        string? keyToDelete = null;
        foreach (var key in columns.Keys)
        {
            if (columns[key].PropertyInfo == propertyInfo)
            {
                keyToDelete = key;
            }
        }
        if (keyToDelete != null)
        {
            columns.Remove(keyToDelete);
        }
    }

    /// <summary>
    /// Gets a column by name.
    /// </summary>
    /// <param name="columnName"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ColumnInfo GetColumn(string columnName)
    {
        if (!columns.ContainsKey(columnName))
        {
            throw new InvalidOperationException(string.Format("The entity type {0} does not have a {1} column", EntityType, columnName));
        }

        return columns[columnName];
    }
}
