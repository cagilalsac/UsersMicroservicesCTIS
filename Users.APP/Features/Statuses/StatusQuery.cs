using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;
using Users.APP.Features.Users;

namespace Users.APP.Features.Statuses
{
    public class StatusQueryRequest : Record, IRequest<IQueryable<StatusQueryResponse>>
    {
    }

    public class StatusQueryResponse : Record
    {
        public string Title { get; set; }

        public int UserCountR { get; set; }

        public string UserNamesR { get; set; }

        public List<UserQueryResponse> UsersR { get; set; }
    }

    public class StatusQueryHandler : DbService<Status>, IRequestHandler<StatusQueryRequest, IQueryable<StatusQueryResponse>>
    {
        public StatusQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Status> DbQuery()
        {
            return base.DbQuery().Include(s => s.Users);
        }

        public Task<IQueryable<StatusQueryResponse>> Handle(StatusQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(DbQuery().Select(s => new StatusQueryResponse
            {
                Id = s.Id,
                Title = s.Title,

                UserCountR = s.Users.Count,
                UserNamesR = string.Join(", ", s.Users.Select(u => u.UserName)),
                UsersR = s.Users.Select(u => new UserQueryResponse
                {
                    BirthDate = u.BirthDate,
                    FirstName = u.FirstName,
                    Gender = u.Gender,
                    Id = u.Id,
                    IsOnline = u.IsOnline,
                    LastName = u.LastName,
                    Password = u.Password,
                    RegistrationDate = u.RegistrationDate,
                    RoleIds = u.RoleIds,
                    Score = u.Score,
                    StatusId = u.StatusId,
                    UserName = u.UserName,
                }).ToList()
            }));
        }
    }
}
