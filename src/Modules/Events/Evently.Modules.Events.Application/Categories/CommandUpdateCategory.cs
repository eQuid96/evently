using Evently.Modules.Events.Domain.Categories;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Application.Categories;

public sealed record CommandUpdateCategory(Guid CategoryId, string Name) : ICommand<CategoryResponse>;


internal sealed class CommandHandlerUpdateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CommandUpdateCategory, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(CommandUpdateCategory request, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetAsync(request.CategoryId, cancellationToken);
        if (category is null)
        {
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound(request.CategoryId));
        }
        
        category.ChangeName(request.Name);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new CategoryResponse(category.Id, category.Name, category.IsArchived);
    }
}
