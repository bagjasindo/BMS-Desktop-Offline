using Npgsql;

namespace BmsDesktop.Infrastructure;

public sealed class Database
{
    private readonly string _connectionString;
    public Database(string connectionString) => _connectionString = connectionString;
    public async Task<NpgsqlConnection> OpenAsync(CancellationToken cancellationToken = default)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
    public async Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
    {
        try { await using var connection = await OpenAsync(cancellationToken); return true; }
        catch { return false; }
    }
}
