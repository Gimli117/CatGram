namespace CatGram.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string? SenderId { get; set; }
        public string? RecieverId { get; set; }
        public string? Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}