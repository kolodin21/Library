namespace Library.DAL.Interface
{
    public interface IModificationRepository
    {
        bool AddEntity<T>(string query, T entity, bool isStoredProcedure = false) where T : class;
        bool DeleteEntityByIdProcedure(string query, int id);
        bool DeleteEntityDynamic(string query, Dictionary<string, object> param);
        bool DeleteEntityDynamic<T>(string query, T entity);
        bool UpdateEntityDynamic(string tableName, Dictionary<string, object> param);
    }
}
