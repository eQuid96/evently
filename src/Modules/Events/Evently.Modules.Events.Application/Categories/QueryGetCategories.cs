using System.Data.Common;
using Dapper;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Application.Categories;

public sealed record QueryGetCategories : IQuery<IReadOnlyCollection<CategoryResponse>>;


internal sealed class QueryHandlerGetCategories(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<QueryGetCategories, IReadOnlyCollection<CategoryResponse>>
{
    public async Task<Result<IReadOnlyCollection<CategoryResponse>>> Handle(QueryGetCategories request, CancellationToken cancellationToken)
    {
        await using DbConnection db = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = $"""
                            SELECT
                                category.id AS              {nameof(CategoryResponse.Id)},
                                category.name AS            {nameof(CategoryResponse.Name)},
                                category.is_archived AS     {nameof(CategoryResponse.IsArchived)}
                            FROM events.categories AS category;
                            """;
        
       IEnumerable<CategoryResponse> categories = await db.QueryAsync<CategoryResponse>(sql);
       return categories.ToArray();
    }
}
