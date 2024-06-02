using nova_mas_blog_api.Services;

namespace nova_mas_blog_api.Extensions
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<AuthService>();
            services.AddScoped<BlogService>();
            services.AddScoped<FileUploadService>();

            return services;
        }
    }
}
