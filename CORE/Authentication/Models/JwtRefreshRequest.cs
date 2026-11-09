using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CORE.Authentication.Models
{
    
    public class JwtRefreshRequest
    {
    
        [Required]
        public string Jwt { get; set; }

        
        [Required]
        public string RefreshToken { get; set; }

        
        [JsonIgnore]
        public string SecurityKey { get; set; }

        
        [JsonIgnore]
        public string Issuer { get; set; }

        
        [JsonIgnore]
        public string Audience { get; set; }
    }
}
