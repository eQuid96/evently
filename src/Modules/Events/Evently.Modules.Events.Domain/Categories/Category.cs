

using Evently.Shared.Domain;

namespace Evently.Modules.Events.Domain.Categories;

public sealed class Category : Entity
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public bool IsArchived { get; private set; }

    private Category()
    {
        
    }
    
    public static Category Create(string name)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            IsArchived = false
        };
        
        category.RaiseEvent(new CategoryCreatedDomainEvent(category.Id));
        
        return category;
    }

    public void Archive()
    {
        IsArchived = true;
        RaiseEvent(new CategoryArchivedDomainEvent(Id));
    }


    public void ChangeName(string name)
    {
        if (Name == name)
        {
            return;
        }
        
        Name = name;
        RaiseEvent(new CategoryChangedNameDomainEvent(Id, name));
    }
}
