namespace dbarone_api.Lib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Denotes the possible CRUD operations on an entity.
/// </summary>
[Flags]
public enum CrudOperation
{
    /// <summary>
    /// No CRUD on table.
    /// </summary>
    NONE = 0,

    /// <summary>
    /// Create / INSERT allowed on entity.
    /// </summary>
    CREATE = 1,

    /// <summary>
    /// Read / SELECT allowed on entity.
    /// </summary>
    READ = 2,

    /// <summary>
    /// Update allowed on entity.
    /// </summary>
    UPDATE = 4,

    /// <summary>
    /// Delete allowed on entity.
    /// </summary> 
    DELETE = 8
}
