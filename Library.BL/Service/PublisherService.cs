using Library.BL.ModelsDTO.Others;
using Library.DAL.Repositories;
using Library.BL.Models;

namespace Library.BL.Service
{
    public class PublisherService : BaseService, IPublisherService
    {
        #region Constructor
        private readonly ISqlProvider<Publisher> _sqlProvider;
        public PublisherService(IRepositoryManager repositoryManager, ISqlProvider<Publisher> sqlProvider) : base(repositoryManager)
        {
            _sqlProvider = sqlProvider;
        }
        #endregion

        #region GetService

        public async Task<IEnumerable<Publisher>?> GetAllEntitiesAsync() => await  
            RepositoryManager.GetDataRepository.GetAllEntityAsync<Publisher>(_sqlProvider.GetAll);

        public async Task<Publisher?> GetSingleEntityByParamAsync(Dictionary<string, object> param) => await 
            RepositoryManager.GetDataRepository.GetSingleEntityByParamAsync<Publisher>(_sqlProvider.GetByParam, param);

        #endregion

        #region AddService

        public async Task<bool> AddEntityAsync(PublisherDto publisher) => await 
            RepositoryManager.ModificationRepository.AddEntityAsync(_sqlProvider.Add, publisher, true);

        #endregion

        #region DeleteService

        public async Task<bool> DeleteEntityAsync(Publisher publisher) => await 
            RepositoryManager.ModificationRepository.DeleteEntityDynamicAsync(_sqlProvider.MainNameTable, publisher);

        #endregion

        #region UpdateService
        public async Task<bool> UpdateEntityAsync(Publisher publisher)
        {
            // Получаем имена колонок таблицы
            var columnNames = await 
                RepositoryManager.GetDataRepository.GetColumnNamesAsync(_sqlProvider.MainNameTable,
                    _sqlProvider.GetColumnAndTypeTable);

            var updateParams = GetDynamicUpdateParams(publisher, columnNames!);

            return await RepositoryManager.ModificationRepository.UpdateEntityDynamicAsync(_sqlProvider.MainNameTable, updateParams);
        }
        #endregion
    }
}
