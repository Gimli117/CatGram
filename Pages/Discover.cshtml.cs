using CatGram.Data;
using CatGram.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CatGram.Pages
{
    public class DiscoverModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DiscoverModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CatProfile> SuggestedCats { get; set; } = new();

        public async Task OnGetAsync()
        {
            var currentCat = await _context.CatProfiles.FindAsync(1);

            if (currentCat != null)
            {
                SuggestedCats = await _context.CatProfiles
                    .Where(c => c.Id != currentCat.Id && c.Color == currentCat.Color)
                    .ToListAsync();

                if (!SuggestedCats.Any())
                {
                    // Fallback: show 3 random cats
                    SuggestedCats = await _context.CatProfiles
                        .Where(c => c.Id != currentCat.Id)
                        .OrderBy(_ => Guid.NewGuid())
                        .Take(3)
                        .ToListAsync();
                }
            }
        }
    }
}