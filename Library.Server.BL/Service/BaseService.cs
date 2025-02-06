using System.Text;
using Library.Server.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Server.BL.Service
{

    public static class Prefix
    {
        public static string History => "HistoryBookUser";
        public static string Activity => "BookActivityUser";
        public static string AllBooks => "AllBooks";

    }



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

        //Универсальный метод для кэширования запросов и получения данных их кэша
        protected async Task<IEnumerable<T>?> GetSetCache<T>(
            string cacheKey,
            Func<Task<IEnumerable<T>?>> getEntity,
            int timeCache = 2)
        {
            if (Cache.TryGetValue(cacheKey, out IEnumerable<T>? entity))
                return entity;

            // Данные не найдены в кэше, загружаем
            entity = (await getEntity())?.ToList() ?? [];

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(timeCache));

            Cache.Set(cacheKey, entity, options);

            return entity.Any() ? entity : null; // Если список пустой, возвращаем null, но в кэш кладем пустой
        }

        //Генерация ключа для кэша
        protected string GenerateCacheKey(string prefix, Dictionary<string, object> param)
        {
            var paramString = new StringBuilder();

            foreach (var item in param)
            {
                paramString.Append($"{item.Key}:{item.Value}");
                paramString.Append(" ");  // Добавляем пробел между параметрами
            }

            return $"{prefix}_{paramString.ToString().Trim()}"; // Убираем лишний пробел в конце
        }

        public void RemoveCache(string cacheKey)
        {
            Cache.Remove(cacheKey);
        }
    }
}
