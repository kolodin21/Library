namespace Library.BL.Interface
{
    public interface IModificationService<in T>
    {
        bool AddEntity(T entity);
        bool DeleteEntity(T entity);
        bool DeleteEntity(Dictionary<string, object> param);
        //bool UpdateEntity(T entity);
    }

    public interface IUpdateEntity<in T>
    {
        bool UpdateEntity(T entity);
    }
}
 