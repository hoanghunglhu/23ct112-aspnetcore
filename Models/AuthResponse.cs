namespace LearnApiNetCore.Models
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public UserInfoResponse User { get; set; } = new UserInfoResponse();
    }

    public class UserInfoResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}