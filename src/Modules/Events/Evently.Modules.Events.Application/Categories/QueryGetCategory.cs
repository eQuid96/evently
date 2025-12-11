using System.Data.Common;
using Dapper;
using Evently.Modules.Events.Domain.Categories;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Application.Categories;

public sealed record QueryGetCategory(Guid CategoryId) : IQuery<CategoryResponse>;


internal sealed class QueryHandlerGetCategory(IDbConnectionFactory connectionFactory)
    : IQueryHandler<QueryGetCategory, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(QueryGetCategory request, CancellationToken cancellationToken)
    {
        await using DbConnection db = await connectionFactory.OpenConnectionAsync();

        const string sql = $"""
                            SELECT
                                category.id AS          {nameof(CategoryResponse.Id)},
                                category.name AS        {nameof(CategoryResponse.Name)},
                                category.is_archived    {nameof(CategoryResponse.IsArchived)}
                            FROM events.categories as category
                            WHERE category.id = @CategoryId
                            """;

        CategoryResponse? category = await db.QuerySingleOrDefaultAsync<CategoryResponse>(sql, request);

        if (category is null)
        {
            return Result<CategoryResponse>.Failure(CategoryErrors.NotFound(request.CategoryId));
        }
        return category;
    }
}
