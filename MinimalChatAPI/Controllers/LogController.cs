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

        private readonly ILogRepository logRepository;

        public LogController(ILogRepository logRepository)
        {
            this.logRepository = logRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery] LogQueryParameters queryParameters)
        {
           

            try
            {
                var logs = await logRepository.GetLogsAsync(queryParameters);

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
                        Username = log.Username,

                        RequestBody = log.RequestBody,
                        Timestamp = log.Timestamp
                    });
                }

                return Ok(response);




            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}