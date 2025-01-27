namespace Library.BL.Interface
{
    public interface IAddService<in T>
    {
        Task<bool> AddEntity(T entity);
    }
}
 