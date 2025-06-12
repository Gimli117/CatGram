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

        [BindProperty]
        public string MessageText { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SelectedCatId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ChatWithCatId { get; set; }
        public CatProfile? SelectedCat { get; set; }
        public CatProfile? ChatWithCat { get; set; }

        public List<ChatMessage> Messages { get; set; } = new();

        public List<CatProfile> MyCats { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToPage("/Identity/Account/Register");

            MyCats = await _context.CatProfiles
                .Where(c => c.ApplicationUserId == currentUser.Id)
                .ToListAsync();

            var allOtherCats = await _context.CatProfiles
                .Where(c => c.ApplicationUserId != currentUser.Id)
                .ToListAsync();

            if (SelectedCatId != 0)
            {
                SelectedCat = await _context.CatProfiles
                    .FirstOrDefaultAsync(c => c.Id == SelectedCatId && c.ApplicationUserId == currentUser.Id);
            }

            if (ChatWithCatId != 0)
            {
                ChatWithCat = await _context.CatProfiles
                    .FirstOrDefaultAsync(c => c.Id == ChatWithCatId);
            }

            if (SelectedCat != null && ChatWithCat != null)
                {
                    Messages = await _context.ChatMessages
                        .Where(m => (m.SenderId == SelectedCat.Id && m.ReceiverId == ChatWithCat.Id)
                                 || (m.SenderId == ChatWithCat.Id && m.ReceiverId == SelectedCat.Id))
                        .OrderBy(m => m.SentAt)
                        .ToListAsync();
                }

            foreach (var otherCat in allOtherCats)
            {
                var commonFields = new List<string>();
                var matchingMyCats = new List<CatProfile>();

                foreach (var myCat in MyCats)
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

        public async Task<IActionResult> OnPostSendMessageAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToPage("/Identity/Account/Register");

            if (SelectedCatId != 0)
            {
                SelectedCat = await _context.CatProfiles
                    .FirstOrDefaultAsync(c => c.Id == SelectedCatId && c.ApplicationUserId == currentUser.Id);
            }

            if (ChatWithCatId != 0)
            {
                ChatWithCat = await _context.CatProfiles
                    .FirstOrDefaultAsync(c => c.Id == ChatWithCatId);
            }

            if (SelectedCat != null && ChatWithCat != null && !string.IsNullOrWhiteSpace(MessageText))
            {
                var message = new ChatMessage
                {
                    SenderId = SelectedCatId,
                    ReceiverId = ChatWithCatId,
                    Message = MessageText,
                    SentAt = DateTime.UtcNow
                };

                _context.ChatMessages.Add(message);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Discover", new { SelectedCatId, ChatWithCatId });
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