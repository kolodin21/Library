using System.Data;
using Library.Common;
using Library.DAL.Configuration;
using Npgsql;

namespace Library.DAL.Repositories
{
    public class BaseRepository
    {
        protected readonly IMessageLogger Logger;

        protected BaseRepository(IMessageLogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        // Метод для работы с запросами с соединением в using
        protected T? ExecuteQuery<T>(Func<NpgsqlConnection, T> queryAction)
        {
            using var connection = new NpgsqlConnection(DatabaseConfig.ConnectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                return queryAction(connection);
            }
            catch (NpgsqlException e)
            {
                Logger.Log(e.Message);
                return default; // Например, для bool это будет false, для объектов — null
            }
        }///////////////////
    }
}
