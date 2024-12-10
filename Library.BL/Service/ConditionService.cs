using Library.BL.ModelsDTO.Others;
using Library.DAL.Repositories;
using Library.BL.Models;

namespace Library.BL.Service
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

        public IEnumerable<Condition>? GetAllEntities() =>
            RepositoryManager.GetDataRepository.GetAllEntity<Condition>(_sqlProvider.GetAll);

        public Condition? GetSingleEntityByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetSingleEntityByParam<Condition>(_sqlProvider.GetByParam, param);

        #endregion

        #region AddService
        public bool AddEntity(ConditionDto condition) =>
            RepositoryManager.ModificationRepository.AddEntity(_sqlProvider.Add, condition, true);

        #endregion

        #region DeleteService

        public bool DeleteEntity(Condition condition) =>
            RepositoryManager.ModificationRepository.DeleteEntityDynamic(_sqlProvider.MainNameTable, condition);

        #endregion

        #region UpdateService

        public bool UpdateEntity(Condition condition)
        {
            // Получаем имена колонок таблицы
            var columnNames =
                RepositoryManager.GetDataRepository.GetColumnNames(_sqlProvider.MainNameTable,
                    _sqlProvider.GetColumnAndTypeTable);

            var updateParams = GetDynamicUpdateParams(condition, columnNames!);

            return RepositoryManager.ModificationRepository.UpdateEntityDynamic(_sqlProvider.MainNameTable, updateParams);
        }


        #endregion
    }
}