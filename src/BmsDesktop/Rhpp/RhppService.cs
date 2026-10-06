using System.Text.Json;
using BmsDesktop.Infrastructure;
using Npgsql;

namespace BmsDesktop.Rhpp;

public sealed class RhppService
{
    private readonly Database _db;
    public RhppService(Database db)=>_db=db;

    public async Task<string> BuildSnapshotAsync(Guid cycleId)
    {
        await using var cn=await _db.OpenAsync();
        async Task<decimal> Scalar(string sql){await using var c=new NpgsqlCommand(sql,cn);c.Parameters.AddWithValue("id",cycleId);return Convert.ToDecimal(await c.ExecuteScalarAsync()??0m);}
        var cycleType=await ScalarText(cn,"select cycle_type from production_cycles where id=@id",cycleId);
        var received=await Scalar("select coalesce(received_qty,0) from doc_intakes where cycle_id=@id");
        var mortality=await Scalar("select coalesce(sum(mortality_qty+cull_qty),0) from daily_recordings where cycle_id=@id");
        var feedKg=await Scalar("select coalesce(sum(feed_used_kg),0) from daily_recordings where cycle_id=@id");
        var birds=await Scalar("select coalesce(sum(bird_qty),0) from harvests where cycle_id=@id and status='POSTED'");
        var weight=await Scalar("select coalesce(sum(total_weight_kg),0) from harvests where cycle_id=@id and status='POSTED'");
        var actualSales=await Scalar("select coalesce(sum(total_weight_kg*actual_price_per_kg),0) from harvests where cycle_id=@id and status='POSTED'");
        var bop=await Scalar("select coalesce(sum(amount),0) from barn_operational_costs where cycle_id=@id");
        var avgBw=birds>0?weight/birds:0m; var mortalityPct=received>0?mortality/received*100m:0m; var fcr=weight>0?feedKg/weight:0m;
        var data=new {cycleId,cycleType,docReceived=received,mortality,mortalityPct,feedKg,harvestBirds=birds,harvestWeightKg=weight,avgBwKg=avgBw,fcr,actualSales,bopProduction=bop,generatedAt=DateTimeOffset.UtcNow};
        return JsonSerializer.Serialize(data);
    }

    public async Task CloseCycleAsync(Guid cycleId,Guid actorId)
    {
        var json=await BuildSnapshotAsync(cycleId); await using var cn=await _db.OpenAsync();await using var tx=await cn.BeginTransactionAsync();
        var type=await ScalarText(cn,"select cycle_type from production_cycles where id=@id",cycleId,tx);
        await using(var r=new NpgsqlCommand("insert into rhpp_snapshots(id,cycle_id,rhpp_type,snapshot) values(@x,@id,@t,@s::jsonb)",cn,tx)){r.Parameters.AddWithValue("x",Guid.NewGuid());r.Parameters.AddWithValue("id",cycleId);r.Parameters.AddWithValue("t",type);r.Parameters.AddWithValue("s",json);await r.ExecuteNonQueryAsync();}
        await using(var c=new NpgsqlCommand("insert into cycle_closings(id,cycle_id,closed_by,snapshot) values(@x,@id,@u,@s::jsonb)",cn,tx)){c.Parameters.AddWithValue("x",Guid.NewGuid());c.Parameters.AddWithValue("id",cycleId);c.Parameters.AddWithValue("u",actorId);c.Parameters.AddWithValue("s",json);await c.ExecuteNonQueryAsync();}
        await using(var u=new NpgsqlCommand("update production_cycles set status='CLOSED',closed_at=now(),updated_at=now() where id=@id and status='ACTIVE'",cn,tx)){u.Parameters.AddWithValue("id",cycleId);if(await u.ExecuteNonQueryAsync()!=1)throw new InvalidOperationException("Hanya cycle ACTIVE yang dapat ditutup.");}
        await using(var b=new NpgsqlCommand("update barns set status='AVAILABLE',updated_at=now() where id=(select barn_id from production_cycles where id=@id)",cn,tx)){b.Parameters.AddWithValue("id",cycleId);await b.ExecuteNonQueryAsync();}
        await tx.CommitAsync();
    }

    private static async Task<string> ScalarText(NpgsqlConnection cn,string sql,Guid id,NpgsqlTransaction? tx=null){await using var c=new NpgsqlCommand(sql,cn,tx);c.Parameters.AddWithValue("id",id);return Convert.ToString(await c.ExecuteScalarAsync())??"";}
}
