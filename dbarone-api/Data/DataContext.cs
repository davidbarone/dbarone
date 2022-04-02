namespace dbarone_api.Data;

using Microsoft.Data.SqlClient;
using dbarone_api.Lib.Data;
using System.Collections;
using dbarone_api.Entities;

public class DataContext : IDataContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    public DataContext(IConfiguration configuration)
    {
        var csBuilder = new SqlConnectionStringBuilder(configuration["ConnectionStrings:dbarone"]);
        csBuilder.Password = configuration["DatabasePassword"]; // user-secret
        this._connectionString = csBuilder.ConnectionString;
    }

    public T Create<T>()
    {
        return Session.CreateEntity<T>();
    }

    public void Execute(string sql, object? param = null)
    {
        using var session = Session.Create(this._connectionString);
        session.Execute(sql, param);
    }

    public IEnumerable<T> Query<T>(string sql, object? param = null)
    {
        using var session = Session.Create(this._connectionString);
        return session.Query<T>(sql, param);
    }

    public IEnumerable<Hashtable> Query(string sql, object param = null)
    {
        using var session = Session.Create(this._connectionString);
        return session.Query(sql, param);
    }

    public IEnumerable<T> Read<T>(object? filter = null)
    {
        using var session = Session.Create(this._connectionString);
        return session.Read<T>(filter);
    }

    public T? Find<T>(params object[] id)
    {
        using var session = Session.Create(this._connectionString);
        return session.Find<T>(id);
    }

    public T Single<T>(params object[] id)
    {
        using var session = Session.Create(this._connectionString);
        return session.Single<T>(id);
    }

    public object[] Insert<T>(T entity)
    {
        using var session = Session.Create(this._connectionString);
        return session.Insert<T>(entity);
    }

    public object[] Update<T>(T entity)
    {
        using var session = Session.Create(this._connectionString);
        return session.Update<T>(entity);
    }

    public void Delete<T>(params object[] key)
    {
        using var session = Session.Create(this._connectionString);
        var entity = session.Single<T>(key);
        session.Delete<T>(entity);
    }
}