namespace GOCAPUI.Models.DTO.User;

public class UserModel
{
    public Guid Id { get; set; } 
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Image { get; set; }
    public string? Bio { get; set; }
    public long? CreateTime { get; set; }
    public long? LastModifyTime { get; set; }
}
