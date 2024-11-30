namespace Library.BL.Interface
{
    public interface IAddEntity<in T>
    {
        bool AddEntity(T entity);
    }
}
 