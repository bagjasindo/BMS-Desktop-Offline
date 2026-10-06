using BmsDesktop.Domain;
using BmsDesktop.Infrastructure;
using Npgsql;

namespace BmsDesktop.Admin;

public sealed class MasterDataService
{
    private readonly Database _db;
    public MasterDataService(Database db)=>_db=db;

    public async Task SaveCompanyAsync(CompanyProfile p, Guid actorId)
    {
        await using var cn=await _db.OpenAsync(); await using var tx=await cn.BeginTransactionAsync();
        const string sql="update company_profile set legal_name=@a,trade_name=@b,address=@c,phone=@d,email=@e,tax_id=@f,updated_at=now() where id=1";
        await using(var cmd=new NpgsqlCommand(sql,cn,tx)){cmd.Parameters.AddWithValue("a",p.LegalName);cmd.Parameters.AddWithValue("b",p.TradeName);cmd.Parameters.AddWithValue("c",p.Address);cmd.Parameters.AddWithValue("d",p.Phone);cmd.Parameters.AddWithValue("e",p.Email);cmd.Parameters.AddWithValue("f",p.TaxId);await cmd.ExecuteNonQueryAsync();}
        await AuditAsync(cn,tx,actorId,"COMPANY_UPDATED","company_profile","1");
        await tx.CommitAsync();
    }

    public async Task<Guid> CreateBarnAsync(string code,string name,int capacity,Guid actorId)
    {
        if(string.IsNullOrWhiteSpace(code)||string.IsNullOrWhiteSpace(name)||capacity<=0) throw new ArgumentException("Data kandang tidak valid.");
        var id=Guid.NewGuid(); await using var cn=await _db.OpenAsync(); await using var tx=await cn.BeginTransactionAsync();
        await using(var cmd=new NpgsqlCommand("insert into barns(id,code,name,capacity) values(@id,@c,@n,@cap)",cn,tx)){cmd.Parameters.AddWithValue("id",id);cmd.Parameters.AddWithValue("c",code.Trim());cmd.Parameters.AddWithValue("n",name.Trim());cmd.Parameters.AddWithValue("cap",capacity);await cmd.ExecuteNonQueryAsync();}
        await AuditAsync(cn,tx,actorId,"BARN_CREATED","barns",id.ToString()); await tx.CommitAsync(); return id;
    }

    private static async Task AuditAsync(NpgsqlConnection cn,NpgsqlTransaction tx,Guid actor,string evt,string entity,string id)
    {
        await using var cmd=new NpgsqlCommand("insert into audit_log(user_id,event_type,entity_type,entity_id) values(@u,@e,@t,@i)",cn,tx);
        cmd.Parameters.AddWithValue("u",actor);cmd.Parameters.AddWithValue("e",evt);cmd.Parameters.AddWithValue("t",entity);cmd.Parameters.AddWithValue("i",id);await cmd.ExecuteNonQueryAsync();
    }
}
