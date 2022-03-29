namespace dbarone_api.Lib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Attribute to denote an entity class.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class TableAttribute : Attribute
{
    /// <summary>
    /// The table name to link to.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name"></param>
    public TableAttribute(string name)
    {
        Name = name;
    }
}
