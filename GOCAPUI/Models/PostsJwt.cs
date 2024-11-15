using GOCAPUI.Models.DTO;

namespace GOCAPUI.Models;

public class PostsJwt
{
    public List<PostDisplay> PostDisplays { get; set; } = [];
    public string JwtToken { get; set; } = "";
    public User User { get; set; }
}
