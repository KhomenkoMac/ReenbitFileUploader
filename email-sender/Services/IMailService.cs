using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EmailSender.Services;

public record MailData
(
    // Receiver
    List<string> To,
    string Subject, string Body
);

public interface IMailService
{
    Task<bool> SendAsync(MailData mailData, CancellationToken cancel);
}