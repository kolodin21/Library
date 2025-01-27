namespace Library.BL.Interface;

public interface IDeleteService<in T>
{
    Task<bool> DeleteEntity(T entity);
}

public interface IDeleteServiceByParam
{
    Task<bool> DeleteEntity(Dictionary<string, object> param);
}

public interface IDeleteServiceByIdProcedure
{
    Task<bool> DeleteEntity(int id);
}