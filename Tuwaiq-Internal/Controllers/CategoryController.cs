using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TuwaiqRecruitment.Data;

namespace TuwaiqRecruitment.Controllers;

[Authorize(Roles = "admin")]
[ApiController]
[Route("api/[controller]")]
public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    [HttpGet("[action]")]
    public async Task<IActionResult> Get()
    {
        var candidates = await _context.Categories.ToListAsync();

        return Ok(candidates);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create(string name)
    {
        await _context.Categories.AddAsync(new Category { Name = name });
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> Update(Guid id, string name)
    {
        var category = await _context.Categories.FindAsync(id);
        category.Name = name;
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return Ok();
    }
}