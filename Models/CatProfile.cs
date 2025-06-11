using System.ComponentModel.DataAnnotations;

namespace CatGram.Models
{
    public class CatProfile
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Color { get; set; }
        public string? Breed { get; set; }
        public int? Age { get; set; }
        public string? FoodPreferences { get; set; }
        [Required]
        public string? ImagePath { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}