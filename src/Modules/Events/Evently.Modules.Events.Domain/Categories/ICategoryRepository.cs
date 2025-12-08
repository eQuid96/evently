namespace Evently.Modules.Events.Domain.Categories;

public interface ICategoryRepository
{
    Task<Category?> GetAsync(Guid categoryId, CancellationToken cancellationToken = default);
    
    void Insert(Category category);
}
