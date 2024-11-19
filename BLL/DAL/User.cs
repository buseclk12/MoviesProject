namespace BLL.DAL
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; } 
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; } = string.Empty; 
    }
}