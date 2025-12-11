using Evently.Shared.Application.Time;

namespace Evently.Shared.Infrastructure;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
