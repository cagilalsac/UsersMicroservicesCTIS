using CORE.Domain;
using System.ComponentModel.DataAnnotations;

namespace Users.APP.Domain
{
    public class Status : Record
    {
        [Required, StringLength(5)]
        public string Title { get; set; }

        public List<User> Users { get; set; } = new(); // navigation property
    }
}
