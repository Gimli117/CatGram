using CatGram.Data;
using CatGram.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CatGram.Pages
{
    public class MainFlowModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MainFlowModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Post> Posts { get; set; }

        public async Task OnGetAsync()
        {
            Posts = await _context.Posts
                .Include(p => p.CatProfile)
                    .OrderByDescending(p => p.PostedAt)
                        .ToListAsync();
        }
    }
}