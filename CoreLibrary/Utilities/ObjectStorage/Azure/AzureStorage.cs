
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CoreLibrary.Extensions;
using CoreLibrary.Models.Setting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CoreLibrary.Utilities.ObjectStorage.Azure
{
    public class AzureStorage : Base.Storage, IAzureStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private BlobContainerClient _blobContainerClient;
        private readonly ConfigurationValues _configurationValues;

        public AzureStorage(IOptions<ConfigurationValues> configurationValues)
        {
            _configurationValues = configurationValues.Value;
            _blobServiceClient = new(_configurationValues.Storage.Azure);
        }

        public async Task DeleteAsync(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();
        }

        public bool HasFile(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string containerName, List<string> imageAsBase64List)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            string? baseUrl = _configurationValues.Storage.BasePath;

            List<(string fileName, string path)> datas = new();
            foreach (string file in imageAsBase64List)
            {
                IFormFile form = await ConvertBase64Generator.Base64ToImage(file);
                
                string fileNewName = await FileRenameAsync(containerName, form.Name, HasFile);

                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);
                await blobClient.UploadAsync(form.OpenReadStream());
                datas.Add((fileNewName, $"{baseUrl}/{containerName}/{fileNewName}"));
            }

            return datas;
        }
    }
}