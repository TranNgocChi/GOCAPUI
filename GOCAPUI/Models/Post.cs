using GOCAPUI.Models.DTO;

namespace GOCAPUI.Models;

public class Post
{
    public string Id { get; set; }
    public string Content { get; set; }
    public User User { get; set; }
    public List<Media> Medias { get; set; }
    public List<Comment> Comments { get; set; }
    public long CreateTime { get; set; }
}

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Picture { get; set; }
    public List<Post> Posts { get; set; }
    public List<string> Followers { get; set; }
    public List<string> Following { get; set; }
    public List<string> Notifications { get; set; }
    public List<string> UserRoles { get; set; }
    public List<string> Likes { get; set; }
    public List<string> Comments { get; set; }
}

public class Media
{
    public string Id { get; set; }
    public string Url { get; set; }
    public string Content { get; set; }
    public MediaType Type { get; set; }  // 0: Image, 1: Video, etc.
    public string ThumbnailUrl { get; set; }
    public string PostId { get; set; }
}

public class Comment
{
    public string Id { get; set; }
    public string Content { get; set; }
}

