using Azure.Core;
using Microsoft.EntityFrameworkCore;
using MinimalChatAPI.Data;
using MinimalChatAPI.Exception;
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

        public async Task<Message> DeleteMessageAsync(int MessageId)
        {
            var existingMessage = await dbContext.Messages.FirstOrDefaultAsync(x => x.Id == MessageId);
            if (existingMessage != null)
            {
                dbContext.Messages.Remove(existingMessage);
                await dbContext.SaveChangesAsync();
                return existingMessage;
                
            }

            return null;
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

        public async Task<IEnumerable<Message>> GetConversationHistoryAsync(Message newMessage)
        {
            var conversation = await dbContext.Messages
            .Where(m => ((m.SenderId == newMessage.SenderId && m.ReceiverId == newMessage.ReceiverId) ||
                         (m.SenderId == newMessage.ReceiverId && m.ReceiverId == newMessage.SenderId)))
            .ToListAsync();

            return conversation;
        }

        public async Task<IEnumerable<User>> GetUserListAsync(int callingUserId)
        {
            return await dbContext.Users.Where(u => u.Id != callingUserId).ToListAsync();
        }

        public async Task<Message> SendMessageAsync(Message newMessage)
        {
            var receiverExists = dbContext.Users.Any(r => r.Id == newMessage.ReceiverId);

            if (!receiverExists)
            {
                throw new NotFoundException("Receiver not found");
            }

            await dbContext.Messages.AddAsync(newMessage);
            await dbContext.SaveChangesAsync();

            return newMessage;
        }

       
    }
}
