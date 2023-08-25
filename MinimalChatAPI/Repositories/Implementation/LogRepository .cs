using Microsoft.EntityFrameworkCore;
using MinimalChatAPI.Data;
using MinimalChatAPI.Models.Domain;
using MinimalChatAPI.Models.DTO;
using MinimalChatAPI.Repositories.Interface;

namespace MinimalChatAPI.Repositories.Implementation
{
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LogRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Log>> GetLogsAsync(LogQueryParameters queryParameters)
        {
            DateTime currentTime = DateTime.Now;
            DateTime defaultStartTime = currentTime.AddMinutes(-5);

            DateTime startTime = queryParameters.StartTime ?? defaultStartTime;
            DateTime endTime = queryParameters.EndTime ?? currentTime;

            var logs = await _dbContext.Logs
                .Where(log => log.Timestamp >= startTime && log.Timestamp <= endTime)
                .ToListAsync();

            return logs;
        }
    }
}
