using System.ComponentModel.DataAnnotations;

public class News
{
    [Key]
    public string NewsId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}