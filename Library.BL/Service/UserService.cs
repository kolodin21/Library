using Library.BL.Interface;
using Library.BL.ModelsDTO;
using Library.DAL.Models;
using Library.DAL.Repositories;

namespace Library.BL.Service
{
    public class UserService(RepositoryManager repositoryManager) : 
        IGetService<User>, 
        IModificationService<UserDto>,
        IUpdateEntity<UpdateUserDto>
    {
        private readonly RepositoryManager _repositoryManager = repositoryManager;

        public IEnumerable<User>? GetAllEntities() =>
            _repositoryManager.GetDataRepository.GetAllEntity<User>(SqlQuery.GetAllUsers);

        public User? GetEntityByParam(Dictionary<string, object> param) =>
            _repositoryManager.GetDataRepository.GetEntityByParam<User>(SqlQuery.GetUserByParam, param);

        public bool AddEntity(UserDto userDto) =>
            _repositoryManager.ModificationRepository.AddEntity<UserDto>(SqlQuery.AddUser, userDto, true);

        public bool DeleteEntity(Dictionary<string, object> param) =>
            _repositoryManager.ModificationRepository.DeleteEntityDynamic(SqlQuery.UserTableName, param);

        public bool DeleteEntity(UserDto userDto) =>
            _repositoryManager.ModificationRepository.DeleteEntityDynamic(SqlQuery.UserTableName, userDto);

        public bool UpdateEntity(UpdateUserDto userProfileDto)
        {
            //Надо изменить 


            // Сбор параметров для обновления
            var updateParams = new Dictionary<string, object>
            {
                ["id"] = userProfileDto.Id
            };

            if (!string.IsNullOrWhiteSpace(userProfileDto.Surname))
                updateParams["surname"] = userProfileDto.Surname;

            if (!string.IsNullOrWhiteSpace(userProfileDto.Name))
                updateParams["name"] = userProfileDto.Name;

            if (!string.IsNullOrWhiteSpace(userProfileDto.Patronymic))
                updateParams["patronymic"] = userProfileDto.Patronymic;

            if (!string.IsNullOrWhiteSpace(userProfileDto.Phone))
                updateParams["phone"] = userProfileDto.Phone;

            if (!string.IsNullOrWhiteSpace(userProfileDto.Email))
                updateParams["email"] = userProfileDto.Email;

            if (!string.IsNullOrWhiteSpace(userProfileDto.Password))
                updateParams["password"] = userProfileDto.Password; // Хешируем пароль перед обновлением

            // Если нечего обновлять
            if (updateParams.Count == 0)
                return false;

            return _repositoryManager.ModificationRepository.UpdateEntityDynamic(SqlQuery.UserTableName, updateParams);
        }
    
    }
}
 