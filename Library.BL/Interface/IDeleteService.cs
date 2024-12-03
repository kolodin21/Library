namespace Library.BL.Interface;

public interface IDeleteService<in T>
{
    bool DeleteEntity(T entity);
}

public interface IDeleteServiceByParam
{
    bool DeleteEntity(Dictionary<string, object> param);
}

public interface IDeleteServiceByIdProcedure
{
    bool DeleteEntity(int id);
}