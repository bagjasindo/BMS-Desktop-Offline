namespace BmsDesktop;

public sealed class MainForm : Form
{
    public MainForm()
    {
        Text = "BMS Desktop Offline";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1000, 650);

        var title = new Label
        {
            Text = "BMS Desktop Offline",
            Dock = DockStyle.Top,
            Height = 72,
            Font = new Font("Segoe UI", 22, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var info = new Label
        {
            Text = "Fondasi aplikasi Windows siap. Tahap berikutnya: Login & Administrator.",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 12),
            TextAlign = ContentAlignment.MiddleCenter
        };

        Controls.Add(info);
        Controls.Add(title);
    }
}
