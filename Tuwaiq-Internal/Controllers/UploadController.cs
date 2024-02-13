 using IdentityService.SDK.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UploadController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpPost]
    public async Task<IActionResult> Post(List<IFormFile> files)
    {
        try
        {
            long size = files.Sum(f => f.Length);
            Dictionary<string, string> fileNames = new Dictionary<string, string>();
            // Larger than 100MB
            if (size > 104857600)
            {
                return BadRequest("FileSizeExceeded");
            }

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string fileName = Path.GetRandomFileName() + Path.GetExtension(formFile.FileName);
                    var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Storage",  "Files", fileName);

                    while (Path.Exists(filePath))
                    {
                        fileName = Path.GetRandomFileName() + Path.GetExtension(formFile.FileName);
                        filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Storage", "Files", fileName);
                    }

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                        fileNames.Add(formFile.FileName,  fileName);
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            var response = new FileUploadResult()
            {
                Count = files.Count,
                Size = size,
                FileNames = fileNames
            };
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
      
    }
}