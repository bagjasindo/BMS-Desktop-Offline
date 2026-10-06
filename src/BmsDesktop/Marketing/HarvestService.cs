using BmsDesktop.Infrastructure;
using Npgsql;

namespace BmsDesktop.Marketing;

public sealed class HarvestService
{
    private readonly Database _db;
    public HarvestService(Database db)=>_db=db;
    public async Task<Guid> AddAsync(Guid cycleId,DateOnly date,Guid? buyer,int birds,decimal weight,decimal actualPrice)
    {
        if(birds<=0||weight<=0||actualPrice<0) throw new ArgumentException("Data panen tidak valid.");
        await using var cn=await _db.OpenAsync();var id=Guid.NewGuid();await using var cmd=new NpgsqlCommand("insert into harvests(id,cycle_id,harvest_date,buyer_id,bird_qty,total_weight_kg,actual_price_per_kg) values(@id,@c,@d,@b,@q,@w,@p)",cn);
        cmd.Parameters.AddWithValue("id",id);cmd.Parameters.AddWithValue("c",cycleId);cmd.Parameters.AddWithValue("d",date.ToDateTime(TimeOnly.MinValue));cmd.Parameters.AddWithValue("b",(object?)buyer??DBNull.Value);cmd.Parameters.AddWithValue("q",birds);cmd.Parameters.AddWithValue("w",weight);cmd.Parameters.AddWithValue("p",actualPrice);await cmd.ExecuteNonQueryAsync();return id;
    }
}
