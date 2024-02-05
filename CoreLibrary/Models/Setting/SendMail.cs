namespace CoreLibrary.Models.Setting;

public class SendMail
{
    public string DefaultFromEmail { get; set; }
    public string SMTPHost { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}