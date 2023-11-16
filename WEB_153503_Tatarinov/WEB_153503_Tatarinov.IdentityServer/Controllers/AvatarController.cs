using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using WEB_153503_Tatarinov.IdentityServer.Models;

namespace WEB_153503_Tatarinov.IdentityServer.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AvatarController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly UserManager<ApplicationUser> _userManager;

    public AvatarController(IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userMgr)
    {
        _webHostEnvironment = webHostEnvironment;
        _userManager = userMgr;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvatar()    
    {
        var userId = _userManager.GetUserId(User);

        if (string.IsNullOrEmpty(userId))
        {
            return NotFound("User not found");
        }

        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", userId);

        var possibleExtensions = new[] { ".jpg", ".png", ".gif" }; // Здесь перечислите возможные расширения
        string mimeType = "application/octet-stream"; // MIME-тип по умолчанию

        var provider = new FileExtensionContentTypeProvider();
        var fileExt = ".png";

        foreach (var ext in possibleExtensions)
        {
            var filePath = imagePath + ext;
            if (System.IO.File.Exists(filePath))
            {
                fileExt = ext;
                if (provider.TryGetContentType(ext, out mimeType))
                {
                    break; // MIME-тип найден, можно завершить цикл
                }
            }
        }

        imagePath = imagePath + fileExt;
        if (!System.IO.File.Exists(imagePath))
        {
            imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "panda.jpg");
            mimeType = "image/jpg";
        }

        byte[] imageBytes;
        using (var stream = System.IO.File.OpenRead(imagePath))
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            imageBytes = memoryStream.ToArray();
        }

        return File(imageBytes, mimeType);
    }
}