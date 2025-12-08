using MediatR;
namespace Evently.Modules.SharedKernel;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResult> : IRequest<Result<TResult>>;
