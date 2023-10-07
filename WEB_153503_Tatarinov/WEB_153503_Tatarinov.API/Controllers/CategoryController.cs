using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153503_Tatarinov.API.Data;
using WEB_153503_Tatarinov.API.Services.CategoryService;
using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Domain.Models;

namespace WEB_153503_Tatarinov.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICategoryService _categoryService;

    public CategoryController(AppDbContext context, ICategoryService categoryService)
    {
        _context = context;
        _categoryService = categoryService;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<ResponseData<List<Category>>>> GetCategories()
    {
        var result = await _categoryService.GetCategoryListAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }
}

