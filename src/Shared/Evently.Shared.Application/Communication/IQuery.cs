using Evently.Shared.Domain;
using MediatR;

namespace Evently.Shared.Application.Communication;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;


