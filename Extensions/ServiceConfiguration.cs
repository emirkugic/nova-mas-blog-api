using nova_mas_blog_api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace nova_mas_blog_api.Extensions
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<AuthService>();
            services.AddScoped<FileUploadService>();

            return services;
        }
    }
}
