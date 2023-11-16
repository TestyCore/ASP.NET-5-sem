using Microsoft.AspNetCore.Mvc;
using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Domain.Models;
using WEB_153503_Tatarinov.Services.CategoryService;
using WEB_153503_Tatarinov.Services.ProductService;
using WEB_153503_Tatarinov.Extensions;

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
        
        var productResponse = await _productService.GetProductListAsync(category, pageNo);
        if (!productResponse.Success)
            return NotFound(productResponse.ErrorMessage);

        if (Request.IsAjaxRequest())
        {
            ListModel<Product> data = productResponse.Data!;
            return PartialView("_ProductPartial", new
            {
                data.Items,
                data.CurrentPage,
                data.TotalPages,
                CategoryNormalizedName = category
            });
        }
        else
        {
            return View(productResponse.Data); 
        }
    }
}