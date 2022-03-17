namespace dbarone_api.Data;

using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    public DapperContext(IConfiguration configuration)
    {
        var csBuilder = new SqlConnectionStringBuilder(configuration["ConnectionStrings:dbarone"]);
        csBuilder.Password = configuration["DatabasePassword"]; // user-secret
        this._connectionString = csBuilder.ConnectionString;
    }
    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);


    public void Execute(string sql, object? param = null)
    {
        // C# 8.0 implied using declarations
        using var db = this.CreateConnection();
        db.Execute(sql, param);
    }

    public IEnumerable<T>? Query<T>(string sql, object? param = null)
    {
        // C# 8.0 implied using declarations
        using var db = this.CreateConnection();
        var items = db.Query<T>(sql, param);
        return items;
    }
}