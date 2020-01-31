
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
      return null;
    }

    [Route("/")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> Get()
    {
      return null;
    }

    [Route("/thumbs")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> GetThumbs()
    {
      return null;
    }

    [Route("/")]
    [HttpPost]
    public async Task<ActionResult> Post()
    {
      return null;
    }
  }
}