using CORE.Authentication.Models;
using CORE.Authentication.Services;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Authentication
{
    public class TokenRequest : JwtRequest, IRequest<JwtResponse>
    {
    }

    public class TokenHandler : DbService<User>,
        IRequestHandler<TokenRequest, JwtResponse>
    {
        private readonly IJwtAuthService _jwtAuthService;

        public TokenHandler(DbContext db, IJwtAuthService jwtAuthService) : base(db)
        {
            _jwtAuthService = jwtAuthService;
        }

        public async Task<JwtResponse> Handle(TokenRequest request, CancellationToken cancellationToken)
        {
            var userEntity = await DbQuery().SingleOrDefaultAsync(
                u => u.UserName == request.UserName && 
                u.Password == request.Password &&
                u.StatusId == 1, cancellationToken);
            if (userEntity is null)
                return null;
            userEntity.RefreshToken = _jwtAuthService.GetRefreshToken();
            // sliding expiration
            //userEntity.RefreshTokenExpiration = DateTime.Now.AddDays(7);
            await DbUpdateAsync(userEntity, cancellationToken);
            var expiration = DateTime.Now.AddMinutes(5);
            return _jwtAuthService.GetJwtResponse(userEntity.Id, userEntity.UserName,
                userEntity.UserRoles.Select(ur => ur.Role.Name), expiration,
                request.SecurityKey, request.Issuer, request.Audience, userEntity.RefreshToken);
        }
    }
}
