using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Domain.Models;

namespace WEB_153503_Tatarinov.API.Services.CategoryService;

public interface ICategoryService
{
    /// <summary>
    /// Receiving list of categories
    /// </summary>
    /// <returns></returns>
    public Task<ResponseData<List<Category>>> GetCategoryListAsync();
}