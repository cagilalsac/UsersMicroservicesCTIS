using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Roles
{
    public class RoleQueryHandler : DbService<Role>,
        IRequestHandler<RoleQueryRequest, IQueryable<RoleQueryResponse>>
    {
        public RoleQueryHandler(DbContext db) : base(db)
        {
        }

        public Task<IQueryable<RoleQueryResponse>> Handle(RoleQueryRequest request, CancellationToken cancellationToken)
        {
            var entityQuery = DbQuery(); // select * from Roles // Role entity query
            var responseQuery = entityQuery.Select(roleEntity => new RoleQueryResponse
            {
                Id = roleEntity.Id,
                Name = roleEntity.Name
            }); // select Id, Name from Roles // RoleQueryResponse query
            return Task.FromResult(responseQuery);
        }
    }
}
