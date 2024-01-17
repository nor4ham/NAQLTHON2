/* using Microsoft.AspNetCore.Mvc;
using Minio;
namespace Customer.Controller;
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
// Validate and process the image if needed
private readonly MinioClient _minio;
public CustomerController(MinioClient minio)
{
        _minio = minio;
}
[HttpPost("upload")]
public async Task<IActionResult> UploadImage(IFormFile imageFile)
{
    // Upload the image to Minio
    using (var stream = imageFile.OpenReadStream())
    {
        var contentType = imageFile.ContentType;
        await _minio.PutObjectAsync("minio-test", "image.jpg", stream, stream.Length, contentType);
    }

    return Ok("Image uploaded successfully.");
}

    
}
 */