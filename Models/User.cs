namespace MinimalApiUsers.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Atribuindo um valor padrão
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Atribuindo um valor padrão
    }
}
