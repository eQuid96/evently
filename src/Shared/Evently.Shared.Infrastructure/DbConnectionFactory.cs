using System.Data.Common;
using Evently.Shared.Application.Data;
using Npgsql;

namespace Evently.Shared.Infrastructure;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
