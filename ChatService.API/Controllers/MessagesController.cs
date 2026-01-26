using ChatService.API.DTOs;
using ChatService.Application.Features.Messages.Commands;
using ChatService.Application.Features.Messages.Queries;
using ChatService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatService.API.Controllers;

[Authorize]
[ApiController]
[Route("Campus-Net/Messages")]
public class MessagesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IJwtRepository _jwtRepository;

    public MessagesController(IMediator mediator, IJwtRepository jwtRepository)
    {
        _mediator = mediator;
        _jwtRepository = jwtRepository;
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage(SendMessageRequest request)
    {
        var userId = _jwtRepository.GetUserId();
        var command = new SendMessageCommand(userId, request.recieverId, request.content);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("edit-message/{id}")]
    public async Task<IActionResult> EditMessage(long id, string content)
    {
        var userId = _jwtRepository.GetUserId();
        
        var command = new EditMessageCommand(userId, id, content);
        
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        return Ok();
    }

    [HttpDelete("delete-message/{id}")]
    public async Task<IActionResult> DeleteMessage(long id)
    {
       var userId = _jwtRepository.GetUserId();
       
       var command = new DeleteMessageCommand(userId, id);
       var result = await _mediator.Send(command);
       if (!result) return NotFound();
       return Ok();
    }

    [HttpGet("load-messages/{otherUserId}")]
    public async Task<IActionResult> LoadMessages(string otherUserId)
    {
        var userId = _jwtRepository.GetUserId();
        
        var query = new LoadMessagesQuery(userId, otherUserId);
        var messages = await _mediator.Send(query);
        return Ok(messages);
    }

    [HttpGet("load-all-messages/{otherUserId}")]
    public async Task<IActionResult> LoadFullMessages(string otherUserId)
    {
        var userId = _jwtRepository.GetUserId();
        
        var query = new LoadFullMessagesQuery(userId, otherUserId);
        var messages = await _mediator.Send(query);
        return Ok(messages);
    }
}
