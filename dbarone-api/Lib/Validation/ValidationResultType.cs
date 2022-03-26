namespace dbarone_api.Lib.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Flags]
public enum ValidationResultType
{
    ERROR,
    WARNING,
    INFORMATION
}
