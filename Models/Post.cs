using System.ComponentModel.DataAnnotations;

namespace CatGram.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string? Caption { get; set; } = string.Empty;
        [Required]
        public string? ImagePath { get; set; }
        public DateTime PostedAt { get; set; }
        public int CatProfileId { get; set; }
        public CatProfile? CatProfile { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}