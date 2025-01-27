namespace Library.DAL.Interface
{
    public interface IModificationRepository
    {
        Task<bool> AddEntityAsync<T>(string query, T entity, bool isStoredProcedure = false) where T : class;
        Task<bool> DeleteEntityByIdProcedureAsync(string query, int id);
        Task<bool> DeleteEntityDynamicAsync(string query, Dictionary<string, object> param);
        Task<bool> DeleteEntityDynamicAsync<T>(string query, T entity);
        Task<bool> UpdateEntityDynamicAsync(string tableName, Dictionary<string, object> param);
    }
}