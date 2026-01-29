using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.BlobStorage;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Infrastructure.BlobStorage;

public class BlobStorageService : IBlobStorageService
{
    private const string AvatarFolderName = "avatars";
    private const string AttachmentsFolderName = "task-attachments";

    private readonly BlobServiceClient _serviceClient;

    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AzureStorage");
        _serviceClient = new BlobServiceClient(connectionString);
    }

    public async Task<bool> DeleteBlobAsync(string blobName, BlobContainerType container)
    {
        var containerClient = GetContainerClient(container);
        var blobClient = containerClient.GetBlobClient(blobName);

        var exists = await blobClient.ExistsAsync();

        if (!exists)
        {
            return false;
        }

        await blobClient.DeleteAsync();
        return true;
    }

    public async Task<string> UploadAvatarAsync(
        IFormFile file,
        int userId)
    {
        var containerClient = _serviceClient.GetBlobContainerClient(AvatarFolderName);

        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var fileName = $"{userId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var blobClient = containerClient.GetBlobClient(fileName);

        var blobHttpHeader = new BlobHttpHeaders { ContentType = file.ContentType };

        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });

        return blobClient.Uri.ToString();
    }

    public async Task<string> UploadTaskAttachmentAsync(
        IFormFile file,
        int taskId)
    {
        var containerClient = _serviceClient.GetBlobContainerClient(AttachmentsFolderName);

        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var fileName = $"{taskId}/{Guid.NewGuid()}_{file.FileName}";
        var blobClient = containerClient.GetBlobClient(fileName);

        var blobHttpHeader = new BlobHttpHeaders { ContentType = file.ContentType };

        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });

        return blobClient.Uri.ToString();
    }

    private BlobContainerClient GetContainerClient(BlobContainerType container)
    {
        var containerName = container switch
        {
            BlobContainerType.Avatars => "avatars",
            BlobContainerType.TaskAttachments => "task-attachments",
            _ => throw new ArgumentOutOfRangeException(nameof(container), container, null)
        };

        return _serviceClient.GetBlobContainerClient(containerName);
    }
}
