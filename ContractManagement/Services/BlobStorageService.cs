using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;

namespace ContractManagement.Services
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(string AzureStorage)
        {
            _blobServiceClient = new BlobServiceClient(AzureStorage);
        }

        public async Task UploadFileAsync(string containerName, string blobName, Stream fileStream)
        {
            try
            {
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                var blobClient = blobContainerClient.GetBlobClient(blobName);
                await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = "application/octet-stream" });

                Console.WriteLine("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UploadFileAsync: {ex.Message}");
            }
        }

        public async Task<Stream> DownloadFileAsync(string containerName, string blobName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            var download = await blobClient.DownloadAsync();
            return download.Value.Content;
        }

        public async Task DeleteFileAsync(string containerName, string blobName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
