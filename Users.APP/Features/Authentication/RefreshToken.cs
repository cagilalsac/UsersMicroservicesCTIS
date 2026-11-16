using CORE.Authentication.Models;
using CORE.Authentication.Services;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Authentication
{
    public class RefreshTokenRequest : JwtRefreshRequest
        , IRequest<JwtResponse>
    {
    }

    public class RefreshTokenHandler : DbService<User>, IRequestHandler<RefreshTokenRequest, JwtResponse>
    {
        private readonly IJwtAuthService _jwtAuthService;

        public RefreshTokenHandler(DbContext db, IJwtAuthService jwtAuthService) : base(db)
        {
            _jwtAuthService = jwtAuthService;
        }

        protected override IQueryable<User> DbQuery()
        {
            return base.DbQuery().Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role);
        }

        public async Task<JwtResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var claims = _jwtAuthService.GetClaims(request.Jwt, request.SecurityKey);
            var userId = Convert.ToInt32(claims.SingleOrDefault(c => c.Type == "Id").Value);
            var userEntity = await DbSingleAsync(u => u.Id == userId &&
                u.RefreshToken == request.RefreshToken && u.RefreshTokenExpiration >= DateTime.Now, cancellationToken);
            if (userEntity is null)
                return null;
            userEntity.RefreshToken = _jwtAuthService.GetRefreshToken();
            // sliding expiration
            //userEntity.RefreshTokenExpiration = DateTime.Now.AddDays(7);
            await DbUpdateAsync(userEntity);
            var expiration = DateTime.Now.AddMinutes(5);
            return _jwtAuthService.GetJwtResponse(userEntity.Id,
                userEntity.UserName, userEntity.UserRoles.Select(ur => ur.Role.Name),
                expiration, request.SecurityKey, request.Issuer, request.Audience,
                userEntity.RefreshToken);
        }
    }
}
