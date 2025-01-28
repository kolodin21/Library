namespace Library.Server.BL.Interface
{
    public interface IAddService<in T>
    {
        Task<bool> AddEntityAsync(T entity);
    }
}
 