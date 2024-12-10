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

        

        public IEnumerable<Publisher>? GetAllEntities() =>
            RepositoryManager.GetDataRepository.GetAllEntity<Publisher>(_sqlProvider.GetAll);

        public Publisher? GetSingleEntityByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetSingleEntityByParam<Publisher>(_sqlProvider.GetByParam, param);

        #endregion

        #region AddService

        public bool AddEntity(PublisherDto publisher) =>
            RepositoryManager.ModificationRepository.AddEntity(_sqlProvider.Add, publisher, true);

        #endregion

        #region DeleteService

        public bool DeleteEntity(Publisher publisher) =>
            RepositoryManager.ModificationRepository.DeleteEntityDynamic(_sqlProvider.MainNameTable, publisher);

        #endregion

        #region UpdateService
        public bool UpdateEntity(Publisher publisher)
        {
            // Получаем имена колонок таблицы
            var columnNames =
                RepositoryManager.GetDataRepository.GetColumnNames(_sqlProvider.MainNameTable,
                    _sqlProvider.GetColumnAndTypeTable);

            var updateParams = GetDynamicUpdateParams(publisher, columnNames!);

            return RepositoryManager.ModificationRepository.UpdateEntityDynamic(_sqlProvider.MainNameTable, updateParams);
        }
        #endregion
    }
}
