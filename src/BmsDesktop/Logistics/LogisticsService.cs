using BmsDesktop.Infrastructure;
using Npgsql;

namespace BmsDesktop.Logistics;

public sealed class LogisticsService
{
    private readonly Database _db;
    public LogisticsService(Database db)=>_db=db;

    public async Task<Guid> ReceiveAsync(Guid? poId,string number,DateOnly date,IEnumerable<(Guid itemId,Guid? cycleId,decimal qty)> items,Guid actorId)
    {
        var rows=items.ToList(); if(rows.Count==0||rows.Any(x=>x.qty<=0)) throw new ArgumentException("Penerimaan barang tidak valid.");
        await using var cn=await _db.OpenAsync(); await using var tx=await cn.BeginTransactionAsync(); var id=Guid.NewGuid();
        await using(var cmd=new NpgsqlCommand("insert into goods_receipts(id,purchase_order_id,receipt_number,receipt_date) values(@id,@po,@no,@dt)",cn,tx)){cmd.Parameters.AddWithValue("id",id);cmd.Parameters.AddWithValue("po",(object?)poId??DBNull.Value);cmd.Parameters.AddWithValue("no",number);cmd.Parameters.AddWithValue("dt",date.ToDateTime(TimeOnly.MinValue));await cmd.ExecuteNonQueryAsync();}
        foreach(var x in rows){var ri=Guid.NewGuid();await using(var cmd=new NpgsqlCommand("insert into goods_receipt_items(id,goods_receipt_id,item_id,cycle_id,qty) values(@id,@r,@i,@c,@q)",cn,tx)){cmd.Parameters.AddWithValue("id",ri);cmd.Parameters.AddWithValue("r",id);cmd.Parameters.AddWithValue("i",x.itemId);cmd.Parameters.AddWithValue("c",(object?)x.cycleId??DBNull.Value);cmd.Parameters.AddWithValue("q",x.qty);await cmd.ExecuteNonQueryAsync();}await using(var sm=new NpgsqlCommand("insert into stock_movements(item_id,cycle_id,movement_type,qty,reference_type,reference_id) values(@i,@c,'RECEIPT',@q,'goods_receipts',@r)",cn,tx)){sm.Parameters.AddWithValue("i",x.itemId);sm.Parameters.AddWithValue("c",(object?)x.cycleId??DBNull.Value);sm.Parameters.AddWithValue("q",x.qty);sm.Parameters.AddWithValue("r",id.ToString());await sm.ExecuteNonQueryAsync();}}
        await Audit(cn,tx,actorId,"GOODS_RECEIVED","goods_receipts",id); await tx.CommitAsync(); return id;
    }
    private static async Task Audit(NpgsqlConnection cn,NpgsqlTransaction tx,Guid user,string evt,string entity,Guid id){await using var c=new NpgsqlCommand("insert into audit_log(user_id,event_type,entity_type,entity_id) values(@u,@e,@t,@i)",cn,tx);c.Parameters.AddWithValue("u",user);c.Parameters.AddWithValue("e",evt);c.Parameters.AddWithValue("t",entity);c.Parameters.AddWithValue("i",id.ToString());await c.ExecuteNonQueryAsync();}
}
