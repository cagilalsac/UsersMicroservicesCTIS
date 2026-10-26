using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Statuses
{
    public class StatusAddRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(5)]
        public string Title { get; set; }
    }

    public class StatusAddHandler : DbService<Status>, IRequestHandler<StatusAddRequest, CommandResponse>
    {
        public StatusAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(StatusAddRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(s => s.Title == request.Title.Trim(), cancellationToken))
                return Error("Status already exists!");
            var entity = new Status
            {
                Title = request.Title.Trim()
            };
            await DbAddAsync(entity, cancellationToken);
            return Success(entity.Id, "Status created successfully.");
        }
    }
}
