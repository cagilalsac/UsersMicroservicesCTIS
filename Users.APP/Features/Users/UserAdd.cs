using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Users
{
    public class UserAddRequest : Record, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")] // "UserName is required!
        [StringLength(30, MinimumLength = 3, 
            ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        // "UserName must be minimum 3 maximum 30 characters!
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(15, MinimumLength = 3,
            ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string Password { get; set; }

        [StringLength(50,
            ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string FirstName { get; set; }

        [StringLength(50,
            ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public double? Score { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public int? StatusId { get; set; }

        public List<int> RoleIds { get; set; } = new();
    }

    public class UserAddHandler : DbService<User>, IRequestHandler<UserAddRequest, CommandResponse>
    {
        public UserAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(UserAddRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(u => u.UserName == request.UserName.Trim(), cancellationToken))
                return Error("User with the same user name exists!");
            var entity = new User
            {
                BirthDate = request.BirthDate,
                FirstName = request.FirstName?.Trim(),
                LastName = request.LastName?.Trim(),
                Gender = request.Gender,
                Password = request.Password.Trim(),
                RegistrationDate = DateTime.Now,
                RoleIds = request.RoleIds,
                Score = request.Score ?? 0,
                StatusId = request.StatusId.Value,
                UserName = request.UserName.Trim()
            };
            await DbAddAsync(entity, cancellationToken);
            return Success(entity.Id, "User added successfully.");
        }
    }
}
