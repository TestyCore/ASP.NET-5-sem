using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Domain.Models;

namespace WEB_153503_Tatarinov.API.Services.ProductService;

public interface IProductService
{
    /// <summary>
    /// Receiving list of all products
    /// </summary>
    /// <param name="categoryNormalizedName">Normalized category name</param>
    /// <param name="pageNo">List page number</param>
    /// <param name="pageSize">Amount of items on each page</param>
    /// <returns>List of all products</returns>
    public Task<ResponseData<ListModel<Product>>> GetProductListAsync(
        string? categoryNormalizedName, int pageNo=1, int pageSize=3);
    
    /// <summary>
    /// Product search by id
    /// </summary>
    /// <param name="id">Product id</param>
    /// <returns>Found product or null</returns>
    public Task<ResponseData<Product>> GetProductByIdAsync(int id);
    
    /// <summary>
    /// Update product 
    /// </summary>
    /// <param name="id">Id of product to update</param>
    /// <param name="product">Product with updated data</param>
    /// <returns></returns>
    public Task UpdateProductAsync(int id, Product product);
    
    /// <summary>
    /// Remove product
    /// </summary>
    /// <param name="id">Id of product to remove</param>
    /// <returns></returns>
    public Task DeleteProductAsync(int id);
    
    /// <summary>
    /// Create product
    /// </summary>
    /// <param name="product">New prodcut</param>
    /// <returns>Created product</returns>
    public Task<ResponseData<Product>> CreateProductAsync(Product product);
    
    /// <summary>
    /// Save form file for product
    /// </summary>
    /// <param name="id">Product Id</param>
    /// <param name="formFile">Form file</param>
    /// <returns>Form file URL</returns>
    public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
}