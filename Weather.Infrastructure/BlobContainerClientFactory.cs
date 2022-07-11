using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;
using Weather.Infrastructure.Models;
using Weather.Infrastructure.Models.Result.Abstractions.Generics;
using static Weather.Infrastructure.Models.Result.Implementations.Generics.Result<System.IO.Stream>;

namespace Weather.Infrastructure
{
    public interface IBlobContainerClientFactory
    {
        Task<IResult<Stream>> DownloadBlobStream(string blobPath);
    }

    public class BlobContainerClientFactory : IBlobContainerClientFactory
    {
        private readonly BlobContainerClient _blobContainerClient;
        private AppSettings Settings { get; }

        public BlobContainerClientFactory(IOptions<AppSettings> option)
        {
            Settings = option.Value;
            _blobContainerClient = new BlobContainerClient(Settings.ConnectionString, Settings.ContainerName);
        }

        public async Task<IResult<Stream>> DownloadBlobStream(string blobPath)
        {
            try
            {
                var file = _blobContainerClient.GetBlobClient(blobPath);

                return await file.ExistsAsync()
                    ? CreateSuccess(await file.OpenReadAsync())
                    : CreateFailed("Blob object doesn't exists.");
            }
            catch (RequestFailedException ex)
            {
                return CreateFailed(ex.Message);
            }
        }
    }
}
