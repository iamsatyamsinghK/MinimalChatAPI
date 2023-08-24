using MinimalChatAPI.Models.Domain;
using MinimalChatAPI.Models.DTO;

namespace MinimalChatAPI.Repositories.Interface
{
    public interface ILogRepository
    {
        //Task AddLogAsync(Log log);
        Task<List<Log>> GetLogsAsync(LogFilter filter);
    }
}
