using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using UrlShortener.Common;
using UrlShortener.Data;
using UrlShortener.Data.Models;

namespace UrlShortener.Services;

public interface IUrlShortenerService
{
    Task<string> ShortenUrlAsync(string url);
    Task<string> GetOriginalUrlAsync(string shortenCode);
}
public class UrlShortenerService : IUrlShortenerService
{
    private readonly UrlShortenerDbContext _dbContext;
    private readonly AppSettings _appSettings;
    public UrlShortenerService(UrlShortenerDbContext dbContext, IOptions<AppSettings> appSettings)
    {
        _dbContext = dbContext;
        _appSettings = appSettings.Value;
    }

    public async Task<string> ShortenUrlAsync(string url)
    {
        var urlMapping = await _dbContext.UrlMappings
            .FirstOrDefaultAsync(x => x.OriginalUrl == url);

        if (urlMapping != null)
            return GetShortLinkUrl(urlMapping.ShortenCode);

        var shortenCode = await GenerateShortenCodeAsync();

        urlMapping = new UrlMapping
        {
            OriginalUrl = url,
            ShortenCode = shortenCode,

        };
        _dbContext.UrlMappings.Add(urlMapping);
        await _dbContext.SaveChangesAsync();

        return GetShortLinkUrl(shortenCode);
    }
    private string GetShortLinkUrl(string shortenCode) => $"{_appSettings.BaseUrl}/{shortenCode}";
    public async Task<string> GetOriginalUrlAsync(string shortenCode)
    {
        var urlMapping = await _dbContext.UrlMappings
            .Where(x => x.ExpirationDate == null || x.ExpirationDate > DateTime.UtcNow)
            .FirstOrDefaultAsync(x => x.ShortenCode == shortenCode);

        if (urlMapping != null)
            return urlMapping.OriginalUrl;

        throw new Exception("Shorten code not found.");
    }
    private async Task<string> GenerateShortenCodeAsync()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        string shortenCode;
        int maxAttempts = 100;
        int attempts = 0;

        do
        {
            if (attempts >= maxAttempts)
            {
                throw new Exception("Unable to generate a unique shorten code after multiple attempts.");
            }

            shortenCode = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            attempts++;
        } while (await _dbContext.UrlMappings.AnyAsync(x => x.ShortenCode == shortenCode));

        return shortenCode;
    }

}