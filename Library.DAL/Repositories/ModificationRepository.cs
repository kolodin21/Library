using System.Data;
using Dapper;
using Library.Common;
using Library.DAL.Interface;

namespace Library.DAL.Repositories
{
    public class ModificationRepository(IMessageLogger logger) : BaseRepository(logger), IModificationRepository
    {
        /// <summary>
        /// Добавление объекта в базу данных.
        /// </summary>
        public bool AddEntity<T>(string sqlQuery, T entity, bool isStoredProcedure = false) where T : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlQuery))
                    throw new ArgumentException("SQL query cannot be null or empty.", nameof(sqlQuery));

                if (entity == null)
                    throw new ArgumentException("Entity cannot be null.", nameof(entity));

                return ExecuteQuery(connection =>
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
                    var result = connection.Execute(sqlQuery, dynamicParams, commandType: isStoredProcedure
                        ? CommandType.StoredProcedure
                        : CommandType.Text);
                    return result > 0;
                });
            }
            catch (Exception ex)
            {
                Logger.Log($"Error in AddEntity: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Обновление объекта на основе динамических параметров.
        /// </summary>
        public bool UpdateEntityDynamic(string tableName, Dictionary<string, object> parameters)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));

            if (parameters == null || parameters.Count == 0)
                throw new ArgumentException("Parameters cannot be null or empty.", nameof(parameters));

            // Разделение параметров на SET и WHERE
            var setClauseParameters = parameters.Where(p => !p.Key.Equals("id", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(p => p.Key, p => p.Value);
            var whereClauseParameters = parameters.Where(p => p.Key.Equals("id", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(p => p.Key, p => p.Value);

            if (!whereClauseParameters.Any())
                throw new ArgumentException(
                    "Parameters must include at least one 'id' or unique identifier for the WHERE clause.");

            // Формирование частей запроса
            string setClause = string.Join(", ", setClauseParameters.Keys.Select(key => $"{key} = @{key}"));
            string whereClause = string.Join(" AND ", whereClauseParameters.Keys.Select(key => $"{key} = @{key}"));

            // Формирование SQL-запроса
            string sqlQuery = $@"UPDATE {tableName} 
                         SET {setClause}
                         WHERE {whereClause}";

            // Выполнение запроса
            return ExecuteQuery(connection =>
            {
                var dynamicParams = new DynamicParameters();

                // Добавляем все параметры
                foreach (var param in parameters)
                {
                    dynamicParams.Add(param.Key, param.Value);
                }

                var result = connection.Execute(sqlQuery, dynamicParams);
                return result > 0;
            });
        }

        /// <summary>
        /// Удаление объекта по ID через хранимую процедуру.
        /// </summary>
        public bool DeleteEntityByIdProcedure(string procedureName, int id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(procedureName))
                    throw new ArgumentException("Procedure name cannot be null or empty.", nameof(procedureName));

                return ExecuteQuery(connection =>
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("_id", id);

                    var result = connection.Execute(procedureName, dynamicParams,
                        commandType: CommandType.StoredProcedure);
                    return result > 0;
                });
            }
            catch (Exception ex)
            {
                Logger.Log($"Error in DeleteEntityByIdProcedure: {ex.Message}");
                throw;
            }
        } //Fixme

        /// <summary>
        /// Удаление объекта на основе динамических параметров.
        /// </summary>
        public bool DeleteEntityDynamic(string tableName, Dictionary<string, object> whereParameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tableName))
                    throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));

                if (whereParameters == null || whereParameters.Count == 0)
                    throw new ArgumentException("WHERE parameters cannot be null or empty.", nameof(whereParameters));

                return ExecuteQuery(connection =>
                {
                    var whereClauses = whereParameters.Keys.Select(key => $"{key} = @{key}");
                    string sqlQuery = $"DELETE FROM {tableName} WHERE {string.Join(" AND ", whereClauses)}";

                    var dynamicParams = new DynamicParameters();
                    foreach (var param in whereParameters)
                    {
                        dynamicParams.Add(param.Key, param.Value);
                    }

                    var result = connection.Execute(sqlQuery, dynamicParams);
                    return result > 0;
                });
            }
            catch (Exception ex)
            {
                Logger.Log($"Error in DeleteEntityDynamic: {ex.Message}");
                throw;
            }
        }

        public bool DeleteEntityDynamic<T>(string tableName, T entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tableName))
                    throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));


                return ExecuteQuery(connection =>
                {
                    var dynamicParams = new DynamicParameters();

                    var property = entity!.GetType().GetProperties();

                    foreach (var param in property)
                    {
                        dynamicParams.Add(param.Name.ToLower(), param.GetValue(entity));
                    }

                    var whereClauses =
                        dynamicParams.ParameterNames.Select((paramName => $"{paramName} = @{paramName}"));

                    string sqlQuery = $"DELETE FROM {tableName} WHERE {string.Join(" AND ", whereClauses)}";

                    var result = connection.Execute(sqlQuery, dynamicParams);
                    return result > 0;
                });
            }
            catch (Exception ex)
            {
                Logger.Log($"Error in DeleteEntityDynamic: {ex.Message}");
                throw;
            }
        }
    }
}

