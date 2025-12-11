using Evently.Shared.Domain;
using MediatR;

namespace Evently.Shared.Application.Communication;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResult> : IRequest<Result<TResult>>;
