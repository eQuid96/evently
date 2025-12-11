using System.Data.Common;
using Dapper;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Application.Events;

public sealed record QueryGetEvents : IQuery<IReadOnlyCollection<EventResponse>>;


internal sealed class QueryHandlerGetEvents(IDbConnectionFactory connectionFactory)
    : IQueryHandler<QueryGetEvents, IReadOnlyCollection<EventResponse>>
{
    public async Task<Result<IReadOnlyCollection<EventResponse>>> Handle(QueryGetEvents request, CancellationToken cancellationToken)
    {
        await using DbConnection db = await connectionFactory.OpenConnectionAsync();

        const string sql = $"""
                           SELECT
                               e.id AS              {nameof(EventResponse.Id)},
                               e.title AS           {nameof(EventResponse.Title)},
                               e.description AS     {nameof(EventResponse.Description)},
                               e.location AS        {nameof(EventResponse.Location)},
                               e.starts_at_utc AS   {nameof(EventResponse.StartsAt)},
                               e.category_id AS     {nameof(EventResponse.CategoryId)},
                               category.name AS     {nameof(EventResponse.Category)},
                               e.ends_at_utc AS     {nameof(EventResponse.EndsAt)}
                           FROM events.events AS e
                           INNER JOIN events.categories category ON category.id = e.category_id;
                           """;

        IEnumerable<EventResponse> events = await db.QueryAsync<EventResponse>(sql);
        return events.ToArray();
    }
}
