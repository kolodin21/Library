namespace Library.DAL.Interface
{
    public interface IModificationRepository
    {
        Task<bool> AddEntity<T>(string query, T entity, bool isStoredProcedure = false) where T : class;
        Task<bool> DeleteEntityByIdProcedure(string query, int id);
        Task<bool> DeleteEntityDynamic(string query, Dictionary<string, object> param);
        Task<bool> DeleteEntityDynamic<T>(string query, T entity);
        Task<bool> UpdateEntityDynamic(string tableName, Dictionary<string, object> param);
    }
}