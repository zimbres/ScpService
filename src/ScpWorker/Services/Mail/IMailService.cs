namespace ScpWorker.Services.Mail;

public interface IMailService
{
    string MailFrom { get; set; }
    string MailTo { get; set; }
    string Bcc { get; set; }
    string Subject { get; set; }
    string Body { get; set; }
    string Attachment { get; set; }
    bool IsBodyHtml { get; set; }
    string Name { get; set; }
    string Username { get; set; }
    string Password { get; set; }
    string SmtpHost { get; set; }
    int Port { get; set; }
    bool EnableSsl { get; set; }
    void SendMail();
}
