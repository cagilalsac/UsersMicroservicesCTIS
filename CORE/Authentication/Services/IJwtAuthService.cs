using CORE.Authentication.Models;
using System.Security.Claims;

namespace CORE.Authentication.Services
{
    
    public interface IJwtAuthService
    {
        
        public JwtResponse GetJwtResponse(int userId, string userName,
            IEnumerable<string> userRoleNames, DateTime expiration,
            string securityKey, string issuer,
            string audience, string refreshToken);
        

       
        public string GetRefreshToken();

        
        public IEnumerable<Claim> GetClaims(string jwt, string securityKey);
    }
}
