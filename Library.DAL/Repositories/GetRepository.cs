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
        /// <summary>
        /// Получение всех объектов из таблицы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
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


        public object? GetAllEntity(string sql, Type type)
        {
            var method = GetType()
                .GetMethods()
                .FirstOrDefault(m => m is { Name: nameof(GetAllEntity), IsGenericMethod: true })
                ?.MakeGenericMethod(type);

            return method?.Invoke(this, [sql]);
        }

        public object? GetEntityByParam(string sql, Type type, Dictionary<string, object> parameters)
        {
            var method = GetType()
                .GetMethods()
                .FirstOrDefault(m =>m is {Name: nameof(GetEntityByParam), IsGenericMethod: true})
                ?.MakeGenericMethod(type);

            return method?.Invoke(this, [sql, parameters]);
        }

        /// <summary>
        /// Получение объекта по определенным параметрам
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T? GetEntityByParam<T>(string sqlQuery, Dictionary<string, object> parameters) where T : class
        {
            try
            {
                if (parameters == null || parameters.Count == 0)
                    throw new ArgumentException("Parameters cannot be null or empty", nameof(parameters));

                return ExecuteQuery(connection =>
                {
                    var dynamicParams = new DynamicParameters();

                    // Формируем условия WHERE, добавляя параметры из словаря
                    foreach (var param in parameters)
                    {
                        sqlQuery += $" AND {param.Key} = @{param.Key}";
                        dynamicParams.Add(param.Key, param.Value);
                    }
                    try
                    {
                        var result = connection.QuerySingleOrDefault<T>(sqlQuery, dynamicParams);
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
                logger.Log($"Argument Exception: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Получение имен и их типов всех колонок в таблице
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Возвращает кортеж </returns>
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
    }
}
