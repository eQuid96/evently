using MediatR;

namespace Evently.Modules.SharedKernel;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;


