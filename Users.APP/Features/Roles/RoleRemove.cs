using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Roles
{
    public class RoleRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class RoleRemoveHandler : DbService<Role>,
        IRequestHandler<RoleRemoveRequest, CommandResponse>
    {
        public RoleRemoveHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Role> DbQuery()
        {
            return base.DbQuery().Include(r => r.UserRoles);
        }

        public async Task<CommandResponse> Handle(RoleRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await DbSingleAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("Role not found!");
            DbRemove(entity.UserRoles);
            await DbRemoveAsync(entity, cancellationToken);
            return Success(entity.Id, "Role deleted successfully.");
        }
    }
}
