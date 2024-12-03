using System.Data;
using System.Text;
using Dapper;
using Library.Common;
using Library.DAL.Interface;
using Library.DAL.Models;

namespace Library.DAL.Repositories
{
    public class ModificationRepository(IMessageLogger logger) : BaseRepository(logger), IModificationRepository
    {

        #region Add

        /// <summary>
        /// Добавление объекта в базу данных.
        /// </summary>
        public bool AddEntity<T>(string sqlQuery, T entity, bool isStoredProcedure = false) where T : class
        {
            var method = nameof(AddEntity);

            if (IsNullFields(sqlQuery, method) || IsNullFields(entity, method))
                return false;
            try
            {
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
                return false;
            }
        }
        #endregion

        #region Update

        /// <summary>
        /// Обновление объекта на основе динамических параметров.
        /// </summary>
        public bool UpdateEntityDynamic(string tableName, Dictionary<string, object> parameters)
        {
            var method = nameof(UpdateEntityDynamic);

            if ((IsNullFields(tableName, method) || IsNullFields(parameters, method)) && !parameters.ContainsKey("id"))
            {
                Logger.Log("Parameters must include an 'id' for the WHERE clause.");
                return false;
            }

            // Генерация SQL-запроса
            string sqlQuery = GenerateUpdateQuery(tableName, parameters);

            // Выполнение запроса
            return ExecuteQuery(connection =>
            {
                var result = connection.Execute(sqlQuery, new DynamicParameters(parameters));
                return result > 0;
            });
        }

        private string GenerateUpdateQuery(string tableName, Dictionary<string, object> parameters)
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
        public bool DeleteEntityByIdProcedure(string procedureName, int id)
        {
            if (IsNullFields(procedureName, nameof(DeleteEntityByIdProcedure)))
                return false;
            try
            {
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
                return false;
            }
        }

        /// <summary>
        /// Удаление объекта на основе динамических параметров.
        /// </summary>
        public bool DeleteEntityDynamic(string tableName, Dictionary<string, object> whereParameters)
        {
            var method = nameof(DeleteEntityDynamic);
            if (IsNullFields(tableName, method) || IsNullFields(whereParameters, method))
                return false;

            try
            {
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
                return false;
            }
        }

        public bool DeleteEntityDynamic<T>(string tableName, T entity)
        {
            if (IsNullFields(tableName, nameof(DeleteEntityDynamic)))
                return false;

            try
            {
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
                return false;
            }
        }
        #endregion
    }
}

