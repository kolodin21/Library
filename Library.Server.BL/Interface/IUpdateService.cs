namespace Library.Server.BL.Interface;

public interface IUpdateService<in T> 
{
    Task<bool> UpdateEntityAsync(T entity);
}