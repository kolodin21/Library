﻿using System.Data;
using Library.DAL.Configuration;
using NLog;
using Npgsql;

namespace Library.DAL.Repositories
{
    public abstract class BaseRepository
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected BaseRepository()
        {
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

                Logger.Info("Подключение успешно!");
                return queryAction(connection);
            }
            catch (NpgsqlException e)
            {
                Logger.Info(e.Message);
                return default; 
            }
        }

        protected bool IsNullFields<T>(T entity, string methodName)
        {
            if (entity is null)
            {
                Logger.Info($"Method {methodName}: entity is null.");
                return true;
            }

            switch (entity)
            {
                case string str when string.IsNullOrWhiteSpace(str):
                    Logger.Info($"Method {methodName}: string entity is empty or whitespace.");
                    return true;

                case Dictionary<string, object> dictionary when dictionary.Count == 0:
                    Logger.Info($"Method {methodName}: dictionary entity is empty.");
                    return true;

                default:
                    return false;
            }
        }
    }
}
