using GOCAPUI.Models.DTO.Post;
using GOCAPUI.Models.DTO.User;

namespace GOCAPUI.Models.ResponseJson
{
    public class PostResponse
    {
        public IEnumerable<PostModel>? Value { get; set; }
        public int? Count { get; set; }
    }
}
