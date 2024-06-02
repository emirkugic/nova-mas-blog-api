using nova_mas_blog_api.Extensions;
using nova_mas_blog_api.Data;

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

//* Configure the HTTP request pipeline using the extension method
app.ConfigurePipeline();

app.UseHttpsRedirection();
app.UseRateLimiting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
