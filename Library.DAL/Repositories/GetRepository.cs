using Dapper;
using Npgsql;
using System.Text.RegularExpressions;
using Library.Common;
using Library.DAL.Configuration;
using Library.DAL.Interface;

namespace Library.DAL.Repositories
{
    public class GetRepository(IMessageLogger logger) : BaseRepository(logger),IGetRepository
    {
        #region GetAll
        public IEnumerable<T>? GetAllEntity<T>(string sqlQuery) where T : class
        {
            return ExecuteQuery(connection =>
            {
                try
                {
                    var result = connection.Query<T>(sqlQuery);
                    return result;
                }
                catch (NpgsqlException e)
                {
                    Logger.Log(e.Message);
                    return null;
                }
            });
        }
        
        public IEnumerable<T>? GetAllEntity<T>(string sqlQuery, Func<SqlMapper.GridReader, IEnumerable<T>> mapFunction) where T : class
        {
            return ExecuteQuery(connection =>
            {
                try
                {
                    using var multi = connection.QueryMultiple(sqlQuery);
                    return mapFunction(multi);
                }
                catch (NpgsqlException e)
                {
                    Logger.Log(e.Message);
                    return null;
                }
            });
        }

        public object? GetAllEntity(string sql, Type type)
        {
            var method = GetType()
                .GetMethods()
                .FirstOrDefault(m => m is { Name: nameof(GetAllEntity), IsGenericMethod: true })
                ?.MakeGenericMethod(type);

            return method?.Invoke(this, [sql]);
        }

        #endregion

        #region GetByParam

        public object? GetEntityByParam(string sql, Type type, Dictionary<string, object> parameters)
        {
            var method = GetType()
                .GetMethods()
                .FirstOrDefault(m =>m is {Name: nameof(GetEntityByParam), IsGenericMethod: true})
                ?.MakeGenericMethod(type);

            return method?.Invoke(this, [sql, parameters]);
        }

        public T? GetEntityByParam<T>(string sqlQuery, Dictionary<string, object> parameters) where T : class
        {
            try
            {
                if (parameters == null || parameters.Count == 0)
                    throw new ArgumentException("Parameters cannot be null or empty", nameof(parameters));

                return ExecuteQuery(connection =>
                {
                    var dynamicParams = DynamicParameters<T>(sqlQuery, parameters, out var finalQuery);
                    try
                    {
                        var result = connection.QuerySingleOrDefault<T>(finalQuery, dynamicParams);
                        return result;
                    }
                    catch (NpgsqlException e)
                    {
                        Logger.Log(e.Message);
                        return null;
                    }
                });
            }
            catch (ArgumentException ex)
            {
                // Логирование или обработка ошибки
                Logger.Log($"Argument Exception: {ex.Message}");
                throw;
            }
            catch (Exception e)
            {
                Logger.Log($"Unexpected Error: {e.Message}");
                throw;
            }
        }

        public T? GetEntityByParam<T>(string sqlQuery, Dictionary<string, object> parameters,Func<SqlMapper.GridReader, T> mapFunction) where T : class
        {
            if (parameters == null || parameters.Count == 0)
                throw new ArgumentException("Parameters cannot be null or empty", nameof(parameters));

            return ExecuteQuery(connection =>
            {
                var dynamicParams = DynamicParameters<T>(sqlQuery, parameters, out var finalQuery);

                try
                {
                    // Выполняем запрос с мульти-маппингом
                    using var multi = connection.QueryMultiple(finalQuery, dynamicParams);
                    return mapFunction(multi);
                }
                catch (NpgsqlException e)
                {
                    Logger.Log($"Database Error: {e.Message}");
                    return null;
                }
                catch (Exception e)
                {
                    Logger.Log($"Unexpected Error: {e.Message}");
                    throw;
                }
            });
        }

        private static DynamicParameters DynamicParameters<T>(string sqlQuery, Dictionary<string, object> parameters, out string finalQuery)
            where T : class
        {
            var dynamicParams = new DynamicParameters(); 
            var whereClause = " WHERE 1=1"; // Базовый WHERE для добавления условий

            // Формируем WHERE-условия и добавляем параметры
            foreach (var param in parameters)
            {
                whereClause += $" AND {param.Key} = @{param.Key}";
                dynamicParams.Add(param.Key, param.Value);
            }

            // Финальный SQL-запрос
            finalQuery = sqlQuery + whereClause;
            return dynamicParams;
        }

        #endregion
        private TResult? ExecuteMultiQuery<TResult>(
            NpgsqlConnection connection,
            string sqlQuery,
            Func<SqlMapper.GridReader, TResult> mapFunction)
        {
            try
            {
                using var multi = connection.QueryMultiple(sqlQuery);
                return mapFunction(multi);
            }
            catch (NpgsqlException e)
            {
                Logger.Log(e.Message);
                return default;
            }
        }

        #region Methods

        public IEnumerable<(string ColumnName, string DataType)> GetColumnNames(string query)
        {
            string schema = DatabaseConfig.DatabaseSchema;
            string tableName = GetTableName(query);

            // Запрос для получения названий колонок и их типов
            string sqlQuery = @"
                    SELECT 
                        a.attname AS column_name,
                        t.typname AS data_type
                    FROM 
                        pg_catalog.pg_attribute a
                    JOIN 
                        pg_catalog.pg_type t ON a.atttypid = t.oid
                    WHERE 
                        a.attrelid = (
                            SELECT oid 
                            FROM pg_catalog.pg_class
                            WHERE relname = @TableName
                              AND relnamespace = (
                                  SELECT oid 
                                  FROM pg_catalog.pg_namespace 
                                  WHERE nspname = @SchemaName
                              )
                        )
                    AND a.attnum > 0 
                    AND NOT a.attisdropped;";

            try
            {
                return ExecuteQuery<IEnumerable<(string ColumnName, string DataType)>>(connection =>
                {
                    connection.Query<(string ColumnName, string DataType)>(
                        sqlQuery,
                        new
                        {
                            TableName = tableName, SchemaName = schema

                        });

                    return null;
                });
            }
            catch (NpgsqlException e)
            {
                Logger.Log(e.Message);
                return [];
            }
        }

        private static string GetTableName(string sqlQuery)
        {
            var match = Regex.Match(sqlQuery, @"(?<=\bFROM\s)\w+", RegexOptions.IgnoreCase);
            return match.Value;
        }
        #endregion
    }
}
