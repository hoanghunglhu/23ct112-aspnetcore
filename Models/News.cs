namespace LearnAspNetCore.Models // <-- Đảm bảo namespace này là đúng
{
    public class New
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}