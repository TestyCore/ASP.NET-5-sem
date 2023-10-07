using Microsoft.EntityFrameworkCore;
using WEB_153503_Tatarinov.API.Data;
using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Domain.Models;

namespace WEB_153503_Tatarinov.API.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _dbContext;

    public CategoryService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        return new ResponseData<List<Category>>
        {
            Data = await _dbContext.Categories.ToListAsync()
        };
    }
}