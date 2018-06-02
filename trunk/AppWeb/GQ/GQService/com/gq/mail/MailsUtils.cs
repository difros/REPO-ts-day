using System;
using System.Collections.Generic;
using MimeKit;
using MailKit;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace GQService.com.gq.mail
{
    public static class MailsUtils
    {
        public static bool EnviarMail(List<String> to, string subject, String body, ISMTPConfig config)
        {
            return EnviarMail(to, null, null, subject, body, config);
        }
        public static bool EnviarMail(List<String> to, List<String> cc, List<String> co, string subject, String body, ISMTPConfig config)
        {

            try
            {
                #region Creación de Mensaje Mail

                var msg = new MimeMessage();
                msg.From.Add(new MailboxAddress(config.NombreFrom, config.EMailFrom));

                if (to != null) foreach (var item in to) { msg.To.Add(new MailboxAddress(item, item)); }

                if (cc != null) foreach (var item in cc) { msg.Cc.Add(new MailboxAddress(item, item)); }

                if (co != null) foreach (var item in co) { msg.Bcc.Add(new MailboxAddress(item, item)); }

                msg.Subject = subject;
                msg.Body = new TextPart("html") { Text = body };

                #endregion

                #region Creación de Cliente SMTP y Envío

                using (var client = new SmtpClient(new ProtocolLogger("smtp.log")))
                {
                    client.Connect(config.Host, config.Port, SecureSocketOptions.SslOnConnect);

                    client.Authenticate(config.EMailFrom, config.Pass);

                    client.Send(msg);

                    client.Disconnect(true);
                }

                #endregion
            }
            catch (Exception ex)
            {
                var a = ex; //The handshake failed due to an unexpected packet format.
                return false;
            }

            return true;

        }
    }
}
