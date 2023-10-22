using Microsoft.EntityFrameworkCore;
using WEB_153503_Tatarinov.API.Data;
using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Domain.Models;

namespace WEB_153503_Tatarinov.API.Services.ProductService;

public class ProductService : IProductService
{
    private readonly int _maxPageSize = 20;
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductService(AppDbContext dbContext, IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(
        string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        if (pageSize > _maxPageSize)
            pageSize = _maxPageSize;
        
        var query = _dbContext.Products.AsQueryable();
        var dataList = new ListModel<Product>();

        query = query
            .Where(d=> categoryNormalizedName==null
                       || d.Category.NormalizedName.Equals(categoryNormalizedName));
        ;
        var count = await query.CountAsync();
        if(count==0)
        {
            return new ResponseData<ListModel<Product>> {
                Data = dataList
            };
        }

        int totalPages = (int)Math.Ceiling(count / (double)pageSize);
        if (pageNo > totalPages)
            return new ResponseData<ListModel<Product>>
            {
                Data = null,
                Success = false,
                ErrorMessage = "No such page"
            };
        dataList.Items = await query
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        dataList.CurrentPage = pageNo;
        dataList.TotalPages = totalPages;

        var response = new ResponseData<ListModel<Product>> {
            Data = dataList
        };
        return response;
    }
    public async Task<ResponseData<Product>> CreateProductAsync(Product tool)
    {
        _dbContext.Products.Add(tool);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new ResponseData<Product> {
                Success = false,
                ErrorMessage = ex.Message,
            };
        }

        return new ResponseData<Product> 
        { 
            Data = tool 
        };
    }

    public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
    {
        var tool = await _dbContext.Products.FindAsync(id);
        if (tool is null)
        {
            return new()
            {
                Success = false,
                ErrorMessage = "Product was not found"
            };
        }

        return new()
        {
            Data = tool
        };
    }

    public async Task DeleteProductAsync(int id)
    {
        var tool = await _dbContext.Products.FindAsync(id);
        if (tool is null)
        {
            throw new Exception("Product was not found");
        }

        _dbContext.Products.Remove(tool);
        await _dbContext.SaveChangesAsync();
    }
    

    public async Task UpdateProductAsync(int id, Product tool)
    {
        var oldProduct = await _dbContext.Products.FindAsync(id);
        if (oldProduct is null)
        {
            throw new Exception("Product was not found");
        }

        oldProduct.Name = tool.Name;
        oldProduct.Description = tool.Description;
        oldProduct.Price = tool.Price;
        oldProduct.ImgPath = tool.ImgPath;
        oldProduct.Category = tool.Category;

        await _dbContext.SaveChangesAsync();
    }
    
    
    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
    {
        var responseData = new ResponseData<string>();
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
        {
            responseData.Success = false;
            responseData.ErrorMessage = "No item found";
            return responseData;
        }
        var host = "https://" + _httpContextAccessor.HttpContext?.Request.Host;
        var imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

        if (formFile != null)
        {
            if (!string.IsNullOrEmpty(product.ImgPath))
            {
                var prevImage = Path.GetFileName(product.ImgPath);
                var prevImagePath = Path.Combine(imageFolder, prevImage);
                if (File.Exists(prevImagePath))
                {
                    File.Delete(prevImagePath);
                }
            }
            var ext = Path.GetExtension(formFile.FileName);
            var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
            var filePath = Path.Combine(imageFolder, fName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            product.ImgPath = $"{host}/images/{fName}";
            await _dbContext.SaveChangesAsync();
        }
        responseData.Data = product.ImgPath;
        
        return responseData;
    }
}