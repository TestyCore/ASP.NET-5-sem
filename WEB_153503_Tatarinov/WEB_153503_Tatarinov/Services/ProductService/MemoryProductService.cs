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
            new Product {Id = 1, Name="Clouds & Posies Cake",
                Description="Elegantly rustic in plush white buttercream.",
                Price = 98, ImgPath= "Images/clouds_posies_cake.png",
                Category= _categories.Find(c=>c.NormalizedName.Equals("cakes"))},
            new Product { Id = 2, Name="Chocolate Frida Cake",
                Description="Our classic chocolate cake.",
                Price = 98, ImgPath= "Images/chocolate_frida_cake.png",
                Category= _categories.Find(c=>c.NormalizedName.Equals("cakes"))},
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
                Items = products,
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