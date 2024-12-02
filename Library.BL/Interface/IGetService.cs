using Dapper;

namespace Library.BL.Interface
{
    public interface IGetService<out T>
    {
        IEnumerable<T>? GetAllEntities();
        T? GetSingleEntityByParam(Dictionary<string, object> param);
        IEnumerable<T>? GetEntitiesByParam(Dictionary<string, object> param);
    }
}
