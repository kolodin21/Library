using Library.Server.DAL.Repositories;
using Library.Models;
using Library.Models.ModelsDTO;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Server.BL.Service
{
    public class AuthorService : BaseService, IAuthorService
    {
        #region Constructor
        private readonly ISqlProvider<Author>_sqlProvider;
        public AuthorService(IRepositoryManager repositoryManager, ISqlProvider<Author> sqlProvider,IMemoryCache cache) :
            base(repositoryManager, cache)
        {
            _sqlProvider = sqlProvider;
        }
        #endregion

        #region GetService

        public async Task<IEnumerable<Author>?> GetAllEntitiesAsync() => await 
            RepositoryManager.GetDataRepository.GetAllEntityAsync<Author>(_sqlProvider.GetAll);

        public async Task<Author?> GetSingleEntityByParamAsync(Dictionary<string, object> param) => await 
            RepositoryManager.GetDataRepository.GetSingleEntityByParamAsync<Author>(_sqlProvider.GetByParam, param);

        #endregion

        #region AddService

        public async Task<bool> AddEntityAsync(Author entity) => await 
            RepositoryManager.ModificationRepository.AddEntityAsync(_sqlProvider.Add, entity, true);

        #endregion

        #region DeleteService

        public async Task<bool> DeleteEntityAsync(Author entity) => await 
            RepositoryManager.ModificationRepository.DeleteEntityDynamicAsync(_sqlProvider.MainNameTable, entity);
        
        #endregion

        #region UpdateService

        public async Task<bool> UpdateEntityAsync(Author author)
        {
            // Получаем имена колонок таблицы
            var columnNames = await 
                RepositoryManager.GetDataRepository.GetColumnNamesAsync(_sqlProvider.MainNameTable,
                    _sqlProvider.GetColumnAndTypeTable);

            var updateParams = GetDynamicUpdateParams(author, columnNames!);

            return await RepositoryManager.ModificationRepository.UpdateEntityDynamicAsync(_sqlProvider.MainNameTable, updateParams);
        }

        public async Task<bool> AddEntityAsync(AuthorDto entity) => await 
             RepositoryManager.ModificationRepository.AddEntityAsync(_sqlProvider.Add, entity, true);
        #endregion
    }
}
