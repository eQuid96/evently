using MediatR;
namespace Evently.Modules.SharedKernel;

public interface ICommand : IRequest;

public interface ICommand<out TResult> : IRequest<TResult>;
