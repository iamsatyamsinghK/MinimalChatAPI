using Microsoft.EntityFrameworkCore;
using MinimalChatAPI.Data;
using MinimalChatAPI.Migrations;
using MinimalChatAPI.Models.Domain;
using MinimalChatAPI.Repositories.Interface;

namespace MinimalChatAPI.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        //public UserRepository(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Message> EditMessageAsync(Message message)
        {
            var existingMessage = await dbContext.Messages.FirstOrDefaultAsync(x => x.Id == message.Id);
            if (existingMessage != null)
            {
                
                existingMessage.Content = message.Content;
                await dbContext.SaveChangesAsync();

                return existingMessage;
            }

            return null;
        }

        

        public async Task<IEnumerable<User>> GetUserListAsync(int callingUserId)
        {
            return await dbContext.Users.Where(u => u.Id != callingUserId).ToListAsync();
        }

        public async Task<Message> SendMessageAsync(Message newMessage)
        {

            await dbContext.Messages.AddAsync(newMessage);
            await dbContext.SaveChangesAsync();

            return newMessage;
        }

       
    }
}
