using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimalChatAPI.Models.Domain;
using MinimalChatAPI.Models.DTO;
using MinimalChatAPI.Repositories.Interface;

namespace MinimalChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCRUDController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserCRUDController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]

        [Authorize]
        public async Task<IActionResult> GetUserList()
        {
            int callingUserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);


            var users = await userRepository.GetUserListAsync(callingUserId);

            var response = new List<UserProfileDto>();

            foreach (var user in users)
            {
                response.Add(new UserProfileDto
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email
                });
            }

            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequestDto messageDTO)
        {
            if (messageDTO == null || string.IsNullOrWhiteSpace(messageDTO.Content))
            {
                return BadRequest("Message content is required.");
            }

            var ExtractedId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var newMessage = new Message
            {
                SenderId = Convert.ToInt32(ExtractedId),
                ReceiverId = messageDTO.ReceiverId,
                Content = messageDTO.Content,
                Timestamp = DateTime.UtcNow
            };

            await userRepository.SendMessageAsync(newMessage);

            var response = new SendMessageResponseDto
            {
                MessageId = newMessage.Id,
                SenderId = newMessage.SenderId,
                ReceiverId = newMessage.ReceiverId,
                Content = newMessage.Content,
                Timestamp = newMessage.Timestamp
            };

            return Ok(response);
        }


        [HttpPut]
        public async Task<IActionResult> EditMessage([FromBody] EditMessageRequestDTO editDTO)
        {
            var ExtractedId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = Convert.ToInt32(ExtractedId);

            if (editDTO == null || string.IsNullOrWhiteSpace(editDTO.Content))
            {
                return BadRequest("Message content is required.");
            }

            var message = new Message
            {
                
                Id = editDTO.MessageId,
                Content = editDTO.Content,
            };

            message = await userRepository.EditMessageAsync(message);

            if (message == null)
            {
                return NotFound("Message not found.");
            }

            if (message.SenderId != userId)
            {
                return Unauthorized("You are not authorized to edit this message.");
            }


            return Ok("Message edited successfully.");

        }
    }
}
