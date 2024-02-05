using CoreLibrary.Models.Setting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoreLibrary.Extensions.AppConfig;

public class FluentMailConfiguration
{
    private static ConfigurationValues? _configurationValues;

    public FluentMailConfiguration(IOptions<ConfigurationValues> configurationValues)
    {
        _configurationValues = configurationValues.Value;
    }

    public static void AddFluentEmail(IServiceCollection services)
    {
        // string defaultFromEmail = _configurationValues.FluentEmail.DefaultFromEmail;
        // string host = _configurationValues.FluentEmail.SMTPHost;
        // int port = Convert.ToInt32(_configurationValues.FluentEmail.SMTPHost);
        // string userName = _configurationValues.FluentEmail.UserName;
        // string password = _configurationValues.FluentEmail.Password;
        //
        // services.AddFluentEmail(defaultFromEmail)
        //     .AddSmtpSender(host, port, userName, password);
    }
}