using Library.Models;
using Library.Models.ModelsDTO;
using Library.Server.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Server.BL.Service
{
    public class BookService : BaseService, IBookService

    {

    #region Constructor

    private readonly ISqlBookProvider _sqlProvider;

    public BookService(IRepositoryManager repositoryManager, ISqlBookProvider sqlProvider,IMemoryCache cache)
        : base(repositoryManager,cache)
    {
        _sqlProvider = sqlProvider;
    }

        #endregion

        #region IGetAllService

        public async Task<IEnumerable<Book>?> GetAllEntitiesAsync()
        {
            const string cacheKey = "all_books"; // Уникальный ключ кэша

            if (Cache.TryGetValue(cacheKey, out IEnumerable<Book>? books))
                return books!;

            books = (await RepositoryManager.GetDataRepository.GetAllEntityAsync<Book>(_sqlProvider.GetAll))?.ToList();

            if (books is null || !books.Any()) // Проверяем на null и пустую коллекцию
                return []; // Возвращаем пустой список вместо null

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            Cache.Set(cacheKey, books, cacheOptions);

            return books;
        }

        public async Task<IEnumerable<Book>?> GetEntitiesByParamAsync(Dictionary<string, object> param) => await
        RepositoryManager.GetDataRepository.GetEntitiesByParamAsync<Book>(_sqlProvider.GetByParam, param);

    public async Task<Book?> GetSingleEntityByParamAsync(Dictionary<string, object> param) => await
        RepositoryManager.GetDataRepository.GetSingleEntityByParamAsync<Book>(_sqlProvider.GetByParam, param);

    public async Task<IEnumerable<BookUserActivityViewDto>?> GetBookActivityUserAsync(Dictionary<string, object> param) => await
        RepositoryManager.GetDataRepository.GetEntitiesByParamAsync<BookUserActivityViewDto>(_sqlProvider.GetActivityBook, param);

    public async Task<IEnumerable<BookUserHistoryViewDto>?> GetBookHistoryUserAsync(Dictionary<string, object> param) => await
        RepositoryManager.GetDataRepository.GetEntitiesByParamAsync<BookUserHistoryViewDto>(_sqlProvider.GetHistoryBook, param);

        #endregion

    #region IAddService

        public async Task<bool> AddEntityAsync(BookAddDto bookAddDto) => await
        RepositoryManager.ModificationRepository.AddEntityAsync<BookAddDto>(_sqlProvider.Add, bookAddDto, true);

    #endregion

    #region IDeleteService

    public async Task<bool> DeleteEntityAsync(int id) => await
        RepositoryManager.ModificationRepository.DeleteEntityByIdProcedureAsync(_sqlProvider.Delete, id);

    #endregion

    #region IUpdateService

    public async Task<bool> UpdateEntityAsync(BookUpdateInfoDto updateBookInfo)
    {
        // Получаем имена колонок таблицы
        var columnNames = await
            RepositoryManager.GetDataRepository.GetColumnNamesAsync(_sqlProvider.MainNameTable,
                _sqlProvider.GetColumnAndTypeTable);
        
        var updateParams = GetDynamicUpdateParams(updateBookInfo, columnNames!);

        return await RepositoryManager.ModificationRepository.UpdateEntityDynamicAsync(_sqlProvider.MainNameTable,
            updateParams);
    }

        #endregion

    }
}
