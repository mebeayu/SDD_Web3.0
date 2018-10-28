using System.Net;
using System.Net.Mail;

namespace Common.Utils
{
    /// <summary>
    /// Powered by Rocby.com
    /// </summary>
    public class MailUtil
    {
        SmtpClient _sc = null;

        public MailUtil(string server, int port, string username, string password)
        {
            _sc = new SmtpClient(server, port);
            _sc.Credentials = new NetworkCredential(username, password);
            _sc.EnableSsl = true;
        }

        public void Send(string from, string to, string title, string content, bool ishtml)
        {
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(from);
            mm.To.Add(new MailAddress(to));
            mm.Subject = title;
            mm.Body = content;
            mm.IsBodyHtml = ishtml;
            mm.Priority = MailPriority.Normal;

            _sc.Send(mm);
        }
    }
}