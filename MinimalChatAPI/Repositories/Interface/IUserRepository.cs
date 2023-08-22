using MinimalChatAPI.Models.Domain;

namespace MinimalChatAPI.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUserListAsync(int callingUserId);

        Task<Message> SendMessageAsync(Message newMessage);
        
        Task<Message> EditMessageAsync( Message message);


    }
}
