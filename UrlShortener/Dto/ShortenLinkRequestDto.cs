using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Dto;

public class ShortenLinkRequestDto
{
    [Required(AllowEmptyStrings = false)]
    [Url(ErrorMessage = "Invalid URL format.")]
    public string Url { get; set; }
    public DateTime? ExpirationDate { get; set; } = null;
    public int? MaxHitCount { get; set; } = null;
}