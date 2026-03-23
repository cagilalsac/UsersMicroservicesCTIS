using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Users
{
    public class UserDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class UserDeleteHandler : Service<User>, IRequestHandler<UserDeleteRequest, CommandResponse>
    {
        public UserDeleteHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(UserDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await DbSet().SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("User not found!");
            await DeleteAsync(entity, cancellationToken);
            return Success("User deleted successfully.", entity.Id);
        }
    }
}
