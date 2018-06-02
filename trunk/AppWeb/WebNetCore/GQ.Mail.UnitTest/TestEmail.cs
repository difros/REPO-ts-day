using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GQ.Mail.UnitTest
{
    [TestClass]
    public class TestEMAIL
    {
        public ISMTPConfig configEmail;
        public class ConfigEmail : ISMTPConfig
        {
            public string SmtpServer { get; set; } = "smtp.gmail.com";
            public int SmtpPort { get; set; } = 587;
            public string SmtpUser { get; set; } = "geminus.qhom.test@gmail.com";
            public string SmtpPass { get; set; } = "viernesdefacturas";
            public string EmailFrom { get; set; } = "geminus.qhom.test@gmail.com";
            public bool UseSSL { get; set; } = true;
        }

        public void IniciarTest()
        {
            configEmail = new ConfigEmail();
        }

        [TestMethod]
        public void Email_Envio()
        {
            IniciarTest();

            if(!MailsSender.Send(new List<string> { "esteban.yofre@geminus-qhom.com" }, "Mail de Prueba de Envio", "HOLA MUNDO !!!", configEmail))
            {
                throw new System.Exception("Fail");
            }

        }
    }
}
