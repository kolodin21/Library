using Dapper;
using Library.Server.DAL.Configuration;
using Library.Server.DAL.Interface;
using NLog;
using Npgsql;

namespace Library.Server.DAL.Repositories
{
    public class GetRepository : BaseRepository,IGetRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region GetAll

        /// <summary>
        /// Выполняет SQL-запрос для получения всех сущностей типа <typeparamref name="T"/> из базы данных.
        /// </summary>
        /// <typeparam name="T">Тип сущности, который будет возвращен в результате выполнения запроса.</typeparam>
        /// <param name="sqlQuery">SQL-запрос, который будет выполнен для получения данных.</param>
        /// <returns>Коллекция сущностей типа <typeparamref name="T"/>, или <c>null</c> в случае ошибки выполнения запроса.</returns>
        public async Task<IEnumerable<T>?> GetAllEntityAsync<T>(string sqlQuery) where T : class
        {
            return await ExecuteQuery(async connection =>
            {
                try
                {
                    var result = await connection.QueryAsync<T>(sqlQuery);
                    return result;
                }
                catch (NpgsqlException e)
                {
                    Logger.Info(e.Message);
                    return null;
                }
            });
        }
        
        //public IEnumerable<T>? GetAllEntity<T>(string sqlQuery, Func<SqlMapper.GridReader, IEnumerable<T>> mapFunction) where T : class
        //{
        //    return ExecuteQuery(connection =>
        //    {
        //        try
        //        {
        //            using var multi = connection.QueryMultiple(sqlQuery);
        //            return mapFunction(multi);
        //        }
        //        catch (NpgsqlException e)
        //        {
        //            Logger.Log(e.Message);
        //            return null;
        //        }
        //    });
        //}

        //public object? GetAllEntity(string sql, Type type)
        //{
        //    var method = GetType()
        //        .GetMethods()
        //        .FirstOrDefault(m => m is { Name: nameof(GetAllEntity), IsGenericMethod: true })
        //        ?.MakeGenericMethod(type);

        //    return method?.Invoke(this, [sql]);
        //}

        #endregion

        #region GetByParam

