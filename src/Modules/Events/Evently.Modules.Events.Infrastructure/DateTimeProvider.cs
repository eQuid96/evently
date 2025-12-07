using Evently.Modules.SharedKernel;

namespace Evently.Modules.Events.Infrastructure;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
