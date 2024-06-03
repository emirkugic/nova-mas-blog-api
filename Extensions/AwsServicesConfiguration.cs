using Amazon.S3;
using Amazon.Extensions.NETCore.Setup;

namespace nova_mas_blog_api.Extensions
{
    public static class AwsServicesConfiguration
    {
        public static IServiceCollection AddAWSServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAWSService<IAmazonS3>(new AWSOptions
            {
                Credentials = new Amazon.Runtime.BasicAWSCredentials(
                    configuration["AWS:AccessKey"],
                    configuration["AWS:SecretKey"]),
                Region = Amazon.RegionEndpoint.GetBySystemName(configuration["AWS:Region"])
            });

            return services;
        }
    }
}
