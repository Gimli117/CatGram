using CatGram.Data;
using CatGram.Models;
using Microsoft.AspNetCore.Authorization;
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

        [BindProperty]
        public List<CatProfile> CatProfiles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Identity/Account/Register");

            CatProfiles = await _context.CatProfiles
                .Where(c => c.ApplicationUserId == user.Id)
                .Include(c => c.Posts) // Eager-load related posts
                .ToListAsync();

            // Optional: sort posts by date if needed per profile
            foreach (var cat in CatProfiles)
            {
                cat.Posts = cat.Posts.OrderByDescending(p => p.PostedAt).ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Identity/Account/Register");

            // Update existing cats
            foreach (var updatedCat in CatProfiles)
            {
                var catInDb = await _context.CatProfiles.FindAsync(updatedCat.Id);
                if (catInDb != null && catInDb.ApplicationUserId == user.Id)
                {
                    catInDb.Name = updatedCat.Name;
                    catInDb.Breed = updatedCat.Breed;
                    catInDb.Color = updatedCat.Color;
                    catInDb.Age = updatedCat.Age;
                    catInDb.FoodPreferences = updatedCat.FoodPreferences;
                    // Image is not updated here to keep existing image
                }
            }

            // Add new cat with image
            if (CatProfile != null && !string.IsNullOrWhiteSpace(CatProfile.Name))
            {
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
            }

            await _context.SaveChangesAsync();
            return RedirectToPage(); // Reload with latest data
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Identity/Account/Register");

            foreach (var cat in CatProfiles)
            {
                var existingCat = await _context.CatProfiles
                    .FirstOrDefaultAsync(c => c.Id == cat.Id && c.ApplicationUserId == user.Id);

                if (existingCat != null)
                {
                    existingCat.Name = cat.Name;
                    existingCat.Breed = cat.Breed;
                    existingCat.Color = cat.Color;
                    existingCat.Age = cat.Age;
                    existingCat.FoodPreferences = cat.FoodPreferences;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Identity/Account/Register");

            var cat = await _context.CatProfiles
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Id == id && c.ApplicationUserId == user.Id);

            if (cat != null)
            {
                // Optionally: delete image file from server
                if (!string.IsNullOrEmpty(cat.ImagePath))
                {
                    var fullPath = Path.Combine(_environment.WebRootPath, cat.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                _context.CatProfiles.Remove(cat); // Cascade deletes posts
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}