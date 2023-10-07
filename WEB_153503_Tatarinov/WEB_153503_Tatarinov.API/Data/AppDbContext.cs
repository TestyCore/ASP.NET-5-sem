using Microsoft.EntityFrameworkCore;
using WEB_153503_Tatarinov.Domain.Entities;

namespace WEB_153503_Tatarinov.API.Data;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {


    }
}