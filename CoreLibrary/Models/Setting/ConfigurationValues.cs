namespace CoreLibrary.Models.Setting;

public class ConfigurationValues
{
    public Database? Database { get; set; }
    public Localization? Localization { get; set; }
    public Logging? Logging { get; set; }
    public PushNotification? PushNotification { get; set; }
    public SendMail? SendMail { get; set; }
    public Quartz? Quartz { get; set; }
    public Redis? Redis { get; set; }
    public Storage? Storage { get; set; }
    public TokenOptions? TokenOptions { get; set; }
    public Telegram? Telegram { get; set; }
    public DbType? DbType { get; set; }
}