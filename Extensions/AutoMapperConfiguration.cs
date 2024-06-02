namespace nova_mas_blog_api.Extensions
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program).Assembly);
            return services;
        }
    }
}
