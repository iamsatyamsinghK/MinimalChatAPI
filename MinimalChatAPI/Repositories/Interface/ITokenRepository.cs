using Microsoft.AspNetCore.Identity;
using MinimalChatAPI.Models.Domain;

namespace MinimalChatAPI.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(User user);
    }
}
