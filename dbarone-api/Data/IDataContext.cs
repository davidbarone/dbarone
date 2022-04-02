namespace dbarone_api.Data;
using System.Data;
using System.Collections;

public interface IDataContext
{
    void Execute(string sql, object? param = null);
    IEnumerable<T> Query<T>(string sql, object? param = null);
    IEnumerable<Hashtable> Query(string sql, object param = null);
    IEnumerable<T> Read<T>(object? filter = null);
    T? Find<T>(params object[] id);
    T Single<T>(params object[] id);
    object[] Insert<T>(T entity);
    object[] Update<T>(T entity);
    void Delete<T>(object[] key);
}