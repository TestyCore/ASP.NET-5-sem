using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Services.ProductService;

namespace WEB_153503_Tatarinov.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        
        public CreateModel(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        
        [BindProperty]
        public IFormFile Image { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }
          
          var response = await _productService.CreateProductAsync(Product, Image);
          if (!response.Success)
          {
              return Page();
          }
            
          return RedirectToPage("./Index");
        }
    }
}
