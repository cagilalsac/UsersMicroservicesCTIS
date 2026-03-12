using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Groups
{
    public class GroupDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class GroupDeleteHandler : Service<Group>, IRequestHandler<GroupDeleteRequest, CommandResponse>
    {
        public GroupDeleteHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(GroupDeleteRequest request, CancellationToken cancellationToken)
        {
            var existingEntity = await DbSet().SingleOrDefaultAsync(groupEntity => groupEntity.Id == request.Id, cancellationToken);
            if (existingEntity is null)
                return Error("Group not found!");
            await DeleteAsync(existingEntity, cancellationToken);
            return Success("Group deleted successfully.", existingEntity.Id);
        }
    }
}
