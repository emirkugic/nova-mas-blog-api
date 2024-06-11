using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace nova_mas_blog_api.Services
{
    public class ImgurUploadService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ImgurUploadService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.imgur.com/3/"),
                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Client-ID", _configuration["Imgur:ClientId"]) }
            };
        }

        // TODO: refactor: convert to JPEG before uploading in ImgurUploadService instead of ImageController
        public async Task<List<(string ImageUrl, string DeleteHash)>> UploadImagesAsync(List<byte[]> imagesData)
        {
            var results = new List<(string ImageUrl, string DeleteHash)>();

            foreach (var imageData in imagesData)
            {
                using var content = new MultipartFormDataContent();
                content.Add(new ByteArrayContent(imageData), "image");

                var response = await _httpClient.PostAsync("image", content);
                response.EnsureSuccessStatusCode();
                var data = JObject.Parse(await response.Content.ReadAsStringAsync());

                var imageUrl = data["data"]?["link"]?.ToString();
                var deleteHash = data["data"]?["deletehash"]?.ToString();

                results.Add((imageUrl!, deleteHash!));
            }
            return results;
        }

        public async Task DeleteImagesAsync(List<string> deleteHashes)
        {
            foreach (var deleteHash in deleteHashes)
            {
                await _httpClient.DeleteAsync($"image/{deleteHash}");
            }
        }
    }

}