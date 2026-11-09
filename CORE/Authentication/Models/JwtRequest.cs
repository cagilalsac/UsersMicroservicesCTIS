using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CORE.Authentication.Models
{
    
    public class JwtRequest
    {
        
        [Required]
        public string UserName { get; set; }

       
        [Required]
        public string Password { get; set; }

        
        [JsonIgnore]
        public string SecurityKey { get; set; }

    
        [JsonIgnore]
        public string Issuer { get; set; }

        
        [JsonIgnore]
        public string Audience { get; set; }
    }
}
