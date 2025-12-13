using System.Data.Common;
using Dapper;
using Evently.Modules.Events.Application.TicketTypes;
using Evently.Modules.Events.Domain.Events;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Application.Events;

public sealed record QueryGetEvent(Guid EventId) : IQuery<EventResponse>;


internal sealed class QueryHandlerGetEvent(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<QueryGetEvent, EventResponse>
{
    public async Task<Result<EventResponse>> Handle(QueryGetEvent request, CancellationToken cancellationToken)
    {
        await using DbConnection db = await dbConnectionFactory.OpenConnectionAsync();

        const string query = $"""
                             SELECT
                                    e.id AS                     {nameof(EventResponse.Id)},
                                    e.title AS                  {nameof(EventResponse.Title)},
                                    e.description AS            {nameof(EventResponse.Description)},
                                    e.location AS               {nameof(EventResponse.Location)},
                                    e.starts_at_utc AS          {nameof(EventResponse.StartsAt)},
                                    e.category_id AS            {nameof(EventResponse.CategoryId)},
                                    categories.name AS          {nameof(EventResponse.Category)},
                                    e.ends_at_utc AS            {nameof(EventResponse.EndsAt)},
                                    ticket_type.id AS           {nameof(TicketTypeResponse.TicketTypeId)},
                                    ticket_type.event_id AS     {nameof(TicketTypeResponse.EventId)},
                                    ticket_type.name AS         {nameof(TicketTypeResponse.Name)},
                                    ticket_type.price AS        {nameof(TicketTypeResponse.Price)},
                                    ticket_type.quantity AS     {nameof(TicketTypeResponse.Quantity)},
                                    ticket_type.currency AS     {nameof(TicketTypeResponse.Currency)}
                                FROM events.events AS e
                                INNER JOIN events.categories as categories on categories.id = e.category_id
                                LEFT JOIN events.ticket_types ticket_type on ticket_type.event_id = e.id
                                WHERE e.id = @EventId
                             """;


        Dictionary<Guid, EventResponse> events = [];
        await db.QueryAsync<EventResponse, TicketTypeResponse?, EventResponse>(query, (@event, ticketType) =>
        {
            if (events.TryGetValue(@event.Id, out EventResponse existingEvent))
            {
                @event = existingEvent;
            }
            else
            {
                events.Add(@event.Id, @event);
            }
            
            if (ticketType is not null)
            {
                @event.TicketTypes.Add(ticketType);
            }
            
            return @event;
        }, request, splitOn: nameof(TicketTypeResponse.TicketTypeId));

        if (events.TryGetValue(request.EventId, out EventResponse eventResponse))
        {
            return eventResponse;
        }
        
        return Result.Failure<EventResponse>(EventErrors.NotFound(request.EventId));
    }
}
