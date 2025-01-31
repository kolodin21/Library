using Library.Models;
using Library.Models.ModelsDTO;
using Library.Server.DAL.Repositories;

namespace Library.Server.BL.Service
{
    public class ConditionService : BaseService, IConditionService
    {
        #region Constructor

        private readonly ISqlProvider<Condition> _sqlProvider;
        public ConditionService(IRepositoryManager repositoryManager, ISqlProvider<Condition> sqlProvider) :
            base(repositoryManager)
        {
            _sqlProvider = sqlProvider;
        }
        #endregion

        #region GetService

        public async Task<IEnumerable<Condition>?> GetAllEntitiesAsync() => await 
            RepositoryManager.GetDataRepository.GetAllEntityAsync<Condition>(_sqlProvider.GetAll);

        public async Task<Condition?> GetSingleEntityByParamAsync(Dictionary<string, object> param) => await 
            RepositoryManager.GetDataRepository.GetSingleEntityByParamAsync<Condition>(_sqlProvider.GetByParam, param);

        #endregion

        #region AddService
        public async Task<bool> AddEntityAsync(ConditionDto condition) => await 
            RepositoryManager.ModificationRepository.AddEntityAsync(_sqlProvider.Add, condition, true);

        #endregion

        #region DeleteService

        public async Task<bool> DeleteEntityAsync(Condition condition) => await 
            RepositoryManager.ModificationRepository.DeleteEntityDynamicAsync(_sqlProvider.MainNameTable, condition);

        #endregion

        #region UpdateService

        public async Task<bool> UpdateEntityAsync(Condition condition)
        {
            // Получаем имена колонок таблицы
            var columnNames = await 
                RepositoryManager.GetDataRepository.GetColumnNamesAsync(_sqlProvider.MainNameTable,
                    _sqlProvider.GetColumnAndTypeTable);

            var updateParams = GetDynamicUpdateParams(condition, columnNames!);

            return await RepositoryManager.ModificationRepository.UpdateEntityDynamicAsync(_sqlProvider.MainNameTable, updateParams!);
        }
        #endregion
    }
}