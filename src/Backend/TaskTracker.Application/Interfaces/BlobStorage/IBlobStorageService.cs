using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Interfaces.BlobStorage;

public interface IBlobStorageService
{
    Task DeleteBlobAsync(string blobName, BlobContainerType container);
    Task<string> UploadAvatarAsync(IFormFile file, BlobContainerType containerType, int userId);
}
