namespace Library.BL.Interface;

public interface IUpdateService<T> 
{
    bool UpdateEntity(T entity);
}