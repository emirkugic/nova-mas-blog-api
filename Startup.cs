using System.Text.Json.Serialization;
using nova_mas_blog_api.Data;
using nova_mas_blog_api.Extensions;

namespace nova_mas_blog_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MongoDbContext>();
            services.AddCustomServices();
            services.AddAWSServices(Configuration);
            services.AddCustomAutoMapper();
            services.AddJwtAuthentication(Configuration);
            services.AddSwaggerDocumentation();
            services.AddRateLimitingServices(Configuration);
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddEndpointsApiExplorer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Middleware to capture and log NullReferenceExceptions
            app.Use(async (context, next) =>
            {
                try
                {
                    await next.Invoke();
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine($"EMIR NullReferenceException detected: {ex.Message} at {ex.StackTrace}");
                    throw;  // Re-throw the exception to keep the original stack trace
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nova Mas API V1"));
        }
    }
}
