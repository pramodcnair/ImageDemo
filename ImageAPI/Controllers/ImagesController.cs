
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ImageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly Options _options;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(HttpClient httpClient, Options options, ILogger<ImagesController> logger)
        {
            _httpClient = httpClient;
            _options = options;
            _logger = logger;
        }

        private async Task<CloudBlobContainer> GetCloudBlobContainer(string containerName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(_options.StorageConnectionString);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            return container;
        }

        [Route("/")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var container = await GetCloudBlobContainer(_options.FullImageContainerName);
            BlobContinuationToken contiuationToken = null;
            List<IListBlobItem> results = new List<IListBlobItem>();
            do
            {
                var response = await container.ListBlobsSegmentedAsync(contiuationToken);
                contiuationToken = response.ContinuationToken;
                results.AddRange(response.Results);
            } while (contiuationToken != null);
            return Ok(results.Select(b => b.Uri.AbsoluteUri));
        }

        [Route("/thumbs")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetThumbs()
        {
            var container = await GetCloudBlobContainer(_options.ThumbnailImageContainerName);
            BlobContinuationToken contiuationToken = null;
            List<IListBlobItem> results = new List<IListBlobItem>();
            do
            {
                var response = await container.ListBlobsSegmentedAsync(contiuationToken);
                contiuationToken = response.ContinuationToken;
                results.AddRange(response.Results);
            } while (contiuationToken != null);
            return Ok(results.Select(b => b.Uri.AbsoluteUri));
        }

        [Route("/")]
        [HttpPost]
        public async Task<ActionResult> Post()
        {
            Stream image = Request.Body;
            CloudBlobContainer container = await GetCloudBlobContainer(_options.FullImageContainerName);
            string blobName = $"{Guid.NewGuid().ToString().ToLower().Replace("-", string.Empty)}";
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            blockBlob.Properties.ContentType = Request.ContentType;
            await blockBlob.UploadFromStreamAsync(image);
            return Created(blockBlob.Uri, null);
        }
    }
}