namespace Evently.Shared.Application.Time;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
