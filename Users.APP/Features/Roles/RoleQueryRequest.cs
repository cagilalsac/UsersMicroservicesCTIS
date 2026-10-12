using CORE.Domain;
using MediatR;

namespace Users.APP.Features.Roles
{
    public class RoleQueryRequest : Record, IRequest<IQueryable<RoleQueryResponse>>
    {
    }
}
