using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153503_Tatarinov.Domain.Entities;
using WEB_153503_Tatarinov.Services.ProductService;

namespace WEB_153503_Tatarinov.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;

        public EditModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        [BindProperty] 
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            
            if (!response.Success)
            {
                return NotFound();
            }
            
            Product = response.Data!;

            return Page(); 
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _productService.UpdateProductAsync(Product.Id, Product, Image);

            return RedirectToPage("./Index");
        }

        private async Task<bool> ProductExists(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            return response.Success;
        }
    }
}
