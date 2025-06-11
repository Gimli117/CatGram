using CatGram.Data;
using CatGram.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CatGram.Pages
{
    public class DiscoverModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DiscoverModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<(CatProfile Cat, List<string> CommonFields, List<CatProfile> MatchingMyCats)> MatchingCats { get; set; } = new();


        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToPage("/Account/Login");

            var myCats = await _context.CatProfiles
                .Where(c => c.ApplicationUserId == currentUser.Id)
                .ToListAsync();

            var allOtherCats = await _context.CatProfiles
                .Where(c => c.ApplicationUserId != currentUser.Id)
                .ToListAsync();

            foreach (var otherCat in allOtherCats)
            {
                var commonFields = new List<string>();
                var matchingMyCats = new List<CatProfile>();

                foreach (var myCat in myCats)
                {
                    bool matched = false;

                    if (otherCat.Breed == myCat.Breed && !commonFields.Contains("Breed"))
                    {
                        commonFields.Add("Breed");
                        matched = true;
                    }

                    if (!string.IsNullOrEmpty(otherCat.Color) && otherCat.Color == myCat.Color && !commonFields.Contains("Color"))
                    {
                        commonFields.Add("Color");
                        matched = true;
                    }

                    if (FoodPreferenceOverlap(myCat.FoodPreferences!, otherCat.FoodPreferences!) && !commonFields.Contains("Food Preferences"))
                    {
                        commonFields.Add("Food Preferences");
                        matched = true;
                    }

                    if (matched)
                    {
                        matchingMyCats.Add(myCat);
                    }
                }

                if (commonFields.Any())
                    MatchingCats.Add((otherCat, commonFields, matchingMyCats));
            }

            return Page();
        }

        private bool FoodPreferenceOverlap(string myFood, string theirFood)
        {
            if (string.IsNullOrEmpty(myFood) || string.IsNullOrEmpty(theirFood)) return false;

            var myList = myFood.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var theirList = theirFood.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            return myList.Intersect(theirList, StringComparer.OrdinalIgnoreCase).Any();
        }
    }
}