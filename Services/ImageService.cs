using MongoDB.Driver;
using nova_mas_blog_api.Models;

namespace nova_mas_blog_api.Services
{
    public class ImageService : BaseService<Image>
    {
        private readonly ImgurUploadService _imgurService;

        public ImageService(IMongoCollection<Image> collection, ImgurUploadService imgurService)
            : base(collection)
        {
            _imgurService = imgurService;
        }



        public async Task<List<(string ImageUrl, string DeleteHash)>> UploadImagesAsync(List<byte[]> imagesData)
        {
            return await _imgurService.UploadImagesAsync(imagesData);
        }

        public async Task DeleteImagesAsync(List<string> deleteHashes)
        {
            await _imgurService.DeleteImagesAsync(deleteHashes);
        }

        // Get all images by Blog ID
        public async Task<IEnumerable<Image>> GetByBlogId(string blogId)
        {
            return await _collection.Find(Builders<Image>.Filter.Eq("BlogId", blogId)).ToListAsync();
        }

        // Delete all images by Blog ID and delete them from Imgur
        public async Task<bool> DeleteAllByBlogId(string blogId)
        {
            // First get all images to retrieve their delete hashes
            var images = await GetByBlogId(blogId);
            var deleteHashes = images.Select(img => img.DeleteHash).ToList();

            // Delete from Imgur
            if (deleteHashes.Any())
            {
                await DeleteImagesAsync(deleteHashes);
            }

            // Now delete the documents from MongoDB
            var deleteResult = await _collection.DeleteManyAsync(Builders<Image>.Filter.Eq("BlogId", blogId));
            return deleteResult.DeletedCount > 0;
        }


        public async Task<Image> DeleteByImageUrl(string image_url)
        {
            var image = await _collection.FindOneAndDeleteAsync(Builders<Image>.Filter.Eq("Url", image_url));
            return image;
        }
    }
}
