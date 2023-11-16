using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Services.ProductService;

namespace WEB_153503_Tatarinov.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IList<Product> Products { get;set; } = default!;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }


        public async Task<IActionResult> OnGetAsync(int pageNo = 1)
        {
            var response = await _productService.GetProductListAsync(null, pageNo);
            
            if (!response.Success)
            {
                return NotFound(response.ErrorMessage ?? "");
            }
            
            Products = response.Data?.Items!;
            CurrentPage = response.Data?.CurrentPage ?? 0;
            TotalPages = response.Data?.TotalPages ?? 0;

            return Page();
        }
    }
}
