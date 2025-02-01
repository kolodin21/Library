using Library.Models;
using Library.Models.ModelsDTO;
using Library.Server.BL.Interface;
using Library.Server.DAL.Repositories;

namespace Library.Server.BL.Service
{
    public class BookService : BaseService, IBookService

    {

    #region Constructor

    private readonly ISqlBookProvider _sqlProvider;

    public BookService(IRepositoryManager repositoryManager, ISqlBookProvider sqlProvider)
        : base(repositoryManager)
    {
        _sqlProvider = sqlProvider;
    }

    #endregion

    #region IGetAllService

    public async Task<IEnumerable<Book>?> GetAllEntitiesAsync() => await
        RepositoryManager.GetDataRepository.GetAllEntityAsync<Book>(_sqlProvider.GetAll);

    public async Task<IEnumerable<Book>?> GetEntitiesByParamAsync(Dictionary<string, object> param) => await
        RepositoryManager.GetDataRepository.GetEntitiesByParamAsync<Book>(_sqlProvider.GetByParam, param);

    public async Task<Book?> GetSingleEntityByParamAsync(Dictionary<string, object> param) => await
        RepositoryManager.GetDataRepository.GetSingleEntityByParamAsync<Book>(_sqlProvider.GetByParam, param);

    public async Task<IEnumerable<BookViewDto>?> GetBookActivityUserAsync(Dictionary<string, object> param) => await
        RepositoryManager.GetDataRepository.GetEntitiesByParamAsync<BookViewDto>(_sqlProvider.GetActivityBook, param); 


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
