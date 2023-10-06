using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Domain.Models;

namespace WEB_153503_Tatarinov.Services.CategoryService;

public class MemoryCategoryService : ICategoryService
{
    public Task<ResponseData<List<Category>>>  GetCategoryListAsync()
    {
        var categories = new List<Category>
        {
            new Category {Id=1, Name="Cakes", NormalizedName="cakes"},
            new Category {Id=2, Name="Buns", NormalizedName="buns"},
            new Category {Id=2, Name="Pies", NormalizedName="pies"},
            new Category {Id=2, Name="Donuts", NormalizedName="donuts"}
        };
        var result = new ResponseData<List<Category>>();
        result.Data=categories;
        return Task.FromResult(result);
    }
}