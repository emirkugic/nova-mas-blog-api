using Microsoft.AspNetCore.Mvc;
using nova_mas_blog_api.Services;
using nova_mas_blog_api.DTOs;
using SixLabors.ImageSharp;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly ImageService _imageService;

    public ImagesController(ImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImages([FromForm] ImageUploadDTO imageUploadDTO)
    {
        if (imageUploadDTO.ImageFiles == null || !imageUploadDTO.ImageFiles.Any())
        {
            return BadRequest("No image files provided.");
        }

        var uploadResults = new List<ImageResponseDTO>();

        foreach (var file in imageUploadDTO.ImageFiles)
        {
            using var imageStream = file.OpenReadStream();
            using var image = SixLabors.ImageSharp.Image.Load(imageStream);
            using var ms = new MemoryStream();
            image.SaveAsJpeg(ms);
            ms.Position = 0;
            var imageData = ms.ToArray();

            var uploadedImages = await _imageService.UploadImagesAsync(new List<byte[]> { imageData });
            var uploadedImage = uploadedImages.FirstOrDefault();

            if (uploadedImage == default)
            {
                return BadRequest("Image upload failed for one or more images.");
            }

            var imageRecord = new nova_mas_blog_api.Models.Image
            {
                Url = uploadedImage.ImageUrl,
                DeleteHash = uploadedImage.DeleteHash,
                UploadDate = DateTime.UtcNow,
                BlogId = imageUploadDTO.BlogId
            };

            await _imageService.Create(imageRecord);

            uploadResults.Add(new ImageResponseDTO
            {
                Id = imageRecord.Id!,
                Url = imageRecord.Url,
                DeleteHash = imageRecord.DeleteHash,
                UploadDate = imageRecord.UploadDate,
                BlogId = imageRecord.BlogId
            });
        }

        return Created("api/images", uploadResults);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage(string id)
    {
        var image = await _imageService.GetById(id);
        if (image == null)
        {
            return NotFound();
        }

        var response = new ImageResponseDTO
        {
            Id = image.Id!,
            Url = image.Url,
            DeleteHash = image.DeleteHash,
            UploadDate = image.UploadDate,
            BlogId = image.BlogId
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImage(string id)
    {
        var image = await _imageService.GetById(id);
        if (image == null)
        {
            return NotFound();
        }

        await _imageService.DeleteImagesAsync(new List<string> { image.DeleteHash });
        var success = await _imageService.Delete(id);
        if (!success)
        {
            return BadRequest("Failed to delete the image.");
        }

        return NoContent();
    }

    [HttpDelete("blog_id")]
    public async Task<IActionResult> DeleteAllByBlogId(string blogId)
    {
        var success = await _imageService.DeleteAllByBlogId(blogId);
        if (!success)
        {
            return BadRequest("Failed to delete the images.");
        }
        return NoContent();
    }

    [HttpGet("blog_id")]
    public async Task<IActionResult> GetByBlogId(string blogId)
    {
        var images = await _imageService.GetByBlogId(blogId);
        if (images == null)
        {
            return NotFound();
        }

        var response = images.Select(image => new ImageResponseDTO
        {
            Id = image.Id!,
            Url = image.Url,
            DeleteHash = image.DeleteHash,
            UploadDate = image.UploadDate,
            BlogId = image.BlogId
        });

        return Ok(response);
    }

    [HttpDelete("url")]
    public async Task<IActionResult> DeleteByImageUrl(string imageUrl)
    {
        var image = await _imageService.DeleteByImageUrl(imageUrl);
        if (image == null)
        {
            return NotFound();
        }

        await _imageService.DeleteImagesAsync(new List<string> { image.DeleteHash });
        return NoContent();
    }


}
