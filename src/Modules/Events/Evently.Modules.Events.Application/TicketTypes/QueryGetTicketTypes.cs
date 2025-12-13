using System.Data.Common;
using Dapper;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Application.TicketTypes;

public sealed record QueryGetTicketTypes(Guid EventId) : IQuery<IReadOnlyCollection<TicketTypeResponse>>;

internal sealed class QueryHandlerGetTicketTypes(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<QueryGetTicketTypes, IReadOnlyCollection<TicketTypeResponse>>
{
    public async Task<Result<IReadOnlyCollection<TicketTypeResponse>>> Handle(QueryGetTicketTypes request, CancellationToken cancellationToken)
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
                            WHERE event_id = @EventId
                            """;

        List<TicketTypeResponse>? response = (await db.QueryAsync<TicketTypeResponse>(sql, request)).AsList();
        return response;
    }
}

