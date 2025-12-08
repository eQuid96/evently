using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.SharedKernel;

namespace Evently.Modules.Events.Application.Categories;

public sealed record CommandArchiveCategory(Guid CategoryId) : ICommand<bool>;


internal sealed class CommandHandlerArchiveCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) 
    : ICommandHandler<CommandArchiveCategory, bool>
{
    public async Task<bool> Handle(CommandArchiveCategory request, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return false;
        }
        
        category.Archive();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
