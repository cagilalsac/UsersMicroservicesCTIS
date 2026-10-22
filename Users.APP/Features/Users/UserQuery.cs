using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;
using Users.APP.Features.Roles;
using Users.APP.Features.Statuses;

namespace Users.APP.Features.Users
{
    public class UserQueryRequest : Record, IRequest<IQueryable<UserQueryResponse>>
    {
    }

    public class UserQueryResponse : Record
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Genders Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public double Score { get; set; }

        public bool IsOnline { get; set; }

        public int StatusId { get; set; }

        public List<int> RoleIds { get; set; }

        public string PasswordR { get; set; }

        public string FullNameR { get; set; }

        public string GenderR { get; set; }

        public string BirthDateR { get; set; }

        public string RegistrationDateR { get; set; }

        public string ScoreR { get; set; }

        public string IsOnlineR { get; set; }

        // Way 1:
        public string StatusTitleR { get; set; }

        // Way 2:
        public StatusQueryResponse StatusR { get; set; }

        // Way 1:
        public string RoleNamesR { get; set; }

        // Way 2:
        public List<RoleQueryResponse> RolesR { get; set; }
    }

    public class UserQueryHandler : DbService<User>,
        IRequestHandler<UserQueryRequest, IQueryable<UserQueryResponse>>
    {
        public UserQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<User> DbQuery()
        {
            return base.DbQuery().Include(u => u.Status)
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .OrderByDescending(u => u.Score)
                .ThenByDescending(u => u.RegistrationDate).ThenBy(u => u.UserName);
            ;
        }

        public Task<IQueryable<UserQueryResponse>> Handle(UserQueryRequest request, CancellationToken cancellationToken)
        {
            var query = DbQuery().Select(u => new UserQueryResponse
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

                PasswordR = new string('*', u.Password.Length),
                FullNameR = u.FirstName + " " + u.LastName,
                GenderR = u.Gender.ToString(),
                IsOnlineR = u.IsOnline ? "Online" : "Offline",
                ScoreR = u.Score.ToString("N1"),
                RegistrationDateR = u.RegistrationDate.ToString("MM/dd/yyyy HH:mm:ss"),
                BirthDateR = u.BirthDate.HasValue ? u.BirthDate.Value.ToShortDateString()
                    : string.Empty,
                // Way 1:
                StatusTitleR = u.Status.Title,
                // Way 2:
                StatusR = new StatusQueryResponse
                {
                    Id = u.Status.Id,
                    Title = u.Status.Title
                },
                // Way 1:
                RoleNamesR = string.Join(", ", u.UserRoles.Select(ur => ur.Role.Name)),
                // Way 2:
                RolesR = u.UserRoles.OrderBy(ur => ur.Role.Name)
                    .Select(ur => new RoleQueryResponse
                    {
                        Id = ur.Role.Id,
                        Name = ur.Role.Name
                    }).ToList()
            });
            return Task.FromResult(query);
        }
    }
}
