using Npgsql;
namespace BmsDesktop.Infrastructure;
public sealed record NetworkHealth(bool Connected,string Message,DateTimeOffset CheckedAt);
public sealed class NetworkHealthService { private readonly Database _db; public NetworkHealthService(Database db)=>_db=db; public async Task<NetworkHealth> CheckAsync(){try{await using var cn=await _db.OpenAsync();await using var cmd=new NpgsqlCommand("select 1",cn);await cmd.ExecuteScalarAsync();return new(true,"Server lokal terhubung",DateTimeOffset.Now);}catch(Exception ex){return new(false,"Server lokal tidak terhubung: "+ex.Message,DateTimeOffset.Now);}} }
