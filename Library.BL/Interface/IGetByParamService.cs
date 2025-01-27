namespace Library.BL.Interface;

public interface IGetByParamService<T>
{
    Task<IEnumerable<T>?> GetEntitiesByParamAsync(Dictionary<string, object> param);
}

public interface IGetSingleByParam<T>
{
    Task<T?> GetSingleEntityByParamAsync(Dictionary<string, object> param);
}