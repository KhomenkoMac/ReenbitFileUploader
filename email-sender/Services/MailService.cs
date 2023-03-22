
namespace EmailSender.Services;

using System;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

public class MailService : IMailService
{
    const int SmptPort = 587;
    const string SmtpHost = "smtp.ethereal.email";
    const string From = "hassie.hahn@ethereal.email";
    const string FromDisplayName = "File Uploader";
    const string FromPassword = "kn8Jb5kRck3ur8mQMt";
    
    public async Task<bool> SendAsync(MailData mailData, CancellationToken cancelToken = default)
    {
        try
        {
            var mail = new MimeMessage();

            #region Sender / Receiver
            // Sender
            mail.From.Add(MailboxAddress.Parse(From));
            // Receiver
            mail.To.AddRange(mailData.To.Select(MailboxAddress.Parse));
            // foreach (string mailAddress in mailData.To)
            //     mail.To.Add(MailboxAddress.Parse(mailAddress));

            #endregion

            #region Content

            // Add Content to Mime Message
            var body = new BodyBuilder();
            mail.Subject = mailData.Subject;
            body.HtmlBody = mailData.Body;
            mail.Body = body.ToMessageBody();

            #endregion

            #region Send Mail

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                SmtpHost, 
                SmptPort, 
                MailKit.Security.SecureSocketOptions.StartTls
            );

            smtp.Authenticate(From, FromPassword);

            await smtp.SendAsync(mail, cancelToken);
            await smtp.DisconnectAsync(true, cancelToken);
            
            #endregion

            return true;

        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return false;
        }
    }
}