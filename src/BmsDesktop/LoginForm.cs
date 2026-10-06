using BmsDesktop.Security;

namespace BmsDesktop;

public sealed class LoginForm : Form
{
    private readonly AuthService _auth;
    private readonly TextBox _username = new() { PlaceholderText = "Username", Width = 300 };
    private readonly TextBox _password = new() { PlaceholderText = "Password", Width = 300, UseSystemPasswordChar = true };
    private readonly Label _status = new() { AutoSize = true };
    public LoginForm(AuthService auth)
    {
        _auth=auth; Text="Login - BMS Desktop Offline"; StartPosition=FormStartPosition.CenterScreen; ClientSize=new Size(460,330); FormBorderStyle=FormBorderStyle.FixedDialog; MaximizeBox=false;
        var title=new Label{Text="BMS Desktop Offline",AutoSize=true,Font=new Font("Segoe UI",20,FontStyle.Bold)};
        var login=new Button{Text="Masuk",Width=300,Height=38};
        login.Click += async (_,_) => await DoLoginAsync();
        AcceptButton=login;
        var panel=new FlowLayoutPanel{FlowDirection=FlowDirection.TopDown,WrapContents=false,AutoSize=true,Location=new Point(75,55)};
        panel.Controls.AddRange([title,new Label{Text="Sistem operasional lokal/LAN",AutoSize=true},_username,_password,login,_status]); Controls.Add(panel);
    }
    private async Task DoLoginAsync()
    {
        _status.Text="Memeriksa...";
        try {
            var user=await _auth.LoginAsync(_username.Text,_password.Text);
            if(user is null){_status.Text="Username atau password salah.";return;}
            Hide(); using var main=new MainForm(user); main.ShowDialog(); Close();
        } catch(Exception ex){_status.Text="Database belum siap: "+ex.Message;}
    }
}
