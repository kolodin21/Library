namespace Library.BL.Interface;

public interface IGetByParamService<out T>
{
    IEnumerable<T>? GetEntitiesByParam(Dictionary<string, object> param);
}

public interface IGetSingleByParam<out T>
{
    T? GetSingleEntityByParam(Dictionary<string, object> param);
}