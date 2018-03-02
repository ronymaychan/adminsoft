
using PLNFramework.Mailing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminSoft.WebSite.Helpers
{
    public static class EmailHelper
    {
        public static bool SendMail(IEmail email) {

            ISmtpEmailServerAppConfiguration config = new SmtpEmailServerAppConfiguration();
            IEmailServerConfiguration server = new SmtpEmailServerConfiguration(config);
            IEmailSender sender = new SmtpEmailSender(server);

            try
            {
                return sender.SendEmail(email);
            }
            catch { }

            return false;
        }
    }
}