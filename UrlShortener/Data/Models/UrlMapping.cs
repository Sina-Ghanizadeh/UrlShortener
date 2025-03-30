using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace UrlShortener.Data.Models;

[Collection("UrlMappings")]
public class UrlMapping
{
    public ObjectId Id { get; set; }
    public required string OriginalUrl { get; set; } 
    public required string ShortenCode { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int HitCount { get; set; } = 0;
    public DateTime? ExpirationDate { get; set; } = null;
    public int? MaxHitCount { get; set; } = null;
}