using BmsDesktop.Infrastructure;
using BmsDesktop.Security;

namespace BmsDesktop;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        try
        {
            var config = AppConfig.Load();
            var db = new Database(config.Database.ConnectionString);
            Application.Run(new LoginForm(new AuthService(db)));
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "BMS Desktop Offline", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
