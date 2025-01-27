using System.Data;
using Dapper;
using Library.DAL.Interface;

namespace Library.DAL.Repositories
{
    public class ModificationRepository: BaseRepository, IModificationRepository
    {
        #region Add

        /// <summary>
        /// Добавление объекта в базу данных.
        /// </summary>
        public async Task<bool> AddEntityAsync<T>(string sqlQuery, T entity, bool isStoredProcedure = false) where T : class
        {
            const string method = nameof(AddEntityAsync);

            if (IsNullFields(sqlQuery, method) || IsNullFields(entity, method))
                return false;
            try
            {
                return await ExecuteQuery(async connection =>
                {
                    var dynamicParams = new DynamicParameters();

                    // Добавляем свойства объекта в параметры
                    var properties = typeof(T).GetProperties();

                    foreach (var property in properties)
                    {
                        if (property.CanRead)
                        {
                            dynamicParams.Add(property.Name.ToLower(), property.GetValue(entity));
                        }
                    }

                    //Выполняем запрос или хранимую процедуру в зависимости от флага isStoredProcedure
                    var result = await connection.ExecuteAsync(sqlQuery, dynamicParams, commandType: isStoredProcedure
                        ? CommandType.StoredProcedure
                        : CommandType.Text);
                    return result > 0;
                });
            }
            catch (Exception ex)
            {
                Logger.Info($"Error in AddEntity: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Update

        /// <summary>
        /// Обновление объекта на основе динамических параметров.
        /// </summary>
        public async Task<bool> UpdateEntityDynamicAsync(string tableName, Dictionary<string, object> parameters)
        {
            const string method = nameof(UpdateEntityDynamicAsync);

            if ((IsNullFields(tableName, method) || IsNullFields(parameters, method)) && !parameters.ContainsKey("id"))
            {
                Logger.Info("Parameters must include an 'id' for the WHERE clause.");
                return false;
            }

            // Генерация SQL-запроса
            var sqlQuery = GenerateUpdateQuery(tableName, parameters);

            // Выполнение запроса
            return await ExecuteQuery(async connection =>
            {
                var result = await connection.ExecuteAsync(sqlQuery, new DynamicParameters(parameters));
                return result > 0;
            });
        }

        private static string GenerateUpdateQuery(string tableName, Dictionary<string, object> parameters)
        {
            var setClause = string.Join(", ", parameters.Keys
                .Where(key => !key.Equals("id", StringComparison.OrdinalIgnoreCase))
                .Select(key => $"{key} = @{key}"));

            return $@"
                    UPDATE {tableName} 
                    SET {setClause} 
                    WHERE id = @id";
        }

        #endregion

        #region Delete

        /// <summary>
        /// Удаление объекта по ID через хранимую процедуру.
        /// </summary>
        public async Task<bool> DeleteEntityByIdProcedureAsync(string procedureName, int id)
        {
            if (IsNullFields(procedureName, nameof(DeleteEntityByIdProcedureAsync)))
                return false;
            try
            {
                return await ExecuteQuery(async connection =>
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("_id", id);

                    var result = await connection.ExecuteAsync(procedureName, dynamicParams,
                        commandType: CommandType.StoredProcedure);
                    return result > 0;
                });
            }
            catch (Exception ex)
            {
                Logger.Info($"Error in DeleteEntityByIdProcedureAsync: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Удаление объекта на основе динамических параметров.
        /// </summary>
        public async Task<bool> DeleteEntityDynamicAsync(string tableName, Dictionary<string, object> whereParameters)
        {
            const string method = nameof(DeleteEntityDynamicAsync);

            if (IsNullFields(tableName, method) || IsNullFields(whereParameters, method))
                return false;

            try
            {
                return await ExecuteQuery(async connection =>
                {
                    var whereClauses = whereParameters.Keys.Select(key => $"{key} = @{key}");
                    var sqlQuery = $"DELETE FROM {tableName} WHERE {string.Join(" AND ", whereClauses)}";

                    var dynamicParams = new DynamicParameters();
                    foreach (var param in whereParameters)
                    {
                        dynamicParams.Add(param.Key, param.Value);
                    }

                    var result = await connection.ExecuteAsync(sqlQuery, dynamicParams);
                    return result > 0;
                });
            }
            catch (Exception ex)
            {
                Logger.Info($"Error in DeleteEntityDynamicAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteEntityDynamicAsync<T>(string tableName, T entity)
        {
            if (IsNullFields(tableName, nameof(DeleteEntityDynamicAsync)))
                return false;

            try
            {
                return await ExecuteQuery(async connection =>
                {
                    var dynamicParams = new DynamicParameters();

                    var property = entity!.GetType().GetProperties();

                    foreach (var param in property)
                    {
                        dynamicParams.Add(param.Name.ToLower(), param.GetValue(entity));
                    }

                    var whereClauses =
                        dynamicParams.ParameterNames.Select((paramName => $"{paramName} = @{paramName}"));

                    var sqlQuery = $"DELETE FROM {tableName} WHERE {string.Join(" AND ", whereClauses)}";

                    var result = await connection.ExecuteAsync(sqlQuery, dynamicParams);
                    return result > 0;
                });
            }
            catch (Exception ex)
            {
                Logger.Info($"Error in DeleteEntityDynamicAsync: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}