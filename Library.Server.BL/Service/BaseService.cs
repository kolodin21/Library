using Library.Server.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Server.BL.Service
{
    public abstract class BaseService
    {
        protected readonly IRepositoryManager RepositoryManager;
        protected readonly IMemoryCache Cache;
        protected BaseService(IRepositoryManager repositoryManager, IMemoryCache cache)
        {
            RepositoryManager = repositoryManager;
            Cache= cache;
        }

        protected Dictionary<string, object>? GetDynamicUpdateParams<T>(T entity, IEnumerable<string> columnNames) where T : class
        {
            var updateParams = new Dictionary<string, object>();
            // Получаем свойства объекта
            var properties = entity.GetType().GetProperties();

            foreach (var property in properties)
            {
                var columnName = columnNames.FirstOrDefault(c =>
                    string.Equals(c, property.Name, StringComparison.OrdinalIgnoreCase));

                if (property.GetValue(entity) != null && columnName != null)
                {
                    updateParams[columnName] = property.GetValue(entity)!;
                }
            }
            return updateParams;
        }
    }
}
