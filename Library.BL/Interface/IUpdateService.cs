namespace Library.BL.Interface;

public interface IUpdateService<in T> where T : class
{
    bool UpdateEntity(T entity);
}