using BmsDesktop.Domain;
using BmsDesktop.Infrastructure;
using Npgsql;

namespace BmsDesktop.Security;

public sealed class AuthService
{
    private readonly Database _db;
    public AuthService(Database db) => _db = db;

    public async Task<AppUser?> LoginAsync(string username, string password)
    {
        await using var cn = await _db.OpenAsync();
        await using var cmd = new NpgsqlCommand("select id,username,display_name,password_hash,role from app_users where lower(username)=lower(@u) and is_active=true limit 1", cn);
        cmd.Parameters.AddWithValue("u", username.Trim());
        await using var rd = await cmd.ExecuteReaderAsync();
        if (!await rd.ReadAsync() || !PasswordHasher.Verify(password, rd.GetString(3))) return null;
        if (!Enum.TryParse<UserRole>(rd.GetString(4), true, out var role)) return null;
        return new AppUser(rd.GetGuid(0), rd.GetString(1), rd.GetString(2), role);
    }
}
