using BmsDesktop.Infrastructure;
using Npgsql;

namespace BmsDesktop.Finance;

public sealed record GlobalProfitLoss(decimal BarnOperatingProfit,decimal BarnMaintenance,decimal GlobalOffice,decimal GlobalOutside,decimal GlobalProfit);

public sealed class ProfitLossService
{
    private readonly Database _db;
    public ProfitLossService(Database db)=>_db=db;

    public async Task<GlobalProfitLoss> GetGlobalAsync()
    {
        await using var cn=await _db.OpenAsync();
        async Task<decimal> Q(string sql){await using var c=new NpgsqlCommand(sql,cn);return Convert.ToDecimal(await c.ExecuteScalarAsync()??0m);}
        var sales=await Q("select coalesce(sum(total_weight_kg*actual_price_per_kg),0) from harvests where status='POSTED'");
        var bop=await Q("select coalesce(sum(amount),0) from barn_operational_costs");
        var barnProfit=sales-bop;
        var maintenance=await Q("select coalesce(sum(amount),0) from barn_maintenance_costs");
        var office=await Q("select coalesce(sum(amount),0) from global_expenses where expense_scope='KANTOR'");
        var outside=await Q("select coalesce(sum(amount),0) from global_expenses where expense_scope='LUAR_KANTOR'");
        return new GlobalProfitLoss(barnProfit,maintenance,office,outside,barnProfit-maintenance-office-outside);
    }
}
