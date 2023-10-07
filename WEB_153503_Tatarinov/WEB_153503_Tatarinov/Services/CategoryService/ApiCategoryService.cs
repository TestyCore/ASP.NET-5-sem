using System.Text;
using System.Text.Json;
using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Domain.Models;

namespace WEB_153503_Tatarinov.Services.CategoryService;

public class ApiCategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiCategoryService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;

    public ApiCategoryService(HttpClient httpClient, ILogger<ApiCategoryService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress?.AbsoluteUri}category/");
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Error: {ex.Message}");
                return new ResponseData<List<Category>>
                {
                    Success = false,
                    ErrorMessage = $"Error: {ex.Message}"
                };

            }
        }
        _logger.LogError($"-----> Could not recieve data from the server. Error:{response.StatusCode}");
        return new ResponseData<List<Category>>()
        {
            Success = false,
            ErrorMessage = $"Could not recieve data from the server. Error:{response.StatusCode}"
        };
    }
}