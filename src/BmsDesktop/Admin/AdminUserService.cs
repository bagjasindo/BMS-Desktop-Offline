using BmsDesktop.Domain;
using BmsDesktop.Infrastructure;
using BmsDesktop.Security;
using Npgsql;

namespace BmsDesktop.Admin;

public sealed class AdminUserService
{
    private readonly Database _db;
    public AdminUserService(Database db)=>_db=db;
    public async Task CreateUserAsync(string username,string displayName,string password,UserRole role)
    {
        if(string.IsNullOrWhiteSpace(username)||string.IsNullOrWhiteSpace(displayName)||password.Length<8) throw new ArgumentException("Data akun belum valid; password minimal 8 karakter.");
        await using var cn=await _db.OpenAsync();
        await using var tx=await cn.BeginTransactionAsync();
        await using var cmd=new NpgsqlCommand("insert into app_users(id,username,display_name,password_hash,role) values(@id,@u,@n,@p,@r)",cn,tx);
        var id=Guid.NewGuid(); cmd.Parameters.AddWithValue("id",id);cmd.Parameters.AddWithValue("u",username.Trim());cmd.Parameters.AddWithValue("n",displayName.Trim());cmd.Parameters.AddWithValue("p",PasswordHasher.Hash(password));cmd.Parameters.AddWithValue("r",role.ToString());
        await cmd.ExecuteNonQueryAsync();
        await using var audit=new NpgsqlCommand("insert into audit_log(user_id,event_type,entity_type,entity_id,details) values(null,'USER_CREATED','app_users',@id,jsonb_build_object('username',@u,'role',@r))",cn,tx);
        audit.Parameters.AddWithValue("id",id.ToString());audit.Parameters.AddWithValue("u",username.Trim());audit.Parameters.AddWithValue("r",role.ToString());await audit.ExecuteNonQueryAsync();
        await tx.CommitAsync();
    }
}
