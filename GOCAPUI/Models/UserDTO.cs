namespace GOCAPUI.Models
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public string Status { get; set; }
    }

    public class UserCreationDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Image { get; set; }
        public string? Bio { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public string Status { get; set; }
    }
}
