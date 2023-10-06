using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Domain.Models;
using WEB_153503_Tatarinov.Services.CategoryService;

namespace WEB_153503_Tatarinov.Services.ProductService;

public class MemoryProductService : IProductService
{
    private List<Product> _products;
    private List<Category> _categories;
    private IConfiguration _configuration;
    
    public MemoryProductService(IConfiguration configuration, ICategoryService categoryService)
    {
        _configuration = configuration;
        
        _categories=categoryService.GetCategoryListAsync().Result.Data;
        SetupData();
    }
    
    /// <summary>
    ///Initialization of lists
    /// </summary>
    private void SetupData()
    {
        _products = new List<Product>
        {
            new Product {
                Id = 1, Name="Clouds & Posies Cake",
                Description="Elegantly rustic in plush white buttercream.",
                Price = 98, ImgPath= "Images/clouds_posies_cake.png",
                Category= _categories.Find(c=>c.NormalizedName.Equals("cakes"))},
            new Product {
                Id = 2, Name="Chocolate Frida Cake",
                Description="Our classic chocolate cake.",
                Price = 98, ImgPath= "Images/chocolate_frida_cake.png",
                Category= _categories.Find(c=>c.NormalizedName.Equals("cakes"))},
            new Product {
                Id = 3, Name="Roses & Roses Cake",
                Description="Our classic chocolate cake.",
                Price = 98, ImgPath= "Images/roses_roses_cake.png",
                Category= _categories.Find(c=>c.NormalizedName.Equals("cakes"))},
            new Product {
                Id = 4, Name="Pink Champagne Cake",
                Description="The right amount of sweetness.",
                Price = 108, ImgPath= "Images/pink_champagne_cake.png",
                Category= _categories.Find(c=>c.NormalizedName.Equals("cakes"))},
            new Product {
                Id = 5, Name="Super Sprinkles Cake",
                Description="This cake brings a smile to all ages!",
                Price = 98, ImgPath= "Images/super_sprinkles_cake.png",
                Category= _categories.Find(c=>c.NormalizedName.Equals("cakes"))},
            new Product {
                Id = 6, Name="Gold Foiled Cake",
                Description="Adorned with a simple, striking deep.",
                Price = 108, ImgPath= "Images/gold_foiled_cake.png",
                Category= _categories.Find(c=>c.NormalizedName.Equals("cakes"))},
            new Product {
                Id = 7, Name="Geo Prism Cake",
                Description="Covering layers of fresh delicious cake. ",
                Price = 98, ImgPath= "Images/geo_prism_cake.png",
                Category= _categories.Find(c=>c.NormalizedName.Equals("cakes"))},
            new Product {
                Id = 8, Name="Plain ring",
                Description="Fully finished dense ring cake.",
                Price = 3, ImgPath= "Images/plain_ring.png",
                Category= _categories.Find(c=>c.NormalizedName.Equals("donuts"))},
            new Product {
                Id = 9, Name="Mini ring",
                Description="Fully finished small powdered sugar.",
                Price = 5, ImgPath= "Images/mini_ring.png",
                Category= _categories.Find(c=>c.NormalizedName.Equals("donuts"))},
            new Product {
                Id = 10, Name="Roll donuts",
                Description="Fully finished yeast donut rolled.",
                Price = 4, ImgPath= "Images/roll_donuts.png",
                Category= _categories.Find(c=>c.NormalizedName.Equals("donuts"))},
            new Product {
                Id = 11, Name="Fritter donuts",
                Description="Fully finished yeast dough.",
                Price = 6, ImgPath= "Images/fritter_donuts.png",
                Category= _categories.Find(c=>c.NormalizedName.Equals("donuts"))},
        };
    }

    public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var result = new ResponseData<ListModel<Product>>();

        if (Int32.TryParse(_configuration["ItemsPerPage"], out int itemsPerPage))
        {
            var products = _products.Where(p => categoryNormalizedName == null ||
                                                p.Category.NormalizedName.Equals(categoryNormalizedName)).ToList();

            result.Data = new()
            {
                Items = products.Skip((pageNo - 1) * itemsPerPage).Take(itemsPerPage).ToList(),
                CurrentPage = pageNo,
                TotalPages = (int)Math.Ceiling((double)products.Count / itemsPerPage)
            };
        }
        else
        {
            result.Success = false;
            result.ErrorMessage = "Invalid \"ItemsPerPage\" value";
        }

        return Task.FromResult(result);
    }

    public Task<ResponseData<Product>> GetProductByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProductAsync(int id, Product product, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProductAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }
}