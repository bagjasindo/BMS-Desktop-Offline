using BmsDesktop.Infrastructure;
using Npgsql;

namespace BmsDesktop.Production;

public sealed class ProductionService
{
    private readonly Database _db;
    public ProductionService(Database db)=>_db=db;

    public async Task ChickInAsync(Guid cycleId,int delivered,int received,int doa,string strain,DateTime arrival,Guid actor)
    {
        if(delivered<0||received<0||doa<0||received>delivered) throw new ArgumentException("Data Chick-In tidak valid.");
        await using var cn=await _db.OpenAsync(); await using var tx=await cn.BeginTransactionAsync();
        await using(var cmd=new NpgsqlCommand("insert into doc_intakes(id,cycle_id,delivered_qty,received_qty,doa_qty,strain,arrival_at) values(@id,@c,@d,@r,@doa,@s,@a)",cn,tx)){cmd.Parameters.AddWithValue("id",Guid.NewGuid());cmd.Parameters.AddWithValue("c",cycleId);cmd.Parameters.AddWithValue("d",delivered);cmd.Parameters.AddWithValue("r",received);cmd.Parameters.AddWithValue("doa",doa);cmd.Parameters.AddWithValue("s",strain??"");cmd.Parameters.AddWithValue("a",arrival);await cmd.ExecuteNonQueryAsync();}
        await using(var cmd=new NpgsqlCommand("update production_cycles set chick_in_date=@d,status='ACTIVE',updated_at=now() where id=@id and status='DRAFT'",cn,tx)){cmd.Parameters.AddWithValue("d",DateOnly.FromDateTime(arrival).ToDateTime(TimeOnly.MinValue));cmd.Parameters.AddWithValue("id",cycleId);if(await cmd.ExecuteNonQueryAsync()!=1)throw new InvalidOperationException("Cycle tidak DRAFT atau tidak ditemukan.");}
        await using(var cmd=new NpgsqlCommand("update barns set status='ACTIVE',updated_at=now() where id=(select barn_id from production_cycles where id=@id)",cn,tx)){cmd.Parameters.AddWithValue("id",cycleId);await cmd.ExecuteNonQueryAsync();}
        await Audit(cn,tx,actor,"CHICK_IN","production_cycles",cycleId); await tx.CommitAsync();
    }

    public async Task RecordDailyAsync(Guid cycleId,DateOnly date,int mortality,int cull,decimal feedKg,string notes,Guid actor)
    {
        await using var cn=await _db.OpenAsync();
        await using var ageCmd=new NpgsqlCommand("select chick_in_date from production_cycles where id=@id and status='ACTIVE'",cn);ageCmd.Parameters.AddWithValue("id",cycleId);
        var raw=await ageCmd.ExecuteScalarAsync(); if(raw is not DateTime chick) throw new InvalidOperationException("Cycle belum ACTIVE/Chick-In.");
        var age=date.DayNumber-DateOnly.FromDateTime(chick).DayNumber; if(age<1) throw new InvalidOperationException("Recording Day 1 dimulai H+1 setelah Chick-In.");
        await using var cmd=new NpgsqlCommand("insert into daily_recordings(id,cycle_id,record_date,age_day,mortality_qty,cull_qty,feed_used_kg,notes) values(@id,@c,@d,@age,@m,@cu,@f,@n)",cn);
        cmd.Parameters.AddWithValue("id",Guid.NewGuid());cmd.Parameters.AddWithValue("c",cycleId);cmd.Parameters.AddWithValue("d",date.ToDateTime(TimeOnly.MinValue));cmd.Parameters.AddWithValue("age",age);cmd.Parameters.AddWithValue("m",mortality);cmd.Parameters.AddWithValue("cu",cull);cmd.Parameters.AddWithValue("f",feedKg);cmd.Parameters.AddWithValue("n",notes??"");await cmd.ExecuteNonQueryAsync();
    }
    private static async Task Audit(NpgsqlConnection cn,NpgsqlTransaction tx,Guid user,string evt,string entity,Guid id){await using var c=new NpgsqlCommand("insert into audit_log(user_id,event_type,entity_type,entity_id) values(@u,@e,@t,@i)",cn,tx);c.Parameters.AddWithValue("u",user);c.Parameters.AddWithValue("e",evt);c.Parameters.AddWithValue("t",entity);c.Parameters.AddWithValue("i",id.ToString());await c.ExecuteNonQueryAsync();}
}
