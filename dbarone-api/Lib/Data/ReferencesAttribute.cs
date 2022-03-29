namespace dbarone_api.Lib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// For columns that act as foreign key fields and reference another object.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class ReferencesAttribute : Attribute
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="referencedType"></param>
    public ReferencesAttribute(Type referencedType)
    {
        this.ReferencedType = referencedType;
    }

    /// <summary>
    /// The referenced type.
    /// </summary>
    public Type ReferencedType { get; set; }
}
