using nova_mas_blog_api.Data;
using nova_mas_blog_api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Connect to MongoDB
builder.Services.AddSingleton<MongoDbContext>();

// Extension methods for Services
builder.Services.AddCustomServices();

// Extension method for AutoMapper
builder.Services.AddCustomAutoMapper();

// Extension method for JWT authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Extension method for Swagger
builder.Services.AddSwaggerDocumentation();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nova Mas API V1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
