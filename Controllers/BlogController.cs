using BlogAPI.Data;
using BlogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BlogController : ControllerBase {
    private readonly ILogger<BlogController> _logger;
    private readonly BlogAPIDbContext _context;

    public BlogController(ILogger<BlogController> logger, BlogAPIDbContext context) {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BlogModel>>> GetAllBlogs() {
        var result = await _context.BlogTable.ToListAsync();
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<BlogModel>> GetSpecificBlog(int id) {
        var result = await _context.BlogTable.FindAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BlogModel>> PostBlog(BlogModel blog) {
        _context.BlogTable.Add(blog);
        await _context.SaveChangesAsync();

        return Created(string.Empty, blog);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlog(int id, BlogModel blog) {
        var existingOne = await _context.BlogTable.FindAsync(id);

        if (existingOne == null)
            return NotFound();

        existingOne.Title = blog.Title;
        existingOne.Content = blog.Content;
        existingOne.Category = blog.Category;
        existingOne.Tags = blog.Tags;
        existingOne.UpdatedAt = DateTime.UtcNow;

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            throw;
        }

        return Ok(blog);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlog(int id) {
        var blog = await _context.BlogTable.FindAsync(id);

        if (blog == null)
            return NotFound();

        _context.BlogTable.Remove(blog);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<BlogModel>>> SearchBlog([FromQuery] string query) {
        if (string.IsNullOrEmpty(query))
            return BadRequest("Query is empty");

        var result = await _context.BlogTable
            .Where(n => n.Title.Contains(query) || n.Content.Contains(query) || n.Category.Contains(query) || n.Tags.Contains(query))
            .ToListAsync();

        if (result.Count == 0)
            return NotFound("No blogs found");
        else
            return Ok(result);
    }
}