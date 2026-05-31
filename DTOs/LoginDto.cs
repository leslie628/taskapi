namespace TaskManagerApi.DTOs
{
    public class LoginDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string display_name { get; set; }
    }
}
