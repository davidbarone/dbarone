namespace dbarone_api.Lib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Denotes that the property uses a sequence for autonumbering
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class SequenceAttribute : Attribute
{
    public string Name { get; set; }
}
