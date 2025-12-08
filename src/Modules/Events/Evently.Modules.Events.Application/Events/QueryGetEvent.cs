using System.Data.Common;
using Dapper;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.SharedKernel;

namespace Evently.Modules.Events.Application.Events;

public sealed record QueryGetEvent(Guid EventId) : IQuery<EventResponse>;


internal sealed class QueryHandlerGetEvent(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<QueryGetEvent, EventResponse>
{
    public async Task<Result<EventResponse>> Handle(QueryGetEvent request, CancellationToken cancellationToken)
    {
        await using DbConnection db = await dbConnectionFactory.OpenConnectionAsync();

        const string query = $"""
                             SELECT
                                    e.id AS             {nameof(EventResponse.Id)},
                                    e.title AS          {nameof(EventResponse.Title)},
                                    e.description AS    {nameof(EventResponse.Description)},
                                    e.location AS       {nameof(EventResponse.Location)},
                                    e.starts_at_utc AS  {nameof(EventResponse.StartsAt)},
                                    e.category_id AS    {nameof(EventResponse.CategoryId)},
                                    categories.name AS  {nameof(EventResponse.Category)},
                                    e.ends_at_utc AS    {nameof(EventResponse.EndsAt)}
                                FROM events.events AS e
                                INNER JOIN events.categories as categories on categories.id = e.category_id
                                WHERE e.id = @EventId
                             """;

        EventResponse? eventResponse = await db.QuerySingleOrDefaultAsync<EventResponse>(query, request);

        if (eventResponse is null)
        {
            return Result<EventResponse>.Failure(EventsErrors.NotFound(request.EventId));
        }
        
        return eventResponse;
    }
}
