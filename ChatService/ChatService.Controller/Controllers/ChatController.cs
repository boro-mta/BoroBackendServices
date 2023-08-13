using Boro.Common.Authentication;
using Boro.Validations;
using ChatService.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChatService.Controller.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IChatBackend _chatBackend;

    public ChatController(ILoggerFactory loggerFactory, IChatBackend chatBackend)
    {
        _logger = loggerFactory.CreateLogger("ChatService");
        _chatBackend = chatBackend;
    }

    [HttpPost("Start/With/{recepientId}")]
    [ValidatesGuid("recepientId")]
    public async Task<ActionResult> StartChat(string recepientId, [FromBody] string message)
    {
        var userId = User.UserId();
        var recepientGuid = Guid.Parse(recepientId);

        await _chatBackend.SendMessage(userId, recepientGuid, message);

        return Ok();
    }

}


