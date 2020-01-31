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
      return null;
    }

    [BindProperty]
    public Microsoft.AspNetCore.Http.IFormFile Upload { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
      return null;
    }
  }
}
