using System.Net;
using System.Net.Mail;
using CoreLibrary.Models.Setting;
using CoreLibrary.Utilities.MailSender.Entity;
using Microsoft.Extensions.Options;

namespace CoreLibrary.Utilities.MailSender;

public class MailSender : IMailSender
{
    private readonly ConfigurationValues _configurationValues;

    public MailSender(IOptions<ConfigurationValues> configurationValues)
    {
        _configurationValues = configurationValues.Value;
    }

    public async Task<bool> Send(MailMetaData parameters)
    {
        string mail = _configurationValues.SendMail.DefaultFromEmail;
        string pw = _configurationValues.SendMail.Password;
        string host = _configurationValues.SendMail.SMTPHost;
        int port = _configurationValues.SendMail.Port;

        var client = new SmtpClient(host, port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(mail, pw)
        };

        await client.SendMailAsync(new MailMessage(from: mail, to: parameters.To, parameters.Subject, parameters.Body));

        return true;
    }
}