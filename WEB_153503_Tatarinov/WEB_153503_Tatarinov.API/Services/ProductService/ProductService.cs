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

    public ProductService(AppDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
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

    

    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
    {
        var tool = await _dbContext.Products.FindAsync(id);
        if (tool is null)
        {
            return new ResponseData<string>
            {
                Success = false,
                ErrorMessage = "Product was not found"
            };
        }

        string imageRoot = Path.Combine(_configuration["AppUrl"]!, "images");
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
        
        string imagePath = Path.Combine(imageRoot, uniqueFileName);

        using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            await formFile.CopyToAsync(stream);
        }

        tool.ImgPath = imagePath;
        await _dbContext.SaveChangesAsync();

        return new ResponseData<string>
        {
            Data = tool.ImgPath,
            Success = true
        };

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
}