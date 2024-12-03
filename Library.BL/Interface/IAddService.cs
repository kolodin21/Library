namespace Library.BL.Interface
{
    public interface IAddService<in T>
    {
        bool AddEntity(T entity);
    }
}
 