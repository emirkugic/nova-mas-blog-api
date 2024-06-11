using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace nova_mas_blog_api.Extensions
{
    public class FileUploadOperation : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if the operation is for creating a blog
            if (context.ApiDescription.HttpMethod == "POST" && context.ApiDescription.RelativePath == "api/Blogs")
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = {
                                    ["Title"] = new OpenApiSchema { Type = "string" },
                                    ["Content"] = new OpenApiSchema { Type = "string" },
                                    ["Category"] = new OpenApiSchema { Type = "string" }, // Ensure enum is properly handled if needed
                                    ["UserId"] = new OpenApiSchema { Type = "string" },
                                    ["images"] = new OpenApiSchema {
                                        Type = "array",
                                        Items = new OpenApiSchema { Type = "string", Format = "binary" }
                                    }
                                },
                                Required = new HashSet<string> { "Title", "Content", "UserId", "Category" }
                            }
                        }
                    }
                };
            }
        }
    }
}
