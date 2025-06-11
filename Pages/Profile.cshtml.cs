using CatGram.Data;
using CatGram.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CatGram.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileModel(ApplicationDbContext context, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        [BindProperty]
        public CatProfile CatProfile { get; set; } = new();

        [BindProperty]
        public IFormFile? ImageUpload { get; set; }

        public List<CatProfile> CatProfiles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            CatProfiles = await _context.CatProfiles
                .Where(c => c.ApplicationUserId == user.Id)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            // Save image
            string? imagePath = null;
            if (ImageUpload != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageUpload.FileName)}";
                var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                using var stream = new FileStream(filePath, FileMode.Create);
                await ImageUpload.CopyToAsync(stream);
                imagePath = $"/images/{fileName}";
            }

            CatProfile.ApplicationUserId = user.Id;
            CatProfile.ImagePath = imagePath;

            _context.CatProfiles.Add(CatProfile);
            await _context.SaveChangesAsync();

            return RedirectToPage(); // reload profile
        }
    }
}