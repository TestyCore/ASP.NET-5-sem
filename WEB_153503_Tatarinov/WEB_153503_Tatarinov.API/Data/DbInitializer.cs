using Microsoft.EntityFrameworkCore;
using WEB_153503_Tatarinov.Domain.Entities;

namespace WEB_153503_Tatarinov.API.Data;

public class DbInitializer
{
    public static async Task SeedData(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        await context.Database.MigrateAsync();
        
        await context.Categories.AddRangeAsync(new List<Category>()
        {
            new Category() {Name="Cakes", NormalizedName="cakes"},
            new Category() {Name="Buns", NormalizedName="buns"},
            new Category() {Name="Pies", NormalizedName="pies"},
            new Category() {Name="Donuts", NormalizedName="donuts"}
        });
        await context.SaveChangesAsync();
        
        string imageRoot = $"{app.Configuration["AppUrl"]!}/images";

        await context.Products.AddRangeAsync(new List<Product>()
        {
            new Product() {
                Name="Clouds & Posies Cake",
                Description="Elegantly rustic in plush white buttercream.",
                Price = 98,
                ImgPath= $"{imageRoot}/clouds_posies_cake.png",
                ImgMimeType = "img/png",
                Category= await context.Categories.SingleAsync(c => c.NormalizedName.Equals("cakes"))
            },
            new Product() {
                Name="Chocolate Frida Cake",
                Description="Our classic chocolate cake.",
                Price = 98,
                ImgPath= $"{imageRoot}/chocolate_frida_cake.png",
                ImgMimeType = "img/png",
                Category= await context.Categories.SingleAsync(c => c.NormalizedName.Equals("cakes"))
            },
            new Product() {
                Name="Roses & Roses Cake",
                Description="Our classic chocolate cake.",
                Price = 98,
                ImgPath= $"{imageRoot}/roses_roses_cake.png",
                ImgMimeType = "img/png",
                Category= await context.Categories.SingleAsync(c => c.NormalizedName.Equals("cakes"))
            },
            new Product() {
                Name="Pink Champagne Cake",
                Description="The right amount of sweetness.",
                Price = 108,
                ImgPath= $"{imageRoot}/pink_champagne_cake.png",
                ImgMimeType = "img/png",
                Category= await context.Categories.SingleAsync(c => c.NormalizedName.Equals("cakes"))
            },
            new Product() {
                Name="Super Sprinkles Cake",
                Description="This cake brings a smile to all ages!",
                Price = 98,
                ImgPath= $"{imageRoot}/super_sprinkles_cake.png",
                ImgMimeType = "img/png",
                Category= await context.Categories.SingleAsync(c => c.NormalizedName.Equals("cakes"))
            },
            new Product() {
                Name="Gold Foiled Cake",
                Description="Adorned with a simple, striking deep.",
                Price = 108,
                ImgPath= $"{imageRoot}/gold_foiled_cake.png",
                ImgMimeType = "img/png",
                Category= await context.Categories.SingleAsync(c => c.NormalizedName.Equals("cakes"))
            },
            new Product() {
                Name="Geo Prism Cake",
                Description="Covering layers of fresh delicious cake.",
                Price = 98,
                ImgPath= $"{imageRoot}/geo_prism_cake.png",
                ImgMimeType = "img/png",
                Category= await context.Categories.SingleAsync(c => c.NormalizedName.Equals("cakes"))
            },
            new Product() {
                Name="Plain ring",
                Description="Fully finished dense ring cake.",
                Price = 3,
                ImgPath= $"{imageRoot}/plain_ring.png",
                ImgMimeType = "img/png",
                Category= await context.Categories.SingleAsync(c => c.NormalizedName.Equals("donuts"))
            },
            new Product() {
                Name="Mini ring",
                Description="Fully finished small powdered sugar.",
                Price = 5,
                ImgPath= $"{imageRoot}/mini_ring.png",
                ImgMimeType = "img/png",
                Category= await context.Categories.SingleAsync(c => c.NormalizedName.Equals("donuts"))
            },
            new Product() {
                Name="Roll donuts",
                Description="Fully finished yeast donut rolled.",
                Price = 4,
                ImgPath= $"{imageRoot}/roll_donuts.png",
                ImgMimeType = "img/png",
                Category= await context.Categories.SingleAsync(c => c.NormalizedName.Equals("donuts"))
            },
            new Product() {
                Name="Fritter donuts",
                Description="Fully finished yeast dough.",
                Price = 6,
                ImgPath= $"{imageRoot}/fritter_donuts.png",
                ImgMimeType = "img/png",
                Category= await context.Categories.SingleAsync(c => c.NormalizedName.Equals("donuts"))
            },
        });
        
        await context.SaveChangesAsync();
    }
}