namespace GQ.Mail
{
    public interface ISMTPConfig
    {
        string SmtpServer { get; set; }
        int SmtpPort { get; set; }
        string SmtpUser { get; set; }
        string SmtpPass { get; set; }
        string EmailFrom { get; set; }
        bool UseSSL { get; set; }
    }
}
