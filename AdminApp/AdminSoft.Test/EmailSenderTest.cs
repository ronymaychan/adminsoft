using System;
using System.Net;
using System.Net.Mail;
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
            email.Tos = new string[] { "ronymaychan@gmail.com" };
            email.Subject = "Prueba de correo";
            email.Body = "Esta un una prueba para el envio de correo.";

            /*La configuración del servidor de correo se encuentra en el webconfig */
            ISmtpEmailServerAppConfiguration config = new SmtpEmailServerAppConfiguration();
            IEmailServerConfiguration server = new SmtpEmailServerConfiguration(config);
            IEmailSender sender = new SmtpEmailSender(server);

            var send = sender.SendEmail(email);

            Assert.IsTrue(send);
        }

        [TestMethod]
        public void SendMail() {
            var client = new SmtpClient("smtp.gmail.com", 587) {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("arquitectura.demo.pln@gmail.com", "Plenumsoft00"),
                EnableSsl = true
            };

            client.Send("arquitectura.demo.pln@gmail.com", "ronymaychan@gmail.com", "test", "testbody");
        }
    }
}
