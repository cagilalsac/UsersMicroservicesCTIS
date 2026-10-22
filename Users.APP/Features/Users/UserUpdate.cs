using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Users
{
    public class UserUpdateRequest : Record, IRequest<CommandResponse>
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

    public class UserUpdateHandler : DbService<User>, IRequestHandler<UserUpdateRequest, CommandResponse>
    {
        public UserUpdateHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<User> DbQuery()
        {
            return base.DbQuery().Include(u => u.UserRoles);
        }

        public async Task<CommandResponse> Handle(UserUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(u => u.Id != request.Id && 
                u.UserName == request.UserName.Trim(), cancellationToken))
                return Error("User with the same user name exists!");
            var entity = await DbSingleAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("User not found!");
            DbRemove(entity.UserRoles);
            entity.BirthDate = request.BirthDate;
            entity.FirstName = request.FirstName?.Trim();
            entity.LastName = request.LastName?.Trim();
            entity.Gender = request.Gender;
            entity.Password = request.Password.Trim();
            entity.RegistrationDate = DateTime.Now;
            entity.RoleIds = request.RoleIds;
            entity.Score = request.Score ?? 0;
            entity.StatusId = request.StatusId.Value;
            entity.UserName = request.UserName.Trim();
            await DbUpdateAsync(entity, cancellationToken);
            return Success(entity.Id, "User updated successfully.");
        }
    }
}
