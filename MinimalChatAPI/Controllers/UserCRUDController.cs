using System.Security.Claims;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalChatAPI.Exception;
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
            var ExtractedId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //int callingUserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            int callingUserId = Convert.ToInt32(ExtractedId);

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



            try
            {
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
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditMessage([FromBody] EditMessageRequestDTO editDTO)
        {
            var ExtractedId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = Convert.ToInt32(ExtractedId);

            if (editDTO == null || string.IsNullOrWhiteSpace(editDTO.Content))
            {
                return BadRequest(new { message = "Message content is required." });
            }

            var message = new Message
            {
                Id = editDTO.MessageId,
                Content = editDTO.Content,
            };

            message = await userRepository.EditMessageAsync(message);

            if (message == null)
            {
                return NotFound(new { message = "Message not found." });
            }

            if (message.SenderId != userId)
            {
                return Unauthorized(new { message = "You are not authorized to edit this message." });
            }

            return Ok(new { message = "Message edited successfully." });
        }


        [HttpDelete]
        [Route("{MessageId:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteMessage([FromRoute] int MessageId)
        {
            var ExtractedId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = Convert.ToInt32(ExtractedId);

            var message = await userRepository.DeleteMessageAsync(MessageId);

            if (message == null)
            {
                return NotFound(new { message = "Message not found." });
            }

            if (message.SenderId != userId)
            {
                return Unauthorized(new { message = "You are not authorized to Delete this message." });
            }


            return Ok(new { message = "Message Deleted successfully." });
        }

        [HttpGet]
        [Route("messages")]
        [Authorize]
        public async Task<IActionResult> GetConversationHistory([FromQuery] ConversationHistoryRequestDto requestDto)
        {
            if (requestDto == null)
            {
                return BadRequest(" Invalid request parameters");
            }

            var ExtractedId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var newMessage = new Message
            {
                SenderId = Convert.ToInt32(ExtractedId),
                ReceiverId = requestDto.UserId,
                //Timestamp = (DateTime)requestDto.Before,
           
            };

            var conversations = await userRepository.GetConversationHistoryAsync(newMessage);

            if (requestDto.Before.HasValue)
            {
                conversations = conversations.Where(m => m.Timestamp < requestDto.Before.Value);
            }
            else
            {
                conversations = conversations.Where(m => m.Timestamp <=  DateTime.Now);
            }

            // Apply sorting
            if (requestDto.Sort.ToLower() == "desc")
            {
                conversations = conversations.OrderByDescending(m => m.Timestamp);
            } 
            else
            {
                conversations = conversations.OrderBy(m => m.Timestamp);
            }

            // Limit the number of messages to be retrieved
            conversations = conversations.Take(requestDto.Count);

            var response = new List<ConversationHistoryResponseDto>();

            foreach (var conversation in conversations)
                  

            {
                response.Add(new ConversationHistoryResponseDto
                {
                    Id = conversation.Id,
                    SenderId = conversation.SenderId,
                    ReceiverId = conversation.ReceiverId,
                    Content = conversation.Content,
                    Timestamp = conversation.Timestamp
                });
            }

            return Ok(response);

        }
    }
}
