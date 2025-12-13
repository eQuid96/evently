using System.Data.Common;
using Dapper;
using Evently.Modules.Events.Domain.TicketType;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Application.TicketTypes;

public sealed record QueryGetTicketType(Guid TicketTypeId) : IQuery<TicketTypeResponse>;


internal sealed class QueryHandlerGetTicketType(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<QueryGetTicketType, TicketTypeResponse>
{
    public async Task<Result<TicketTypeResponse>> Handle(QueryGetTicketType request, CancellationToken cancellationToken)
    {
        await using DbConnection db = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = $"""
                   SELECT
                       id AS         {nameof(TicketTypeResponse.TicketTypeId)},
                       event_id AS   {nameof(TicketTypeResponse.EventId)},
                       name AS       {nameof(TicketTypeResponse.Name)},
                       price AS      {nameof(TicketTypeResponse.Price)},
                       quantity AS   {nameof(TicketTypeResponse.Quantity)},
                       currency AS   {nameof(TicketTypeResponse.Currency)}
                   FROM events.ticket_types
                   WHERE id = @TicketTypeId
                   """;

        TicketTypeResponse? response = await db.QuerySingleOrDefaultAsync<TicketTypeResponse>(sql, request);
        if (response is null)
        {
            return Result.Failure<TicketTypeResponse>(TicketTypeErrors.NotFound(request.TicketTypeId));
        }
        return response;
    }
}
