using System.Text;
using System.Text.Json;
using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Domain.Models;

namespace WEB_153503_Tatarinov.Services.ProductService;

public class ApiProductService : IProductService
{
    private HttpClient _httpClient;
    private string? _pageSize;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<ApiProductService> _logger;
    
    public ApiProductService(HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ApiProductService> logger)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetSection("ItemsPerPage").Value;
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;
    }
    
    public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}product/");
       
        if (categoryNormalizedName != null)
        {
            urlString.Append($"{categoryNormalizedName}/");
        };
        
        if (pageNo > 1)
        {
            urlString.Append($"{pageNo}");
        };
        
        if (!_pageSize.Equals("3"))
        {
            urlString.Append(QueryString.Create("pageSize", _pageSize).ToString());
        }
        
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if(response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Product>>>(_serializerOptions);
            }
            catch(JsonException ex)
            {
                _logger.LogError($"-----> Error: {ex.Message}");
                return new ResponseData<ListModel<Product>>
                {
                    Success = false,
                    ErrorMessage = $"Error: {ex.Message}"
                };
            }
        }
        _logger.LogError($"-----> Could not recieve data from the server. Error:{response.StatusCode}");
        return new ResponseData<ListModel<Product>> {
            Success = false,
            ErrorMessage = $"Could not recieve data from the server. Error:{response.StatusCode}"
        };
    }
    
    public async Task<ResponseData<Product>> CreateProductAsync(
        Product product,
        IFormFile? formFile)
    {
        var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Product");

        var response = await _httpClient.PostAsJsonAsync(
            uri,
            product,
            _serializerOptions);
        if (response.IsSuccessStatusCode)
        {
            var data = await response
                .Content
                .ReadFromJsonAsync<ResponseData<Product>>
                    (_serializerOptions);

            return data; // product;
        }
        _logger
            .LogError($"-----> object not created. Error:{response.StatusCode}");
        return new ResponseData<Product>
        {
            Success = false,
            ErrorMessage = $"Object is not added. Error:{response.StatusCode}"
        };
    }
    
    public async Task DeleteProductAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Product/{id}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"-----> Could not retrieve data from the server. Error: {response.StatusCode}");
        }
    }

    public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Product/product{id}");
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Error: {ex.Message}");
                return new ResponseData<Product>
                {
                    Success = false,
                    ErrorMessage = $"Error: {ex.Message}"
                };
            }
        }
        _logger.LogError($"-----> Could not retrieve data from the server. Error: {response.StatusCode}");
        return new ResponseData<Product>()
        {
            Success = false,
            ErrorMessage = $"Could not retrieve data from the server. Error: {response.StatusCode}"
        };
    }

    
    public async Task UpdateProductAsync(int id, Product tool, IFormFile? formFile)
    {
        var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "Product/" + id);
        var response = await _httpClient.PutAsJsonAsync(uri, tool, _serializerOptions);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"-----> Could not retrieve data from the server. Error: {response.StatusCode}");
        }
        else if (formFile != null) 
        {
            int toolId = (await response.Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions))!.Data!.Id;
            await SaveImageAsync(toolId, formFile);
        }
    }
    
    private async Task SaveImageAsync(int id, IFormFile image)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{_httpClient.BaseAddress?.AbsoluteUri}Product/{id}")
        };
        var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(image.OpenReadStream());
        content.Add(streamContent, "formFile", image.FileName);
        request.Content = content;
        await _httpClient.SendAsync(request);
    }
}