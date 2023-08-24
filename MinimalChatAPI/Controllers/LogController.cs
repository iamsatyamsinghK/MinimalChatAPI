using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalChatAPI.Models.Domain;
using MinimalChatAPI.Models.DTO;
using MinimalChatAPI.Repositories.Interface;
using System;


namespace MinimalChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {

        private readonly ILogRepository _logRepository;

        public LogController(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery] LogQueryParameters queryParameters)
        {
            var filter = new LogFilter
            {
                StartTime = queryParameters.StartTime,
                EndTime = queryParameters.EndTime
            };

            try
            {
                var logs = await _logRepository.GetLogsAsync(filter);

                if (logs == null || logs.Count == 0)
                {
                    return NotFound("No logs found.");
                }
                var response = new List<LogDto>();

                foreach (var log in logs)
                {
                    response.Add(new LogDto
                    {
                        IpAddress = log.IpAddress,
                        //Username = log.Username,
                        //RequestPath = log.RequestPath,
                        RequestBody = log.RequestBody,
                        Timestamp = log.Timestamp
                    });
                }

                return Ok(response);



                //return Ok(logs);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
