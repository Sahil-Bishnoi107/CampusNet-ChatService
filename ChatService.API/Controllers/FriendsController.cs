using ChatService.API.DTOs;
using ChatService.Application.Features.UserRelations.Commands;
using ChatService.Application.Features.UserRelations.Queries;
using ChatService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatService.API.Controllers;

[Authorize]
[ApiController]
[Route("Campus-net/social/Friends")]
public class FriendsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IJwtRepository _jwtRepository;

    public FriendsController(IMediator mediator, IJwtRepository jwtRepository)
    {
        _mediator = mediator;
        _jwtRepository = jwtRepository;
    }

    [HttpPost("send-request")]
    public async Task<IActionResult> SendFriendRequest(FriendRequestDto request)
    {
        var userId = _jwtRepository.GetUserId();
        
        var command = new SendFriendRequestCommand(userId, request.TargetUserId);
        var result = await _mediator.Send(command);
        if (!result) return BadRequest("Friend request failed (already friends or pending).");
        return Ok();
    }

    [HttpPost("accept-request")]
    public async Task<IActionResult> AcceptFriendRequest(FriendRequestActionDto request)
    {
        var command = new AcceptFriendRequestCommand(request.RequesterId);
        var result = await _mediator.Send(command);
        if (!result) return BadRequest("Acceptance failed.");
        return Ok();
    }

    [HttpPost("decline-request")]
    public async Task<IActionResult> DeclineFriendRequest(FriendRequestActionDto request)
    {
        var userId = _jwtRepository.GetUserId();
        
        var command = new DeclineFriendRequestCommand(userId, request.RequesterId);
        var result = await _mediator.Send(command);
        if (!result) return BadRequest("Action failed.");
        return Ok();
    }
    
    [HttpPost("block")]
    public async Task<IActionResult> BlockUser(BlockUserDto request)
    {
        var userId = _jwtRepository.GetUserId();
        
        var command = new BlockUserCommand(userId, request.BlockedUserId);
        var result = await _mediator.Send(command);
        return Ok();
    }

    [HttpGet("get-friends")]
    public async Task<IActionResult> GetFriends()
    {
        var userId = _jwtRepository.GetUserId();
        
        var query = new LoadFriendsQuery(userId);
        var friends = await _mediator.Send(query);
        return Ok(friends);
    }

    [HttpGet("get-mutual-friends/{otherUserId}")]
    public async Task<IActionResult> GetMutualFriends(string otherUserId)
    {
        var userId = _jwtRepository.GetUserId();
        
        var query = new GetMutualFriendsQuery(userId, otherUserId);
        var mutuals = await _mediator.Send(query);
        return Ok(mutuals);
    }

    [HttpGet("friend-requests")]
    public async Task<IActionResult> GetFriendRequests()
    {
        var requests = await _mediator.Send(new AllFriendRequestsQuery());
        return Ok(requests);
    }
}
