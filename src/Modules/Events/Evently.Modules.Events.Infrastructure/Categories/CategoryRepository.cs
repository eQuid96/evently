using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Categories;

internal sealed class CategoryRepository(EventsDbContext dbContext) : ICategoryRepository
{
    public Task<Category?> GetAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
       return dbContext.Categories.SingleOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
    }

    public void Insert(Category category)
    {
        dbContext.Categories.Add(category);
    }
}
