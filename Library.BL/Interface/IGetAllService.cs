using Dapper;

namespace Library.BL.Interface
{
    public interface IGetAllService<T>
    {
        Task<IEnumerable<T>?> GetAllEntitiesAsync();
    }
}
