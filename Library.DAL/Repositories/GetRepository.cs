using Dapper;
using Library.Common;
using Npgsql;
using Library.DAL.Configuration;
using Library.DAL.Interface;

namespace Library.DAL.Repositories
{
    public class GetRepository(IMessageLogger logger) : BaseRepository(logger),IGetRepository
    {
        #region GetAll

        /// <summary>
        /// Выполняет SQL-запрос для получения всех сущностей типа <typeparamref name="T"/> из базы данных.
        /// </summary>
        /// <typeparam name="T">Тип сущности, который будет возвращен в результате выполнения запроса.</typeparam>
        /// <param name="sqlQuery">SQL-запрос, который будет выполнен для получения данных.</param>
        /// <returns>Коллекция сущностей типа <typeparamref name="T"/>, или <c>null</c> в случае ошибки выполнения запроса.</returns>
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

        //Todo
        //public object? GetEntityByParam(string sql, Type type, Dictionary<string, object> parameters)
        //{
        //    var method = GetType()
        //        .GetMethods()
        //        .FirstOrDefault(m =>m is {Name: nameof(GetSingleEntityByParam), IsGenericMethod: true})
        //        ?.MakeGenericMethod(type);

        //    return method?.Invoke(this, [sql, parameters]);
        //}

        /// <summary>
        /// Выполняет SQL-запрос для получения одного объекта типа <typeparamref name="T"/> на основе переданных параметров.
        /// </summary>
        /// <typeparam name="T">Тип объекта, который будет возвращен в результате выполнения запроса.</typeparam>
        /// <param name="sqlQuery">SQL-запрос для выполнения.</param>
        /// <param name="parameters">Словарь параметров, которые передаются в запрос.</param>
        /// <returns>Объект типа <typeparamref name="T"/>, если найден, иначе null.</returns>
        public T? GetSingleEntityByParam<T>(string sqlQuery, Dictionary<string, object> parameters) where T : class
        {
            var method = nameof(ExecuteWithParameters);
            if (IsNullFields(parameters, method) || IsNullFields(sqlQuery, method))
                return null;
            try
            {

                return ExecuteWithParameters(
                    nameof(GetSingleEntityByParam),
                    sqlQuery,
                    parameters,
                    (connection, dynamicParams, finalQuery) =>
                    {
                        var result = GetMethod<T>(nameof(GetSingleEntityByParam), finalQuery, dynamicParams, connection)
                            .Invoke();
                        return result as T;
                    }
                );
            }
            catch (Exception e)
            {
                Logger.Log($"Unexpected Error in {method}: {e.Message}");
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
        public IEnumerable<T>? GetEntitiesByParam<T>(string sqlQuery, Dictionary<string, object> parameters) where T : class
        {
            var method = nameof(ExecuteWithParameters);
            if (IsNullFields(parameters, method) || IsNullFields(sqlQuery, method))
                return null;
            try
            {
                return ExecuteWithParameters(
                    nameof(GetEntitiesByParam),
                    sqlQuery,
                    parameters,
                    (connection, dynamicParams, finalQuery) =>
                    {
                        var result = GetMethod<T>(nameof(GetEntitiesByParam), finalQuery, dynamicParams, connection)
                            .Invoke();
                        return result as IEnumerable<T>; // Приведение результата
                    }
                );
            }
            catch (Exception e)
            {
                Logger.Log($"Unexpected Error in {method}: {e.Message}");
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
        private T? ExecuteWithParameters<T>(string methodName, string sqlQuery, Dictionary<string, object>? parameters, Func<NpgsqlConnection, DynamicParameters, string, T?> executeQuery) where T : class
        {
            try
            {
                return ExecuteQuery(connection =>
                {
                    try
                    {
                        var dynamicParams = DynamicParameters<T>(sqlQuery, parameters!, out var finalQuery);
                        return executeQuery(connection, dynamicParams, finalQuery);
                    }
                    catch (NpgsqlException e)
                    {
                        Logger.Log($"Database Error in {methodName}: {e.Message}");
                        return null;
                    }
                });
            }
            catch (Exception e)
            {
                Logger.Log($"Unexpected Error in {methodName}: {e.Message}");
                return null;
            }
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
        private static Func<object?> GetMethod<T>(string methodName, string finalQuery, DynamicParameters dynamicParams, NpgsqlConnection connection)
        {
            if (finalQuery == null) throw new ArgumentNullException(nameof(finalQuery));
            return methodName switch
            {
                "GetSingleEntityByParam" =>
                    () => connection.QuerySingleOrDefault<T>(finalQuery, dynamicParams),

                "GetEntitiesByParam" =>
                    () => connection.Query<T>(finalQuery, dynamicParams),

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

        public IEnumerable<string>? GetColumnNames(string tableName, string sqlQuery)
        {
            string schema = DatabaseConfig.DatabaseSchema;
            var method = nameof(GetColumnNames);

            if (IsNullFields(tableName, method) || IsNullFields(sqlQuery, method) || IsNullFields(schema, method))
                return null;

            try
            {
                return ExecuteQuery(connection =>
                {
                    var result = connection.Query<string>(
                        sqlQuery,
                        new { TableName = tableName, SchemaName = schema }
                    );
                    return result;
                });
            }
            catch (NpgsqlException e)
            {
                Logger.Log(e.Message);
                return Array.Empty<string>(); // Возвращаем пустой массив.
            }
        }
        #endregion
    }
}
