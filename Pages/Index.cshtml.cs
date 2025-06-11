using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatGram.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/Profile");
        }
    }
}
