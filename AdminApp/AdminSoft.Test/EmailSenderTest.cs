using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLNFramework.Mailing;

namespace AdminSoft.Test
{
    [TestClass]
    public class EmailSenderTest
    {
        [TestMethod]
        public void SendEmailTest()
        {
            IEmail email = new Email();
            email.Tos = new string[] { "rmay@plenumsoft.com.mx" };
            email.Subject = "Prueba de correo";
            email.Body = "Esta un una prueba para el envio de correo.";

            /*La configuración del servidor de correo se encuentra en el webconfig */
            ISmtpEmailServerAppConfiguration config = new SmtpEmailServerAppConfiguration();
            IEmailServerConfiguration server = new SmtpEmailServerConfiguration(config);
            IEmailSender sender = new SmtpEmailSender(server);

            var send = sender.SendEmail(email);

            Assert.IsTrue(send);
        }
    }
}
