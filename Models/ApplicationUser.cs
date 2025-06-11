using Microsoft.AspNetCore.Identity;

namespace CatGram.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<CatProfile>? CatProfiles { get; set; }
    }
}