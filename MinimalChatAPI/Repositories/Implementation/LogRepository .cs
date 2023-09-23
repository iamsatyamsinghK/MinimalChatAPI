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
            // Get the Indian Standard Time (IST) timezone
            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            // Convert the current time to IST
            DateTime currentTimeUtc = DateTime.UtcNow;
            DateTime currentTimeIST = TimeZoneInfo.ConvertTimeFromUtc(currentTimeUtc, istTimeZone);

            // Convert the start time and end time to IST, if provided
            DateTime startTimeIST = queryParameters.StartTime.HasValue
                ? TimeZoneInfo.ConvertTimeFromUtc(queryParameters.StartTime.Value, istTimeZone)
                : currentTimeIST;

            DateTime endTimeIST = queryParameters.EndTime.HasValue
                ? TimeZoneInfo.ConvertTimeFromUtc(queryParameters.EndTime.Value, istTimeZone)
                : currentTimeIST;

            var logs = await _dbContext.Logs
                .Where(log => log.Timestamp >= startTimeIST && log.Timestamp <= endTimeIST)
                .ToListAsync();

            return logs;
        }
    }
}