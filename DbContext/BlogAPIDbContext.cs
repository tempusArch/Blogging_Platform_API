using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Data;

public class BlogAPIDbContext : DbContext {
    public BlogAPIDbContext(DbContextOptions<BlogAPIDbContext> options) :base(options) {
        
    }
    public DbSet<BlogModel> BlogTable {get; set;}
}