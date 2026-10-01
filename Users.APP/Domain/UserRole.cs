using CORE.Domain;

namespace Users.APP.Domain
{
    public class UserRole : Record
    {
        public int UserId { get; set; } // foreign key

        public User User { get; set; } // navigation property

        public int RoleId { get; set; } // foreign key

        public Role Role { get; set; } // navigation property
    }
}
