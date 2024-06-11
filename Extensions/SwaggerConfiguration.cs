using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace nova_mas_blog_api.Extensions
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
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

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                // Register the EnumSchemaFilter to display enums as strings
                c.SchemaFilter<EnumSchemaFilter>();
                // Register the file upload operation filter
                c.OperationFilter<FileUploadOperation>();

            });

            return services;
        }
        public class EnumSchemaFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema schema, SchemaFilterContext context)
            {
                if (context.Type.IsEnum)
                {
                    schema.Enum.Clear();
                    foreach (var enumName in Enum.GetNames(context.Type))
                    {
                        schema.Enum.Add(new OpenApiString(enumName));
                    }
                    schema.Type = "string";
                }
            }
        }
    }
}
