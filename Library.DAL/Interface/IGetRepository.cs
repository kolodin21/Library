using Dapper;

namespace Library.DAL.Interface
{
    public interface IGetRepository
    {
        IEnumerable<T>? GetAllEntity<T>(string query) where T : class;
        T? GetEntityByParam<T>(string query, Dictionary<string, object> param) where T : class;

        IEnumerable<T>? GetAllEntity<T>(string sqlQuery, Func<SqlMapper.GridReader, IEnumerable<T>> mapFunction) where T : class;

        public T? GetEntityByParam<T>(string sqlQuery, Dictionary<string, object> parameters,
            Func<SqlMapper.GridReader, T> mapFunction) where T : class;
    }
}
