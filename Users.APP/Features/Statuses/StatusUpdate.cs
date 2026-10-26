using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Statuses
{
    public class StatusUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(5)]
        public string Title { get; set; }
    }

    public class StatusUpdateHandler : DbService<Status>, IRequestHandler<StatusUpdateRequest, CommandResponse>
    {
        public StatusUpdateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(StatusUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(s => s.Id != request.Id && 
                s.Title == request.Title.Trim(), cancellationToken))
                return Error("Status already exists!");
            var entity = await DbSingleAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("Status not found!");
            entity.Title = request.Title.Trim();
            await DbUpdateAsync(entity, cancellationToken);
            return Success(entity.Id, "Status updated successfully.");
        }
    }
}
