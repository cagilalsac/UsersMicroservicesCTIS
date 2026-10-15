using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using Users.APP.Domain;

namespace Users.APP.Features.Roles
{
    public class RoleAddRequest : Record, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(25)]
        public string Name { get; set; }
    }

    public class RoleAddHandler : DbService<Role>,
        IRequestHandler<RoleAddRequest, CommandResponse>
    {
        public RoleAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(RoleAddRequest request, 
            CancellationToken cancellationToken)
        {
            // Way 1:
            //var entity = await DbSingleAsync(r => r.Name == request.Name);
            //if (entity is not null)
            //    return Error("Role with the same name exists!");
            // Way 2:
            if (await DbQuery().AnyAsync(r => r.Name == request.Name.Trim(), 
                cancellationToken))
                return Error("Role with the same name exists!");
            var entity = new Role
            {
                Name = request.Name.Trim()
            };
            await DbAddAsync(entity);
            return Success(entity.Id, "Role created successfully.");
        }
    }
}
