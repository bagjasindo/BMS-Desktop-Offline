using BmsDesktop.Domain;
using BmsDesktop.Infrastructure;
using Npgsql;
namespace BmsDesktop.Security;
public sealed class BarnScopeService { private readonly Database _db; public BarnScopeService(Database db)=>_db=db; public async Task<bool> CanAccessAsync(AppUser u,Guid barnId){if(u.Role is UserRole.ADMIN or UserRole.KEUANGAN or UserRole.OWNER)return true;if(u.Role is not (UserRole.PRODUKSI or UserRole.PPL or UserRole.LOGISTIK))return false;await using var cn=await _db.OpenAsync();await using var c=new NpgsqlCommand("select exists(select 1 from user_barn_assignments where user_id=@u and barn_id=@b)",cn);c.Parameters.AddWithValue("u",u.Id);c.Parameters.AddWithValue("b",barnId);return Convert.ToBoolean(await c.ExecuteScalarAsync());} }
