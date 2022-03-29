namespace dbarone_api.Lib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Use this attribute to denote a .NET member should be mapped for READ operations, but not for WRITE operations.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class ReadOnlyAttribute : Attribute
{
}
