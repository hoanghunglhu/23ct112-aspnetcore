
using Microsoft.EntityFrameworkCore;
using LearnApiNetCore.Entity;


namespace LearnApiNetCore.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        bool ValidateToken(string token);
        int? GetUserIdFromToken(string token);
    }
}