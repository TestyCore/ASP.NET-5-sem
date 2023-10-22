using Microsoft.AspNetCore.Mvc;
using WEB_153503_Tatarinov.API.Data;
using WEB_153503_Tatarinov.API.Services.ProductService;
using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Domain.Models;

namespace WEB_153503_Tatarinov.API.Controllers;

    [Route("api/[controller]")]
    [ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

     // GET: api/Products
    [HttpGet("{pageNo:int}")]
    [HttpGet("{category?}/{pageNo:int?}")]
    public async Task<ActionResult<ResponseData<List<Product>>>> GetProducts(string? category, int pageNo = 1, int pageSize = 3)
    {
        var result = await _productService.GetProductListAsync(category, pageNo, pageSize);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET: api/Products/product5
    [HttpGet("product{id}")]
    public async Task<ActionResult<ResponseData<Product>>> GetProduct(int id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    // PUT: api/Products/5
 
    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseData<Product>>> PutProduct(int id, Product product)
    {
        try
        {
            await _productService.UpdateProductAsync(id, product);
        }
        catch (Exception ex)
        {
            return NotFound(new ResponseData<Product>()
            {
                Data = null,
                Success = false,
                ErrorMessage = ex.Message
            });
        }

        return Ok(new ResponseData<Product>()
        {
            Data = product,
        });
    }

    // POST: api/Products
    [HttpPost]
    public async Task<ActionResult<ResponseData<Product>>> PostProduct(Product product)
    {
        var result = await _productService.CreateProductAsync(product);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
        }
        catch (Exception ex)
        {
            return NotFound(new ResponseData<Product>()
            {
                Data = null,
                Success = false,
                ErrorMessage = ex.Message
            });
        }

        return NoContent();
    }

    
    // POST: api/Products/5
    [HttpPost("{id}")]
    public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
    {
        var response = await _productService.SaveImageAsync(id, formFile);
        
        if (response.Success)
        {
            return Ok(response);
        }
        
        return NotFound(response);
    }
    
    private async Task<bool> ProductExists(int id)
    {
        return (await _productService.GetProductByIdAsync(id)).Success;
    }
}

