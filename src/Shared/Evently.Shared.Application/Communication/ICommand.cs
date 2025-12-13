using Evently.Shared.Domain;
using MediatR;

namespace Evently.Shared.Application.Communication;

public interface ICommand : IRequest<Result>, IBaseCommand;

public interface ICommand<TResult> : IRequest<Result<TResult>>, IBaseCommand;

public interface IBaseCommand;
