namespace dbarone_api.Data;

using Microsoft.Data.SqlClient;
using System.Data;

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
}