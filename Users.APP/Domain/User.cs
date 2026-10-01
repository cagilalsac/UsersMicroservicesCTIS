using CORE.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Users.APP.Domain
{
    public class User : Record
    {
        [Required, StringLength(30)]
        public string UserName { get; set; }

        [Required, StringLength(15)]
        public string Password { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public double Score { get; set; }

        public bool IsOnline { get; set; }

        public int StatusId { get; set; } // foreign key

        public Status Status { get; set; } // navigation property

        public List<UserRole> UserRoles { get; set; } = new(); // navigation property

        [NotMapped]
        public List<int> RoleIds
        {
            get => UserRoles.Select(ur => ur.RoleId).ToList();
            set => UserRoles = value.Select(rId => new UserRole
            {
                RoleId = rId
            }).ToList();
        }
    }
}
