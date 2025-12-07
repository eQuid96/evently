using System.Data.Common;

namespace Evently.Modules.SharedKernel;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}
