using BmsDesktop.Domain;

namespace BmsDesktop;

public sealed class MainForm : Form
{
    private readonly AppUser _user;
    public MainForm(AppUser user)
    {
        _user=user; Text="BMS Desktop Offline"; StartPosition=FormStartPosition.CenterScreen; MinimumSize=new Size(1100,700);
        var header=new Label{Text=$"BMS Desktop Offline   |   {_user.DisplayName}   |   {_user.Role}",Dock=DockStyle.Top,Height=64,Font=new Font("Segoe UI",16,FontStyle.Bold),TextAlign=ContentAlignment.MiddleLeft,Padding=new Padding(20,0,0,0)};
        var menu=new FlowLayoutPanel{Dock=DockStyle.Left,Width=230,FlowDirection=FlowDirection.TopDown,Padding=new Padding(12)};
        foreach(var name in AllowedModules(_user.Role)){var b=new Button{Text=name,Width=195,Height=42};menu.Controls.Add(b);}
        var body=new Label{Text="Dashboard BMS Desktop Offline",Dock=DockStyle.Fill,Font=new Font("Segoe UI",18),TextAlign=ContentAlignment.MiddleCenter};
        Controls.Add(body);Controls.Add(menu);Controls.Add(header);
    }
    private static IEnumerable<string> AllowedModules(UserRole role) => role switch
    {
        UserRole.ADMIN => ["Dashboard","Administrator","Master Data","Produksi/PPL","Logistik","Marketing","Keuangan","RHPP","Audit"],
        UserRole.PRODUKSI or UserRole.PPL => ["Dashboard","Produksi/PPL","RHPP"],
        UserRole.LOGISTIK => ["Dashboard","Logistik"],
        UserRole.MARKETING => ["Dashboard","Marketing","Panen"],
        UserRole.KEUANGAN => ["Dashboard","Keuangan","Laporan"],
        UserRole.OWNER => ["Dashboard","Laporan"],
        UserRole.PELANGGAN => ["Dashboard","Transaksi Saya"],
        _ => ["Dashboard"]
    };
}
