using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatService.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace ChatService.Infrastructure.Jwt
{
    public class JwtRepository : IJwtRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity!.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated");

            var userId = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("UserId (sub) claim missing");

            return userId;
        }
    }
}
