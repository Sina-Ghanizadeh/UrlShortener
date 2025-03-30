using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers;

[Route("")]
[ApiController]
public class RedirectController : ControllerBase
{
    private readonly IUrlShortenerService _urlShortenerService;

    public RedirectController(IUrlShortenerService urlShortenerService)
    {
        _urlShortenerService = urlShortenerService;
    }

    [HttpGet("{shortenCode}")]
    public async Task<IActionResult> RetrieveOriginalUrl([FromRoute] string shortenCode)
    {
        var result = await _urlShortenerService.GetOriginalUrlAsync(shortenCode);
        return Redirect(result);
    }

}
