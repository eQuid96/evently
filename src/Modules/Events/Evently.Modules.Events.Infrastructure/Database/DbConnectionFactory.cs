using System.Data.Common;
using Evently.Modules.SharedKernel;
using Npgsql;

namespace Evently.Modules.Events.Infrastructure.Database;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
