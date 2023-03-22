using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Diagnostics;
using webapi.Controllers;

namespace api;


/// <summary>
/// Service that aims to upload docx file on
/// in Azure blob storage
/// </summary>
public class AzureDocxUploaderService
{
    private const string BlobContainerName = "docs";
    private readonly BlobServiceClient _blobServiceClient;

    public AzureDocxUploaderService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }
    
    public async Task UploadFileAsStream(UserDocDto request)
    {
        BlobContainerClient? docsContainer = await EsureCreatedBlobContainer(_blobServiceClient);
        
        if (docsContainer == null) return;
        
        var fileBuffer = new MemoryStream();
        
        await request
            .UserDocxFile
            .CopyToAsync(fileBuffer);

        var blobClient = docsContainer
            .GetBlobClient(request.UserDocxFile.FileName);
        
        using var readUploadFile = request
            .UserDocxFile
            .OpenReadStream();
        await blobClient
            .UploadAsync(readUploadFile, overwrite: false);
    }
    
    //-------------------------------------------------
    // Create a container
    //-------------------------------------------------
    private static async Task<BlobContainerClient?> EsureCreatedBlobContainer(BlobServiceClient blobServiceClient)
    {
        var containerName = AzureDocxUploaderService.BlobContainerName;

        try
        {
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

            container.CreateIfNotExists(PublicAccessType.BlobContainer);

            if (await container.ExistsAsync())
            {
                Debug.WriteLine("Created container {0}", container.Name);
            }

            return container;
        }
        catch (RequestFailedException e)
        {
            Console.WriteLine("HTTP error code {0}: {1}",
                e.Status, e.ErrorCode);
            Console.WriteLine(e.Message);
        }

        return null;
    }
}
