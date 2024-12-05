using Dapper;

namespace Library.BL.Interface
{
    public interface IGetAllService<out T>
    {
        IEnumerable<T>? GetAllEntities();
    }
}
