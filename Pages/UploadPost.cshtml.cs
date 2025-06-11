using CatGram.Data;
using CatGram.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CatGram.Pages
{
    public class UploadPostModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public UploadPostModel(ApplicationDbContext context, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        //[BindProperty]
        //public string Caption { get; set; }

        [BindProperty]
        public IFormFile ImageUpload { get; set; }

        [BindProperty]
        public Post Post { get; set; }

        public SelectList UserCats { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var cats = await _context.CatProfiles
                .Where(c => c.ApplicationUserId == user!.Id)
                .ToListAsync();

            UserCats = new SelectList(cats, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (/*!ModelState.IsValid || */ImageUpload == null || Post.CatProfileId == 0)
            {
                ModelState.AddModelError("", "Please select a cat, upload an image, and fill out all fields.");
                return Page();
            }

            // Get the selected cat
            var cat = await _context.CatProfiles
                .FirstOrDefaultAsync(c => c.Id == Post.CatProfileId && c.ApplicationUserId == user!.Id);

            if (cat == null)
            {
                ModelState.AddModelError("", "Invalid cat selected.");
                return Page();
            }

            // Create /uploads folder if it doesn't exist
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            // Save image
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(ImageUpload.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageUpload.CopyToAsync(stream);
            }

            // Create post
            Post.ImagePath = $"/uploads/{fileName}";
            Post.PostedAt = DateTime.UtcNow;
            Post.CatProfileId = cat.Id;

            _context.Posts.Add(Post);

            await _context.SaveChangesAsync();  // ← if this fails silently, nothing gets saved

            return RedirectToPage("/MainFlow");
        }
    }
}