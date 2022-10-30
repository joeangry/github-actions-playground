using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace MyProgram;

public class DatabaseHacker
{
    private readonly ILogger<DatabaseHacker> _logger;
    private readonly string _connectionString;

    public DatabaseHacker(ILogger<DatabaseHacker> logger, string connectionString)
    {
        _logger = logger;
        _connectionString = connectionString;
    }

    public async Task<string> HackDatabaseSomehow()
    {
        _logger.LogInformation("Hacking database using connection string {Connection}", _connectionString);
        
        await using var sqlConnection = new SqlConnection(_connectionString);
        var sqlServerVerison = await sqlConnection.QueryFirstAsync<string>("SELECT @@VERSION");

        return sqlServerVerison;
    }
}