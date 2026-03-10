using System.Data.Common;
using Evently.Shared.Application.Data;
using Evently.Shared.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Evently.Shared.Infrastructure;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}

public class InterceptorPublishDomainEvents(IServiceScopeFactory scopeFactory) : SaveChangesInterceptor
{
    public async override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, 
        int result,
        CancellationToken cancellationToken = default)
    {

        if (eventData.Context is not null)
        {
            IDomainEvent[] domainEvents = eventData.Context.ChangeTracker
                .Entries<Entity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    IReadOnlyCollection<IDomainEvent> events = entity.DomainEvents;
                    entity.ClearDomainEvents();
                    return events;
                })
                .ToArray();

            using IServiceScope scope = scopeFactory.CreateScope();
            IPublisher publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            foreach (IDomainEvent domainEvent in domainEvents)
            {
                await publisher.Publish(domainEvent, cancellationToken);
            }
        }
        
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
} 
