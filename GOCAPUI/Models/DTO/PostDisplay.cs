namespace GOCAPUI.Models.DTO;
public class UserDisplay
{
    public string? Picture { get; set; }
    public string Name { get; set; }
    public string Id { get; set; }
}
public enum MediaType
{
    Image = 0,
    Video = 1,
    File = 2,
    Document = 3,
    Audio = 4,
    Archive = 5,
    Spreadsheet = 6,
    Presentation = 7
}

public class MediaModel
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public MediaType Type { get; set; }
    public string? ThumbnailUrl { get; set; }
    public Guid PostId { get; set; }
}
public class Comment 
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string? Content { get; set; }
}
public class PostDisplay
{
    public string Id { get; set; }
    public string Content { get; set; }
    public UserDisplay User { get; set; }
    public List<Comment> Comments { get; set; } // Can be more specific depending on the comment structure
    public List<Media> Medias { get; set; }
    public long CreateTime { get; set; }

}