using Microsoft.Extensions.DependencyInjection;
using nova_mas_blog_api.Data;
using nova_mas_blog_api.Models;
using nova_mas_blog_api.Services;
using MongoDB.Driver;

namespace nova_mas_blog_api.Extensions
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<AuthService>();
            services.AddScoped<BlogService>();
            services.AddScoped<AWSFileUploadService>();
            services.AddScoped<ImgurUploadService>();

            // Ensure that MongoDbContext is registered and available for injection
            services.AddSingleton<MongoDbContext>();

            // Register ImageService with correct dependencies
            services.AddScoped<ImageService>(serviceProvider =>
            {
                var dbContext = serviceProvider.GetRequiredService<MongoDbContext>();
                var imgurService = serviceProvider.GetRequiredService<ImgurUploadService>();
                return new ImageService(dbContext.Images, imgurService);
            });

            return services;
        }
    }
}
