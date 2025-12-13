using Evently.Modules.Events.Domain.Categories;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Domain;
using FluentValidation;

namespace Evently.Modules.Events.Application.Categories;

public sealed record CommandCreateCategory(string Name) : ICommand<Guid>;

internal sealed class ValidatorCommandCreateCategory : AbstractValidator<CommandCreateCategory>
{
    public ValidatorCommandCreateCategory()
    {
        RuleFor(c => c.Name).NotEmpty();
    }
}


internal sealed class CommandHandlerCreateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) 
    : ICommandHandler<CommandCreateCategory, Guid>
{
    public async Task<Result<Guid>> Handle(CommandCreateCategory request, CancellationToken cancellationToken)
    {
        var newCategory = Category.Create(request.Name);
        categoryRepository.Insert(newCategory);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return newCategory.Id;
    }
}
