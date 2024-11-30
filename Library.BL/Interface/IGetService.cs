using Dapper;

namespace Library.BL.Interface
{
    public interface IGetService<out T>
    {
        /// <summary>
        /// Получить все сущности.
        /// </summary>
        /// <returns>Список сущностей или null, если их нет.</returns>
        IEnumerable<T>? GetAllEntities();

        /// <summary>
        /// Получить сущность по указанным параметрам.
        /// </summary>
        /// <param name="param">Параметры для поиска сущности в виде словаря.</param>
        /// <returns>Объект сущности или null, если сущность не найдена.</returns>
        T? GetEntityByParam(Dictionary<string, object> param);
    }
}
