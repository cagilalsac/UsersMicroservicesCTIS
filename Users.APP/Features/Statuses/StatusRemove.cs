using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Statuses
{
    public class StatusRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class StatusRemoveHandler : DbService<Status>, IRequestHandler<StatusRemoveRequest, CommandResponse>
    {
        public StatusRemoveHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Status> DbQuery()
        {
            return base.DbQuery().Include(s => s.Users);
        }

        public async Task<CommandResponse> Handle(StatusRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await DbSingleAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("Status not found!");
            if (entity.Users.Any())
                return Error("Status can't be deleted because it has relational users!");
            await DbRemoveAsync(entity, cancellationToken);
            return Success(entity.Id, "Status removed successfully.");
        }
    }
}
