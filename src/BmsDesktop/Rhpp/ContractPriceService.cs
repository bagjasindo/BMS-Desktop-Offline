using BmsDesktop.Infrastructure;
using Npgsql;
namespace BmsDesktop.Rhpp;
public sealed class ContractPriceService { private readonly Database _db; public ContractPriceService(Database db)=>_db=db; public async Task<decimal?> GetBwPriceAsync(Guid cycleId,decimal avgBwKg){await using var cn=await _db.OpenAsync();await using var c=new NpgsqlCommand("select cbp.price_per_kg from production_cycles pc join contract_bw_prices cbp on cbp.contract_id=pc.contract_id where pc.id=@id and @bw between cbp.min_bw and cbp.max_bw order by cbp.min_bw desc limit 1",cn);c.Parameters.AddWithValue("id",cycleId);c.Parameters.AddWithValue("bw",avgBwKg);var v=await c.ExecuteScalarAsync();return v is null||v is DBNull?null:Convert.ToDecimal(v);} }
