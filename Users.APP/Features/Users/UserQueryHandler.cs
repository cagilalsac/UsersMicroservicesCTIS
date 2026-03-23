using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;
using Users.APP.Features.Groups;

namespace Users.APP.Features.Users
{
    public class UserQueryRequest : Request, IRequest<IQueryable<UserQueryResponse>>
    {
    }

    public class UserQueryResponse : Response
    {
        // entity properties
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Gender { get; set; } // 1, 2

        public DateTime? BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public decimal Score { get; set; }

        public bool IsActive { get; set; }

        public string Address { get; set; }

        public int? CountryId { get; set; } 

        public int? CityId { get; set; } 

        public int? GroupId { get; set; }

        public List<int> RoleIds { get; set; }

        // custom properties
        public string FullName { get; set; }

        public string GenderF { get; set; } // "Woman", "Man"

        public string BirthDateF { get; set; } // "03/12/2026"

        public string RegistrationDateF { get; set; }

        public string ScoreF { get; set; }

        public string IsActiveF { get; set; } // "Active", "Inactive"

        public string Group { get; set; } // "CTIS", "Bilkent"

        public string Roles { get; set; } // "Admin", "User"

        public GroupQueryResponse GroupResponse { get; set; }
    }

    public class UserQueryHandler : Service<User>, IRequestHandler<UserQueryRequest, IQueryable<UserQueryResponse>>
    {
        public UserQueryHandler(DbContext db) : base(db)
        {
            // if the culture of the application is needed to be changed
            //CultureInfo = new CultureInfo("tr-TR");
        }

        // base query
        // select * from Users
        // overridden query
        // select * from Users inner join Groups on Users.GroupId = Groups.Id order by IsActive desc, RegistrationDate desc, UserName 
        protected override IQueryable<User> DbSet()
        {
            return base.DbSet().Include(userEntity => userEntity.Group).OrderByDescending(userEntity => userEntity.IsActive).ThenByDescending(userEntity => userEntity.RegistrationDate).ThenBy(userEntity => userEntity.UserName);
        }

        public Task<IQueryable<UserQueryResponse>> Handle(UserQueryRequest request, CancellationToken cancellationToken)
        {
            var query = DbSet().Select(userEntity => new UserQueryResponse
            {
                // entity data
                Address = userEntity.Address,
                BirthDate = userEntity.BirthDate,
                CityId = userEntity.CityId,
                CountryId = userEntity.CountryId,
                FirstName = userEntity.FirstName,
                Gender = (int)userEntity.Gender,
                GroupId = userEntity.GroupId,
                Id = userEntity.Id,
                IsActive = userEntity.IsActive,
                LastName = userEntity.LastName,
                Password = userEntity.Password,
                RegistrationDate = userEntity.RegistrationDate,
                RoleIds = userEntity.RoleIds,
                Score = userEntity.Score,
                UserName = userEntity.UserName,
                
                // custom data
                FullName = userEntity.FirstName + " " + userEntity.LastName,
                IsActiveF = userEntity.IsActive ? "Active" : "Inactive",
                ScoreF = userEntity.Score.ToString("N1"), // 3.33333333 -> 3.3
                GenderF = userEntity.Gender.ToString(), // "Man", "Woman"
                RegistrationDateF = userEntity.RegistrationDate.ToString("MM/dd/yyyy HH:mm:ss"),
                BirthDateF = userEntity.BirthDate.HasValue ? userEntity.BirthDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                
                Roles = string.Join(", ", userEntity.UserRoles.Select(userRoleEntity => userRoleEntity.Role.Name)),

                // Way 1:
                Group = userEntity.Group.Title,

                // Way 2:
                GroupResponse = new GroupQueryResponse
                {
                    Id = userEntity.Group.Id,
                    Title = userEntity.Group.Title
                }
            });

            return Task.FromResult(query);
        }
    }
}
