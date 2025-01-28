using Library.Server.BL.Models;
using Library.Server.BL.ModelsDTO.User;
using Library.Server.DAL.Repositories;

namespace Library.Server.BL.Service
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

        public async Task<IEnumerable<User>?> GetAllEntitiesAsync() =>
           await RepositoryManager.GetDataRepository.GetAllEntityAsync<User>(_sqlProvider.GetAll);

        public async Task<IEnumerable<User>?> GetEntitiesByParamAsync(Dictionary<string, object> param) =>
           await RepositoryManager.GetDataRepository.GetEntitiesByParamAsync<User>(_sqlProvider.GetByParam, param);

        public async Task<User?> GetSingleEntityByParamAsync(Dictionary<string, object> param) =>
            await RepositoryManager.GetDataRepository.GetSingleEntityByParamAsync<User>(_sqlProvider.GetByParam, param);

        #endregion

        #region AddService

        public async Task<bool> AddEntityAsync(UserAddDto userAddDto) =>
            await RepositoryManager.ModificationRepository.AddEntityAsync<UserAddDto>(_sqlProvider.Add, userAddDto, true);

        #endregion

        #region DeleteService

        //public bool DeleteEntity(Dictionary<string, object> param) =>
        //    RepositoryManager.ModificationRepository.DeleteEntityDynamic(_sqlProvider.Delete, param);

        //public bool DeleteEntity(UserAddDto userAddDto) =>
        //    RepositoryManager.ModificationRepository.DeleteEntityDynamic(_sqlProvider.Delete, userAddDto);

        public async Task<bool> DeleteEntityAsync(int id) =>
           await RepositoryManager.ModificationRepository.DeleteEntityByIdProcedureAsync(_sqlProvider.Delete, id);

        #endregion

        #region UpdateService

        public async Task<bool> UpdateEntityAsync(UserUpdatePersonalInfoDto entity) => await UpdatePersonalInfoAsync(entity);
        private async Task<bool> UpdatePersonalInfoAsync(UserUpdatePersonalInfoDto updatePersonalInfo)
        {
            // Получаем имена колонок таблицы
            var columnNames = await 
                RepositoryManager.GetDataRepository.GetColumnNamesAsync(_sqlProvider.NamePersonTable,
                    _sqlProvider.GetColumnAndTypeTable);

            var updateParams = GetDynamicUpdateParams(updatePersonalInfo, columnNames);

            return await RepositoryManager.ModificationRepository.UpdateEntityDynamicAsync(_sqlProvider.NamePersonTable, updateParams!);
        }


        public async Task<bool> UpdateEntityAsync(UserUpdateContactInfoDto entity) => await UpdateContactInfoAsync(entity);
        private async Task<bool> UpdateContactInfoAsync(UserUpdateContactInfoDto updateContactInfo)
        {
            
            // Получаем имена колонок таблицы
            var columnNames = await 
                RepositoryManager.GetDataRepository.GetColumnNamesAsync(_sqlProvider.MainNameTable,
                    _sqlProvider.GetColumnAndTypeTable);

            var updateParams = GetDynamicUpdateParams(updateContactInfo, columnNames);

            return await RepositoryManager.ModificationRepository.UpdateEntityDynamicAsync(_sqlProvider.MainNameTable, updateParams!);
        }
        #endregion
    }
}