using MediatR;

namespace Evently.Modules.SharedKernel;

public interface IQuery<out TResponse> : IRequest<TResponse>;


