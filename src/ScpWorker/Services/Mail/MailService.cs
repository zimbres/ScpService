using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace ScpWorker.Services.Mail;

public class MailService : MailSettings, IMailService
{
    private readonly IConfiguration _configuration;
    private readonly RetryPolicy _retryPolicy = Policy.Handle<Exception>()
        .WaitAndRetry(3, retryCount => TimeSpan.FromSeconds(Math.Pow(2, retryCount)));

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
        var mailSettings = _configuration.GetSection(nameof(MailSettings)).Get<MailSettings>();

        MailFrom = mailSettings.MailFrom;
        MailTo = mailSettings.MailTo;
        Bcc = mailSettings.Bcc;
        Subject = mailSettings.Subject;
        Body = mailSettings.Body;
        Attachment = mailSettings.Attachment;
        IsBodyHtml = mailSettings.IsBodyHtml;
        Name = mailSettings.Name;
        Username = mailSettings.Username;
        Password = mailSettings.Password;
        SmtpHost = mailSettings.SmtpHost;
        Port = mailSettings.Port;
        EnableSsl = mailSettings.EnableSsl;
    }

    public void SendMail()
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(Name, MailFrom));
        message.To.Add(new MailboxAddress(MailTo, MailTo));
        message.Subject = Subject;
        message.Body = new TextPart("html")
        {
            Text = Body
        };

        var smtpClient = new SmtpClient();
        try
        {
            smtpClient.Connect(SmtpHost, Port, EnableSsl ? SecureSocketOptions.Auto : SecureSocketOptions.None);
            if (Username != "")
            {
                smtpClient.Authenticate(Username, Password);
            }
            _retryPolicy.Execute(() => smtpClient.Send(message));
        }
        catch (Exception)
        {
            return;
        }
        finally
        {
            smtpClient.Disconnect(true);
        }
    }
}
