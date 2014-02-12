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
        private const string OurEmailAddress = "freeImzist@gmail.com";
        private const string OurPassword = "scitech08701";

        public static void SendEmail(string emailAddress, string subject, string body, string replyToAddress = OurEmailAddress)
        {
            var message = new MailMessage(new MailAddress(OurEmailAddress), new MailAddress(emailAddress))
            {
                Subject = subject,
                Body = body
            };

            if (!String.IsNullOrEmpty(replyToAddress))
            {
                //@todo validate email 
                message.ReplyToList.Add(replyToAddress);
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(OurEmailAddress, OurPassword),
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
