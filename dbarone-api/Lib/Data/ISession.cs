namespace dbarone_api.Lib.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISession : IDisposable
{
    IDbConnection Connection { get; set; }
    IDbTransaction Transaction { get; set; }
    void Commit();
    void Rollback();
    object ExecuteScalar(string sql, object param = null, CommandType commandType = CommandType.Text, int? commandTimeout = null);
    IEnumerable<T> Query<T>(string sql = null, object param = null, CommandType commandType = CommandType.Text, bool buffered = true);
    IEnumerable<Hashtable> Query(string sql, object param = null, CommandType commandType = CommandType.Text, Action<List<string>> callbackHeaders = null);
    //T CreateEntity<T>();
    IEnumerable<T> Read<T>(object? filter = null);
    T? Find<T>(params object[] keys);
    T Single<T>(params object[] keys);
    object[] Insert<T>(T? entity);
    object[] Update<T>(T? entity);
    void Delete<T>(T? entity);
    object?[] GetKeys<T>(object? obj);
    int Execute(string sql, object param = null, CommandType commandType = CommandType.Text);
    IDataReader ExecuteReader(string sql, object param = null, CommandType commandType = CommandType.Text);
    IEnumerable<Hashtable> GetSchema(string sql, object parameters = null, CommandType commandType = CommandType.Text);
    void BulkCopy(IEnumerable<Hashtable> data, Dictionary<string, Type> schema, string destination, int timeout = 60 * 60);
}
