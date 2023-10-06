using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Domain.Models;

namespace WEB_153503_Tatarinov.Services.CategoryService;

public interface ICategoryService
{
    /// <summary>
    /// Receiving list of categories
    /// </summary>
    /// <returns>List of categories</returns>
    public Task<ResponseData<List<Category>>> GetCategoryListAsync();
}