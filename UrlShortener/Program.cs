using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using UrlShortener.Common;
using UrlShortener.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Database Configuration
builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
{
    var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings");
    var connectionString = mongoDbSettings.GetValue<string>("ConnectionString");
    var databaseName = mongoDbSettings.GetValue<string>("DatabaseName");

    if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(databaseName))
    {
        throw new InvalidOperationException("MongoDbSettings are not configured properly.");
    }

    options.UseMongoDB(connectionString, databaseName);
});
#endregion
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
