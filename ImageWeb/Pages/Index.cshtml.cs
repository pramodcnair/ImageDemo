using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ImageWeb.Pages
{
  public class IndexModel : PageModel
  {
    private HttpClient _httpClient;
    private Options _options;
    private readonly ILogger<IndexModel> _logger;

    public List<string> ThumbnailImageList { get; private set; }
    public List<string> FullImageList { get; private set; }
    public IndexModel(HttpClient httpClient, Options options, ILogger<IndexModel> logger)
    {
      _httpClient = httpClient;
      _options = options;
      _logger = logger;
    }
    public async Task OnGetAsync()
    {
      var imageUrl=_options.ApiUrl;

      var thumbsUrl=Flurl.Url.Combine(imageUrl,"/thumbs/");

      Task<string> getFullImages=_httpClient.GetStringAsync(imageUrl);

      Task<string> getThumbnailImages=_httpClient.GetStringAsync(thumbsUrl);

      await Task.WhenAll(getFullImages);

      string fullImageJson=getFullImages.Result;

      IEnumerable<string> fullImageList=JsonConvert.DeserializeObject<IEnumerable<String>>(fullImageJson);
      
      FullImageList =fullImageList.ToList<string>();

      string thumbnailImageJson=getThumbnailImages.Result;

      IEnumerable<string> thumbnailImageList=JsonConvert.DeserializeObject<IEnumerable<String>>(thumbnailImageJson);
      
      ThumbnailImageList =thumbnailImageList.ToList<string>();
    }

    [BindProperty]
    public Microsoft.AspNetCore.Http.IFormFile Upload { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {

      if(Upload !=null && Upload.Length>0)
      {
        var imageUrl=_options.ApiUrl;
        using(var image=new StreamContent(Upload.OpenReadStream()))
        {
          image.Headers.ContentType=new System.Net.Http.Headers.MediaTypeHeaderValue(Upload.ContentType);

          var resposne=await _httpClient.PostAsync(imageUrl,image);
        }
      }

      return RedirectToPage("/Index");
    }
  }
}