namespace Library.BL.Interface;

public interface IUpdateService<in T> 
{
    Task<bool> UpdateEntityAsync(T entity);
}