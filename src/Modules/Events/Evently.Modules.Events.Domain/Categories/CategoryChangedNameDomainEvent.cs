using Evently.Modules.SharedKernel;

namespace Evently.Modules.Events.Domain.Categories;

public sealed class CategoryChangedNameDomainEvent(Guid categoryId, string newName) : DomainEvent
{
    public Guid CategoryId { get; init; } = categoryId;
    public string Name { get; init; } = newName;
}
