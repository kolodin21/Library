using Library.BL.Interface;
using Library.BL.ModelsDTO;
using Library.DAL.Models;
using Library.DAL.Repositories;

namespace Library.BL.Service
{
    public class UserService(RepositoryManager repositoryManager) : 
        IGetService<User>, 
        IAddEntity<UserDto>,
        IDeleteEntity<UserDto>,
        IUpdateService<UserPersonalInfoDto>,  // Реализация интерфейса для UserPersonalInfoDto
        IUpdateService<UserContactInfoDto>   // Реализация интерфейса для UserContactInfoDto
    {
        private readonly RepositoryManager _repositoryManager = repositoryManager;

        #region IGetService

        public IEnumerable<User>? GetAllEntities() =>
            _repositoryManager.GetDataRepository.GetAllEntity<User>(SqlQuery.GetAllUsers);

        public User? GetSingleEntityByParam(Dictionary<string, object> param) =>
            _repositoryManager.GetDataRepository.GetSingleEntityByParam<User>(SqlQuery.GetUserByParam, param);

        public IEnumerable<User>? GetEntitiesByParam(Dictionary<string, object> param) =>
            _repositoryManager.GetDataRepository.GetEntitiesByParam<User>(SqlQuery.GetUserByParam, param);

        #endregion

        #region IAddEntity

        public bool AddEntity(UserDto userDto) =>
            _repositoryManager.ModificationRepository.AddEntity<UserDto>(SqlQuery.AddUser, userDto, true);

        #endregion

        #region IDeleteEntity

        public bool DeleteEntity(Dictionary<string, object> param) =>
            _repositoryManager.ModificationRepository.DeleteEntityDynamic(SqlQuery.NameUsersTable, param);

        public bool DeleteEntity(UserDto userDto) =>
            _repositoryManager.ModificationRepository.DeleteEntityDynamic(SqlQuery.NameUsersTable, userDto);

        #endregion

        #region IUpdateService

        public bool UpdateEntity(UserPersonalInfoDto entity) => UpdatePersonalInfo(entity);
        private bool UpdatePersonalInfo(UserPersonalInfoDto personalInfo)
        {
            var updateParams = new Dictionary<string, object>
            {
                ["id"] = personalInfo.Id
            };

            if (!string.IsNullOrWhiteSpace(personalInfo.Surname))
                updateParams["surname"] = personalInfo.Surname;

            if (!string.IsNullOrWhiteSpace(personalInfo.Name))
                updateParams["name"] = personalInfo.Name;

            if (!string.IsNullOrWhiteSpace(personalInfo.Patronymic))
                updateParams["patronymic"] = personalInfo.Patronymic;

            if (updateParams.Count <= 1) return false;

            return _repositoryManager.ModificationRepository.UpdateEntityDynamic(SqlQuery.NamePersonTable, updateParams);
        }


        public bool UpdateEntity(UserContactInfoDto entity) => UpdateContactInfo(entity);
        private bool UpdateContactInfo(UserContactInfoDto contactInfo)
        {
            var updateParams = new Dictionary<string, object>
            {
                ["id"] = contactInfo.Id
            };

            if (!string.IsNullOrWhiteSpace(contactInfo.Phone))
                updateParams["phone"] = contactInfo.Phone;

            if (!string.IsNullOrWhiteSpace(contactInfo.Email))
                updateParams["email"] = contactInfo.Email;

            if (updateParams.Count <= 1) return false;

            return _repositoryManager.ModificationRepository.UpdateEntityDynamic(SqlQuery.NameUsersTable, updateParams);
        }

        #endregion

    }
}
 