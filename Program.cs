using Microsoft.OpenApi.Models;
using nova_mas_blog_api.Data;
using nova_mas_blog_api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Connect to MongoDB
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddCustomServices();
builder.Services.AddCustomAutoMapper();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Nova Mas API",
        Version = "v1",
        Description = "API to manage blog application",
        Contact = new OpenApiContact
        {
            Name = "Emir Kugić",
            Email = "emirkugic0@gmail.com",
            Url = new Uri("https://emirkugic.tech/")
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
