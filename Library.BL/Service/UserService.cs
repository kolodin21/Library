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

        public IEnumerable<User>? GetAllEntities() =>
            _repositoryManager.GetDataRepository.GetAllEntity<User>(SqlQuery.GetAllUsers);

        public User? GetEntityByParam(Dictionary<string, object> param) =>
            _repositoryManager.GetDataRepository.GetEntityByParam<User>(SqlQuery.GetUserByParam, param);

        public bool AddEntity(UserDto userDto) =>
            _repositoryManager.ModificationRepository.AddEntity<UserDto>(SqlQuery.AddUser, userDto, true);

        public bool DeleteEntity(Dictionary<string, object> param) =>
            _repositoryManager.ModificationRepository.DeleteEntityDynamic(SqlQuery.NameUsersTable, param);

        public bool DeleteEntity(UserDto userDto) =>
            _repositoryManager.ModificationRepository.DeleteEntityDynamic(SqlQuery.NameUsersTable, userDto);

        public bool UpdateEntity(UserPersonalInfoDto entity) => UpdatePersonalInfo(entity);

        public bool UpdateEntity(UserContactInfoDto entity) => UpdateContactInfo(entity);

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

    }
}
 