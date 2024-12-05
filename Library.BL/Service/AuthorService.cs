using Library.BL.ModelsDTO.Others;
using Library.DAL.Models;
using Library.DAL.Repositories;

namespace Library.BL.Service
{
    public class AuthorService : BaseService, IAuthorService
    {
        #region Constructor
        private readonly ISqlProvider<Author>_sqlProvider;
        public AuthorService(IRepositoryManager repositoryManager, ISqlProvider<Author> sqlProvider) :
            base(repositoryManager)
        {
            _sqlProvider = sqlProvider;
        }
        #endregion

        #region GetService

        public IEnumerable<Author>? GetAllEntities() =>
            RepositoryManager.GetDataRepository.GetAllEntity<Author>(_sqlProvider.GetAll);

        public Author? GetSingleEntityByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetSingleEntityByParam<Author>(_sqlProvider.GetByParam, param);

        #endregion

        #region AddService

        public bool AddEntity(Author entity) =>
            RepositoryManager.ModificationRepository.AddEntity(_sqlProvider.Add, entity, true);

        #endregion

        #region DeleteService

        public bool DeleteEntity(Author entity) =>
            RepositoryManager.ModificationRepository.DeleteEntityDynamic(_sqlProvider.MainNameTable, entity);
        
        #endregion

        #region UpdateService

        public bool UpdateEntity(Author author)
        {
            // Получаем имена колонок таблицы
            var columnNames =
                RepositoryManager.GetDataRepository.GetColumnNames(_sqlProvider.MainNameTable,
                    _sqlProvider.GetColumnAndTypeTable);

            var updateParams = GetDynamicUpdateParams(author, columnNames!);

            return RepositoryManager.ModificationRepository.UpdateEntityDynamic(_sqlProvider.MainNameTable, updateParams);
        }

        public bool AddEntity(AuthorDto entity) =>
             RepositoryManager.ModificationRepository.AddEntity(_sqlProvider.Add, entity, true);
        #endregion
    }
}
