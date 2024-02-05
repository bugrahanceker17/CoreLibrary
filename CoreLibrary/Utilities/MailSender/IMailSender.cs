using CoreLibrary.Utilities.MailSender.Entity;

namespace CoreLibrary.Utilities.MailSender;

public interface IMailSender
{
    Task<bool> Send(MailMetaData parameters);
}