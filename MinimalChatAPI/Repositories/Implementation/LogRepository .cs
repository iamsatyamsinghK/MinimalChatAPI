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
        //public async Task AddLogAsync(Log log)
        //{
        //    _dbContext.Logs.Add(log);
        //    await _dbContext.SaveChangesAsync();
        //}

       
            public async Task<List<Log>> GetLogsAsync(LogFilter filter)
            {
                 var logs = await _dbContext.Logs
                        .Where(log => log.Timestamp >= filter.StartTime && log.Timestamp <= filter.EndTime)
                        .ToListAsync();

                        return logs;



            //var logs = await _dbContext.Logs
            //    .Where(log => log.Timestamp >= filter.StartTime && log.Timestamp <= filter.EndTime)
            //    .ToListAsync();



            //var logDtos = logs.Select(log => new LogDto
            //{
            //    IpAddress = log.IpAddress,
            //    Username = log.Username,
            //    RequestPath = log.RequestPath,
            //    RequestBody = log.RequestBody,
            //    Timestamp = log.Timestamp
            //}).ToList();

            // return logs;


            
        }
    }
}
