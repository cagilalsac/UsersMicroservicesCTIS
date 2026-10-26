using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Users
{
    public class UserRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class UserRemoveHandler : DbService<User>, IRequestHandler<UserRemoveRequest, CommandResponse>
    {
        public UserRemoveHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<User> DbQuery() => base.DbQuery().Include(u => u.UserRoles);

        public async Task<CommandResponse> Handle(UserRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await DbSingleAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("User not found!");
            DbRemove(entity.UserRoles);
            await DbRemoveAsync(entity, cancellationToken);
            return Success(entity.Id, "User deleted successfully.");
        }
    }
}
