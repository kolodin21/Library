namespace Library.Server.DAL.Interface
{
    public interface IGetRepository
    {
        Task<IEnumerable<T>?> GetAllEntityAsync<T>(string query) where T : class;
        Task<T?> GetSingleEntityByParamAsync<T>(string query, Dictionary<string, object> param) where T : class;
        Task<IEnumerable<T>?> GetEntitiesByParamAsync<T>(string query, Dictionary<string, object> param) where T : class;
        Task<IEnumerable<string>> GetColumnNamesAsync(string tableName, string sqlQuery);
    }
}