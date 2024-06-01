using nova_mas_blog_api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace nova_mas_blog_api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();

            return services;
        }

        public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program).Assembly);

            return services;
        }
    }
}
