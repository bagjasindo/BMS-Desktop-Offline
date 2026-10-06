using System.Text.Json;

namespace BmsDesktop.Infrastructure;

public sealed class AppConfig
{
    public DatabaseConfig Database { get; set; } = new();
    public static AppConfig Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(path)) throw new InvalidOperationException("appsettings.json belum tersedia. Salin dari appsettings.example.json lalu isi konfigurasi server lokal.");
        return JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(path), new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? throw new InvalidOperationException("Konfigurasi aplikasi tidak valid.");
    }
}
public sealed class DatabaseConfig
{
    public string Host { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 5432;
    public string Database { get; set; } = "bms_desktop";
    public string Username { get; set; } = "bms_app";
    public string Password { get; set; } = "";
    public string ConnectionString => $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};Pooling=true;Timeout=5;Command Timeout=30";
}
