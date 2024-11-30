
namespace Library.DAL.Interface
{
    public interface IGetRepository
    {
        IEnumerable<T>? GetAllEntity<T>(string query) where T : class;
        T? GetEntityByParam<T>(string query, Dictionary<string, object> param) where T : class;
    }
}
