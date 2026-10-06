using BmsDesktop.Infrastructure;
using Npgsql;

namespace BmsDesktop.Finance;

public sealed record CashRequestLine(string GroupType,Guid? BarnId,string Description,decimal Amount,string Notes);

public sealed class CashRequestService
{
    private readonly Database _db;
    public CashRequestService(Database db)=>_db=db;
    public async Task<Guid> CreateAsync(string number,DateOnly date,string subject,IEnumerable<CashRequestLine> lines,Guid actor)
    {
        var rows=lines.ToList();if(rows.Count==0||rows.Any(x=>x.Amount<0))throw new ArgumentException("Rincian pengajuan tidak valid.");
        await using var cn=await _db.OpenAsync();await using var tx=await cn.BeginTransactionAsync();var id=Guid.NewGuid();
        await using(var h=new NpgsqlCommand("insert into finance_cash_requests(id,request_number,request_date,subject,created_by) values(@id,@n,@d,@s,@u)",cn,tx)){h.Parameters.AddWithValue("id",id);h.Parameters.AddWithValue("n",number);h.Parameters.AddWithValue("d",date.ToDateTime(TimeOnly.MinValue));h.Parameters.AddWithValue("s",subject);h.Parameters.AddWithValue("u",actor);await h.ExecuteNonQueryAsync();}
        foreach(var x in rows){await using var c=new NpgsqlCommand("insert into finance_cash_request_items(id,request_id,group_type,barn_id,description,amount,notes) values(@x,@r,@g,@b,@d,@a,@n)",cn,tx);c.Parameters.AddWithValue("x",Guid.NewGuid());c.Parameters.AddWithValue("r",id);c.Parameters.AddWithValue("g",x.GroupType);c.Parameters.AddWithValue("b",(object?)x.BarnId??DBNull.Value);c.Parameters.AddWithValue("d",x.Description);c.Parameters.AddWithValue("a",x.Amount);c.Parameters.AddWithValue("n",x.Notes??"");await c.ExecuteNonQueryAsync();}
        await tx.CommitAsync();return id;
    }
}
