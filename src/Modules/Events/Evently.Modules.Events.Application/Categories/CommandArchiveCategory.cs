using Evently.Modules.Events.Domain.Categories;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Application.Categories;

public sealed record CommandArchiveCategory(Guid CategoryId) : ICommand;


internal sealed class CommandHandlerArchiveCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) 
    : ICommandHandler<CommandArchiveCategory>
{
    public async Task<Result> Handle(CommandArchiveCategory request, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure(CategoryErrors.NotFound(request.CategoryId));
        }
        
        category.Archive();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
}
