namespace Evently.Modules.SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
