using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.UI.WPF.Helpers
{
    public class EMailHelper
    {
        public static bool SendEMail(string mailAdress, string code, string firstName, string lastName)
        {
            MailAddress from = new MailAddress("todolist.turkey@gmail.com");
            MailAddress to = new MailAddress(mailAdress);

            MailMessage message = new MailMessage();
            message.From = from;
            message.To.Add(to);
            message.Subject = "To-Do List Membership Activation";
            message.Body = string.Format("Dear "+firstName+" "+lastName+"<br/><br/>"+"Your Activation Code is : "+code+"<br/><br/>"+"Thanks && To-Do-List Turkey");
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.Credentials = new NetworkCredential(from.Address, "5536699447");
            client.EnableSsl = true;
            
            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