        /// <summary>
        /// Выполняет SQL-запрос для получения одного объекта типа <typeparamref name="T"/> на основе переданных параметров.
        /// </summary>
        /// <typeparam name="T">Тип объекта, который будет возвращен в результате выполнения запроса.</typeparam>
        /// <param name="sqlQuery">SQL-запрос для выполнения.</param>
        /// <param name="parameters">Словарь параметров, которые передаются в запрос.</param>
        /// <returns>Объект типа <typeparamref name="T"/>, если найден, иначе null.</returns>
        public async Task<T?> GetSingleEntityByParamAsync<T>(string sqlQuery, Dictionary<string, object> parameters) where T : class
        {
            string method = nameof(ExecuteWithParametersAsync);
            if (IsNullFields(parameters, method) || IsNullFields(sqlQuery, method))
                return null;
            try
            {

                return await ExecuteWithParametersAsync(
                    nameof(GetSingleEntityByParamAsync),
                    sqlQuery,
                    parameters,
                    async (connection, dynamicParams, finalQuery) =>
                    {
                        var result = await GetMethodAsync<T>(nameof(GetSingleEntityByParamAsync), finalQuery, dynamicParams, connection)
                            .Invoke();
                        return result as T;
                    }
                );
            }
            catch (Exception e)
            {
                Logger.Info($"Unexpected Error in {method}: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Выполняет SQL-запрос для получения коллекции объектов типа <typeparamref name="T"/> на основе переданных параметров.
        /// </summary>
        /// <typeparam name="T">Тип объектов, которые будут возвращены в результате выполнения запроса.</typeparam>
        /// <param name="sqlQuery">SQL-запрос для выполнения.</param>
        /// <param name="parameters">Словарь параметров, которые передаются в запрос.</param>
        /// <returns>Коллекция объектов типа <typeparamref name="T"/>, если найдены, иначе null.</returns>
        public async Task<IEnumerable<T>?> GetEntitiesByParamAsync<T>(string sqlQuery, Dictionary<string, object> parameters) where T : class
        {
            string method = nameof(ExecuteWithParametersAsync);
            if (IsNullFields(parameters, method) || IsNullFields(sqlQuery, method))
                return null;
            try
            {
                return await ExecuteWithParametersAsync
                (
                    nameof(GetEntitiesByParamAsync),
                    sqlQuery,
                    parameters,
                    async (connection, dynamicParams, finalQuery) =>
                    {
                        var result = await GetMethodAsync<T>(nameof(GetEntitiesByParamAsync), finalQuery, dynamicParams, connection)
                            .Invoke();
                        return result as IEnumerable<T>; // Приведение результата
                    }
                );
            }
            catch (Exception e)
            {
                Logger.Info($"Unexpected Error in {method}: {e.Message}");
                return null;
            }
        }
        #endregion

        #region MethodsByGetByParam

        /// <summary>
        /// Обрабатывает выполнение SQL-запроса с параметрами и делегатом для возврата результата.
        /// </summary>
        /// <typeparam name="T">Тип объекта, который должен быть возвращен в результате выполнения запроса.</typeparam>
        /// <param name="methodName">Имя метода, который вызывается (для логирования и диагностики).</param>
        /// <param name="sqlQuery">SQL-запрос для выполнения.</param>
        /// <param name="parameters">Словарь параметров, которые передаются в запрос.</param>
        /// <param name="executeQuery">Делегат, который выполняет запрос и возвращает результат.</param>
        /// <returns>Результат выполнения запроса типа <typeparamref name="T"/>, или null, если произошла ошибка.</returns>
        private async Task<T?> ExecuteWithParametersAsync<T>(
            string methodName,
            string sqlQuery,
            Dictionary<string, object>? parameters,
            Func<NpgsqlConnection, DynamicParameters, string, Task<T?>> executeQuery
        ) where T : class
        {
            if (parameters != null)

                return await ExecuteQuery(async connection =>
                {
                    var dynamicParams = DynamicParameters<T>(sqlQuery, parameters, out var finalQuery);
                    return await executeQuery(connection, dynamicParams, finalQuery);
                });
            Logger.Info($"Parameters are null in {methodName}");
            return null;

        }

        /// <summary>
        /// Создает делегат для выполнения SQL-запроса, в зависимости от типа возвращаемых данных (один объект или коллекция).
        /// </summary>
        /// <typeparam name="T">Тип объекта или коллекции объектов, которые будут возвращены.</typeparam>
        /// <param name="methodName">Имя метода, для которого выбирается запрос.</param>
        /// <param name="finalQuery">SQL-запрос, который нужно выполнить.</param>
        /// <param name="dynamicParams">Параметры, которые передаются в запрос.</param>
        /// <param name="connection">Соединение с базой данных.</param>
        /// <returns>Делегат, который выполняет запрос и возвращает результат.</returns>
        private static Func<Task<object?>> GetMethodAsync<T>(string methodName, string finalQuery, DynamicParameters dynamicParams, NpgsqlConnection connection)
        {
            if (finalQuery == null) 
                throw new ArgumentNullException(nameof(finalQuery));

            return methodName switch
            {
                "GetSingleEntityByParamAsync" =>
                    async () => await connection.QuerySingleOrDefaultAsync<T>(finalQuery, dynamicParams),

                "GetEntitiesByParamAsync" =>
                    async () => await connection.QueryAsync<T>(finalQuery, dynamicParams),

                _ => throw new ArgumentException("Invalid method name", nameof(methodName))
            };
        }
        
        /// <summary>
        /// Формирует параметры для SQL-запроса с учетом переданных значений.
        /// </summary>
        /// <typeparam name="T">Тип, который используется в контексте выполнения запроса.</typeparam>
        /// <param name="sqlQuery">Исходный SQL-запрос, к которому будут добавлены условия.</param>
        /// <param name="parameters">Словарь параметров, которые должны быть добавлены в запрос.</param>
        /// <param name="finalQuery">Итоговый SQL-запрос с добавленными параметрами.</param>
        /// <returns>Экземпляр <see cref="DynamicParameters"/>, содержащий все параметры для выполнения запроса.</returns>
        private static DynamicParameters DynamicParameters<T>(string sqlQuery, Dictionary<string, object> parameters, out string finalQuery)
            where T : class
        {
            var dynamicParams = new DynamicParameters(); 
            // Формируем WHERE-условия и добавляем параметры
            foreach (var param in parameters)
            {
                sqlQuery += $" AND {param.Key} = @{param.Key}";
                dynamicParams.Add(param.Key, param.Value);
            }
            // Финальный SQL-запрос
            finalQuery = sqlQuery;
            return dynamicParams;
        }

        #endregion

        #region Methods

        public Task<IEnumerable<string>> GetColumnNamesAsync(string tableName, string sqlQuery)
        {
            var schema = DatabaseConfig.DatabaseSchema;
            const string method = nameof(GetColumnNamesAsync);

            if (IsNullFields(tableName, method) || IsNullFields(sqlQuery, method) || IsNullFields(schema, method))
                return Task.FromResult<IEnumerable<string>>(Array.Empty<string>());

            return ExecuteQuery(async connection =>
            {
                try
                {
                    var result = await connection.QueryAsync<string>(
                        sqlQuery,
                        new { TableName = tableName, SchemaName = schema }
                    );
                    return result;
                }
                catch (NpgsqlException e)
                {
                    Logger.Info($"Database error in {method}: {e.Message}");
                    return Array.Empty<string>(); // Возвращаем пустой массив.
                }
            });
        }
        #endregion
    }
}