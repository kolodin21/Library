namespace Library.BL.Interface;

public interface IDeleteService<in T>
{
    Task<bool> DeleteEntityAsync(T entity);
}

public interface IDeleteServiceByParam
{
    Task<bool> DeleteEntityAsync(Dictionary<string, object> param);
}

public interface IDeleteServiceByIdProcedure
{
    Task<bool> DeleteEntityAsync(int id);
}