using System.Data;

namespace Blog.Domain.Interfaces;

public interface IDatabaseConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
    Task CloseConnectionAsync(IDbConnection dbConnection);
}