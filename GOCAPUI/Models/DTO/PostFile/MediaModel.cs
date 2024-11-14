namespace GOCAPUI.Models.DTO.Post;

public class MediaModel
{
    public Guid? Id { get; set; }
    public string? Url { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string? ThumbnailUrl { get; set; }
    public Guid? PostId { get; set; }
}
