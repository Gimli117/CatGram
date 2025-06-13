using CatGram.Data;
using CatGram.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CatGram.Pages
{
    public class MainFlowModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MainFlowModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Post> Posts { get; set; } = new();

        [BindProperty]
        public int PostId { get; set; }

        [BindProperty]
        public new string Content { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            Posts = await _context.Posts
                .Include(p => p.CatProfile)
                .Include(p => p.Comments)
                .ThenInclude(c => c.ApplicationUser)
                .OrderByDescending(p => p.PostedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            var comment = new Comment
            {
                PostId = PostId,
                Content = Content,
                ApplicationUserId = user.Id,
                PostedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}