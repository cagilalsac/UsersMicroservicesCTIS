using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.APP.Features.Authentication;

namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public TokensController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost, Route("~/api/[action]")] // POST: api/Token
        public async Task<IActionResult> Token(TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                tokenRequest.SecurityKey = _configuration["SecurityKey"];
                tokenRequest.Issuer = _configuration["Issuer"];
                tokenRequest.Audience = _configuration["Audience"];
                var response = await _mediator.Send(tokenRequest);
                if (response is not null)
                    return Ok(response);
                return NotFound("User not found!");
            }
            return BadRequest(ModelState);
        }
    }
}
