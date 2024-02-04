using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TuwaiqRecruitment.Data;
using TuwaiqRecruitment.ModelView;

namespace TuwaiqRecruitment.Controllers;

[Authorize(Roles = "admin")]
[ApiController]
[Route("api/[controller]")]
public class CompanyController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CompanyController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Get(int page = 1, int pageSize = 10, string query = "")
    {
        var companies = await _context.Companies
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var count = await _context.Companies.CountAsync();

        var res = new PaginationList
        {
            Data = companies,
            LastPage = (int)Math.Ceiling(count / (double)pageSize)
        };

        return Ok(companies);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> Update(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var company = await _context.Companies.FindAsync(id);
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> UploadLogo(IFormFile file)
    {
        switch (file.Length)
        {
            // Larger than 100MB
            case > 104857600:
                return BadRequest("FileSizeExceeded");
            case 0:
                return BadRequest("NoFile");
        }

        var fileName = Path.GetRandomFileName().Replace(".", "") + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Storage", fileName);

        await using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(fileName);
    }

    [HttpGet("[action]/{fileName}")]
    public async Task<IActionResult> GetCompanyLogo(string fileName)
    {
        var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Storage", fileName);
        var file = await System.IO.File.ReadAllBytesAsync(filePath);
        if (file == null) return NotFound();
        if (file.Length == 0) return NotFound();
        if (file.Length > 104857600) return BadRequest("FileSizeExceeded");
        return File(file, Path.GetExtension(fileName) == "png" ? "image/png" : "image/jpeg");
    }
}