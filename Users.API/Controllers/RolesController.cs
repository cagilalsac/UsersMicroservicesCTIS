using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.APP.Features.Roles;

namespace Users.API.Controllers
{
    [Route("api/[controller]")] // https://localhost:1780/api/Roles
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var roleResponseQuery = await _mediator.Send(new RoleQueryRequest());
            var roleResponseList = await roleResponseQuery.ToListAsync();
            return Ok(roleResponseList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var roleResponseQuery = await _mediator.Send(new RoleQueryRequest());
            var roleResponseItem = await roleResponseQuery
                .SingleOrDefaultAsync(roleRepsone => roleRepsone.Id == id);
            return Ok(roleResponseItem);
        }
    }
}
