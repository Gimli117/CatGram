namespace CatGram.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime PostedAt { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public int? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }

        public int PostId { get; set; }
        public Post? Post { get; set; }

        public List<Comment> Replies { get; set; } = new();
    }
}