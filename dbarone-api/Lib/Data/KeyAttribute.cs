namespace dbarone_api.Lib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class KeyAttribute : Attribute
{
    public int Order { get; set; }
}
