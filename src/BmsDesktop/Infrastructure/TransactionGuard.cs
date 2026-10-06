using Npgsql;
namespace BmsDesktop.Infrastructure;
public static class TransactionGuard { public static async Task<T> RunAsync<T>(Database db,Func<NpgsqlConnection,NpgsqlTransaction,Task<T>> work){await using var cn=await db.OpenAsync();await using var tx=await cn.BeginTransactionAsync();try{var result=await work(cn,tx);await tx.CommitAsync();return result;}catch{await tx.RollbackAsync();throw;}} }
