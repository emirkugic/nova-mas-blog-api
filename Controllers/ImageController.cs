using Microsoft.AspNetCore.Mvc;
using nova_mas_blog_api.Services;
using nova_mas_blog_api.DTOs;
using nova_mas_blog_api.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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
    public async Task<IActionResult> UploadImage([FromForm] ImageUploadDTO imageUploadDTO)
    {
        var imageFile = imageUploadDTO.ImageFile;
        if (imageFile == null || imageFile.Length == 0)
        {
            return BadRequest("No image file provided.");
        }

        using var stream = new MemoryStream();
        await imageFile.CopyToAsync(stream);
        var imageData = stream.ToArray();

        var uploadedImages = await _imageService.UploadImagesAsync(new List<byte[]> { imageData });
        var uploadedImage = uploadedImages.FirstOrDefault();

        if (uploadedImage == default)
        {
            return BadRequest("Image upload failed.");
        }

        var image = new Image
        {
            Url = uploadedImage.ImageUrl,
            DeleteHash = uploadedImage.DeleteHash,
            UploadDate = DateTime.UtcNow,
            BlogId = imageUploadDTO.BlogId
        };

        await _imageService.Create(image);

        var response = new ImageResponseDTO
        {
            Id = image.Id,
            Url = image.Url,
            DeleteHash = image.DeleteHash,
            UploadDate = image.UploadDate,
            BlogId = image.BlogId
        };

        return CreatedAtAction(nameof(GetImage), new { id = image.Id }, response);
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
            Id = image.Id,
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
}
