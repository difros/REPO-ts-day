using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace GQ.Mail
{
    public static class MailsSender
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool Send(List<String> to, string subject, String body, ISMTPConfig config)
        {
            return Send(to, null, null, subject, body, config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="co"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool Send(List<String> to, List<String> cc, List<String> co, string subject, String body, ISMTPConfig config)
        {
            #region Creación de Mensaje Mail
            MailMessage msg = new MailMessage
            {
                From = new MailAddress(config.EmailFrom),
                IsBodyHtml = true,
                Subject = subject,
                Body = body,
                Priority = MailPriority.Normal,
                BodyEncoding = System.Text.Encoding.UTF8
            };

            var _to = msg.To;
            if (to != null)
            {
                foreach (var item in to)
                {
                    _to.Add(new MailAddress(item));
                }
            }

            var _cc = msg.CC;
            if (cc != null)
            {
                foreach (var item in cc)
                {
                    _cc.Add(new MailAddress(item));
                }
            }

            var _co = msg.Bcc;
            if (co != null)
            {
                foreach (var item in co)
                {
                    _co.Add(new MailAddress(item));
                }
            }
            #endregion

            #region Creación de Cliente SMTP
            var client = new SmtpClient
            {
                Host = config.SmtpServer,
                Port = config.SmtpPort,
                EnableSsl = config.UseSSL
            };

            if (config.SmtpUser != null && config.SmtpPass != null)
            {
                client.Credentials = new NetworkCredential(config.SmtpUser, config.SmtpPass);
            }
            #endregion

            #region Envío de Mail
            return Send(msg, client);
            #endregion

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private static bool Send(MailMessage msg, SmtpClient client)
        {
            try
            {
                client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                Log.Log.GetLog().Error("GQ.Mail.MailsSender.Send", ex);
                return false;
            }
        }
    }
}
