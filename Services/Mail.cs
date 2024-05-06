using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Services
{
    public class Mail(string subject, string body, string toEmail, string toName)
    {
        const string fromEmail = "app.notes.recovery@gmail.com";
        const string fromPassword = "evdt urja mhen lhwq";
        readonly string subject = subject;
        readonly string body = body;
        readonly string toEmail = toEmail;
        readonly string toName = toName;

        public void sendMail()
        {
            var fromAddress = new MailAddress(fromEmail, "AppNotes");
            var toAddress = new MailAddress(toEmail, toName);

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            message.IsBodyHtml = true;
            smtp.Send(message);
        }
    }
}
        
       
        

        

