using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrlShortener.Dto;
using UrlShortener.Services;

namespace UrlShortener.Controllers;

[Route("[controller]")]
[ApiController]
public class ShortenController : ControllerBase
{
    private readonly IUrlShortenerService _urlShortenerService;

    public ShortenController(IUrlShortenerService urlShortenerService)
    {
        _urlShortenerService = urlShortenerService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShortenLink([FromBody] ShortenLinkRequestDto linkRequest)
    {
        var shortenCode = await _urlShortenerService.ShortenUrlAsync(linkRequest.Url);
        return Ok(shortenCode);
    }
}
