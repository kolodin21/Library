using Library.BL.ModelsDTO.User;
using Library.DAL.Models;
using Library.DAL.Repositories;

namespace Library.BL.Service
{
    public class UserService:BaseService,IUserService
    {

        #region Constructor

        private readonly ISqlUserProvider _sqlProvider;
        public UserService(IRepositoryManager repositoryManager, ISqlUserProvider sqlProvider): 
            base(repositoryManager)
        {
            _sqlProvider = sqlProvider;
        }
        
        #endregion

        #region GetService

        public IEnumerable<User>? GetAllEntities() =>
            RepositoryManager.GetDataRepository.GetAllEntity<User>(_sqlProvider.GetAll);
        public IEnumerable<User>? GetEntitiesByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetEntitiesByParam<User>(_sqlProvider.GetByParam, param);

        public User? GetSingleEntityByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetSingleEntityByParam<User>(_sqlProvider.GetByParam, param);

        #endregion

        #region AddService

        public bool AddEntity(UserAddDto userAddDto) =>
            RepositoryManager.ModificationRepository.AddEntity<UserAddDto>(_sqlProvider.Add, userAddDto, true);

        #endregion

        #region DeleteService

        //public bool DeleteEntity(Dictionary<string, object> param) =>
        //    RepositoryManager.ModificationRepository.DeleteEntityDynamic(_sqlProvider.Delete, param);

        //public bool DeleteEntity(UserAddDto userAddDto) =>
        //    RepositoryManager.ModificationRepository.DeleteEntityDynamic(_sqlProvider.Delete, userAddDto);

        public bool DeleteEntity(int id) =>
            RepositoryManager.ModificationRepository.DeleteEntityByIdProcedure(_sqlProvider.Delete, id);

        #endregion

        #region UpdateService

        public bool UpdateEntity(UserUpdatePersonalInfoDto entity) => UpdatePersonalInfo(entity);
        private bool UpdatePersonalInfo(UserUpdatePersonalInfoDto updatePersonalInfo)
        {
            // Получаем имена колонок таблицы
            var columnNames =
                RepositoryManager.GetDataRepository.GetColumnNames(_sqlProvider.NamePersonTable,
                    _sqlProvider.GetColumnAndTypeTable);

            if (columnNames is null)
                return false;

            var updateParams = GetDynamicUpdateParams(updatePersonalInfo, columnNames);

            return RepositoryManager.ModificationRepository.UpdateEntityDynamic(_sqlProvider.NamePersonTable, updateParams!);
        }


        public bool UpdateEntity(UserUpdateContactInfoDto entity) => UpdateContactInfo(entity);
        private bool UpdateContactInfo(UserUpdateContactInfoDto updateContactInfo)
        {
            
            // Получаем имена колонок таблицы
            var columnNames =
                RepositoryManager.GetDataRepository.GetColumnNames(_sqlProvider.MainNameTable,
                    _sqlProvider.GetColumnAndTypeTable);

            if (columnNames is null)
                return false;

            var updateParams = GetDynamicUpdateParams(updateContactInfo, columnNames);

            return RepositoryManager.ModificationRepository.UpdateEntityDynamic(_sqlProvider.MainNameTable, updateParams!);
        }

        #endregion
    }
}