using System.Data;
using Library.Common;
using Library.DAL.Configuration;
using Npgsql;

namespace Library.DAL.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly IMessageLogger Logger;

        protected BaseRepository(IMessageLogger logger)
        {
            Logger = logger;
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
                return default; 
            }
        }

        protected bool IsNullFields<T>(T entity, string methodName)
        {
            if (entity is null)
            {
                Logger.Log($"Method {methodName}: entity is null.");
                return true;
            }

            switch (entity)
            {
                case string str when string.IsNullOrWhiteSpace(str):
                    Logger.Log($"Method {methodName}: string entity is empty or whitespace.");
                    return true;

                case Dictionary<string, object> dictionary when dictionary.Count == 0:
                    Logger.Log($"Method {methodName}: dictionary entity is empty.");
                    return true;

                default:
                    return false;
            }
        }
    }
}
