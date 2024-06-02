using nova_mas_blog_api.Extensions;
using nova_mas_blog_api.Data;
using nova_mas_blog_api.Middleware;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

// TODO: set up CORS policy before deployment to production

var builder = WebApplication.CreateBuilder(args);

//* Connect to MongoDB
builder.Services.AddSingleton<MongoDbContext>();

//* Extension methods for Services
builder.Services.AddCustomServices();

//* AWS Services setup
builder.Services.AddAWSServices(builder.Configuration);

//* Extension method for AutoMapper
builder.Services.AddCustomAutoMapper();

//* Extension method for JWT authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

//* Extension method for Swagger
builder.Services.AddSwaggerDocumentation();

//* Extension method for rate limiting
builder.Services.AddRateLimitingServices(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

//* Configure the HTTP request pipeline using the extension method
app.ConfigurePipeline();

//* Middleware that blocks requests from specific countries to prevent spam
app.UseMiddleware<CountryBlockingMiddleware>("./GeoLite2/GeoLite2-Country.mmdb");

app.UseHttpsRedirection();
app.UseRateLimiting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
