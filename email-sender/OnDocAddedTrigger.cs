using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EmailSender.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EmailSender.Function;

public class OnDocAddedTrigger
{
    private enum RecievedFilenamePieces 
    { 
        UserEmail, 
        DocxName 
    }

    private const string Docname_UserEmail_Separator = "__";
    private readonly IMailService _mailService;

    public OnDocAddedTrigger(IMailService mailService)
    {
        _mailService = mailService;
    }

    [FunctionName("OnDocAddedTrigger")]
    public async Task RunAsync([BlobTrigger("docs/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
    {
        var recievedFilenamePieces = name.Split(OnDocAddedTrigger.Docname_UserEmail_Separator);

        if (recievedFilenamePieces.Length != 2)
        {
            log.LogInformation($"Skipping file -> {name}");
            return;
        }

        string userEmail = recievedFilenamePieces[(int)RecievedFilenamePieces.UserEmail];
        string docxName = recievedFilenamePieces[(int)RecievedFilenamePieces.DocxName];

        var isSent = await _mailService.SendAsync(new MailData(
            new List<string>{ userEmail }, 
            "Docx Upload Notification",
            $"<p style=\"font-size: 2.5em; font-family: \'Segoe UI\', Tahoma, Geneva, Verdana, sans-serif;\"> Dear <strong>{userEmail}</strong> , <p style=\"font-family: consolas; font-size: 1.2em;\"> Your document <span style=\"border-radius: 50px; color: rgb(180, 12, 149); background-color: rgb(255, 192, 243);\">{docxName}</span> has been uploaded! </p> </p>"
        ), CancellationToken.None);

        log.LogInformation($"Is sent: {isSent}; User email: {userEmail}; Docx name: {docxName}; Size: {myBlob.Length} Bytes");
    }
}
