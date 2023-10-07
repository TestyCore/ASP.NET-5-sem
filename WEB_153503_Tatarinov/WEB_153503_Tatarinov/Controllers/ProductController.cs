using Microsoft.AspNetCore.Mvc;
using WEB_153503_Tatarinov.Services.CategoryService;
using WEB_153503_Tatarinov.Services.ProductService;

namespace WEB_153503_Tatarinov.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }
    // GET
    public async Task<IActionResult> Index(string? category, int pageNo = 1)
    {
        var categoryResponse = await _categoryService.GetCategoryListAsync();
        if(!categoryResponse.Success)
            return NotFound(categoryResponse.ErrorMessage);
        
        ViewData["categories"] = categoryResponse.Data;
        ViewData["selectedCategory"] = categoryResponse.Data?.SingleOrDefault(c => c.NormalizedName == category);
        
        var productResponce = await _productService.GetProductListAsync(category, pageNo);
        if (!productResponce.Success)
            return NotFound(productResponce.ErrorMessage);

        return View(productResponce.Data);
    }
}