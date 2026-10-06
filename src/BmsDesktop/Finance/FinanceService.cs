using BmsDesktop.Infrastructure;
using Npgsql;

namespace BmsDesktop.Finance;

public sealed class FinanceService
{
    private readonly Database _db;
    public FinanceService(Database db)=>_db=db;
    public async Task<Guid> AddBarnOperationalCostAsync(Guid cycleId,DateOnly date,string description,decimal amount)
    {
        if(amount<0||string.IsNullOrWhiteSpace(description))throw new ArgumentException("BOP Produksi tidak valid.");
        await using var cn=await _db.OpenAsync();var id=Guid.NewGuid();await using var cmd=new NpgsqlCommand("insert into barn_operational_costs(id,cycle_id,expense_date,description,amount) values(@id,@c,@d,@x,@a)",cn);
        cmd.Parameters.AddWithValue("id",id);cmd.Parameters.AddWithValue("c",cycleId);cmd.Parameters.AddWithValue("d",date.ToDateTime(TimeOnly.MinValue));cmd.Parameters.AddWithValue("x",description);cmd.Parameters.AddWithValue("a",amount);await cmd.ExecuteNonQueryAsync();return id;
    }
    public async Task<Guid> AddBarnMaintenanceAsync(Guid barnId,DateOnly date,string description,decimal amount)
    {
        if(amount<0||string.IsNullOrWhiteSpace(description))throw new ArgumentException("Perawatan kandang tidak valid.");
        await using var cn=await _db.OpenAsync();var id=Guid.NewGuid();await using var cmd=new NpgsqlCommand("insert into barn_maintenance_costs(id,barn_id,expense_date,description,amount) values(@id,@b,@d,@x,@a)",cn);
        cmd.Parameters.AddWithValue("id",id);cmd.Parameters.AddWithValue("b",barnId);cmd.Parameters.AddWithValue("d",date.ToDateTime(TimeOnly.MinValue));cmd.Parameters.AddWithValue("x",description);cmd.Parameters.AddWithValue("a",amount);await cmd.ExecuteNonQueryAsync();return id;
    }
}
