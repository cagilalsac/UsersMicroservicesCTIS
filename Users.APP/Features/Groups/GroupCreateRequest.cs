using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Groups
{
    public class GroupCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
    }

    public class GroupCreateHandler : Service<Group>, IRequestHandler<GroupCreateRequest, CommandResponse>
    {
        public GroupCreateHandler(DbContext db) : base(db)
        {
        }

        public Task<CommandResponse> Handle(GroupCreateRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
