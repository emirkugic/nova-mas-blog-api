using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace nova_mas_blog_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AWSImageController : ControllerBase
    {
        private readonly AWSFileUploadService _fileUploadService;

        public AWSImageController(AWSFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }


        [HttpPost("upload-profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var fileName = Path.Combine("profiles", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
            using (var stream = file.OpenReadStream())
            {
                var url = await _fileUploadService.UploadFileAsync(stream, fileName, file.ContentType);
                return Ok(new { Message = "File uploaded successfully.", Url = url });
            }
        }

        [HttpDelete("delete-profile-picture/{fileName}")]
        public async Task<IActionResult> DeleteProfilePicture(string fileName)
        {
            await _fileUploadService.DeleteFileAsync(fileName);
            return NoContent();
        }
    }
}