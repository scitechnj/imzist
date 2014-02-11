using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Imzist.Logic
{
    public static class Emailer
    {
        public static void SendEmail(string emailAddress, string subject, string body)
        {
            var message = new MailMessage(new MailAddress("freeImzist@gmail.com"), new MailAddress(emailAddress))
            {
                Subject = subject,
                Body = body,
            };
            
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("freeimzist@gmail.com", "scitech08701"),
                EnableSsl = true
            };

            try
            {
                smtp.Send(message);
            }
            catch (Exception e)
            {
                //TODO: Please teach us logging!!!!
            }
        }
    }
}
