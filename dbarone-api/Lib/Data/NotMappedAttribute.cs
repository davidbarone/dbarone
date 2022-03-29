namespace dbarone_api.Lib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Use this attribute to exclude a member from the data mapping process.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class NotMappedAttribute : Attribute
{
}

