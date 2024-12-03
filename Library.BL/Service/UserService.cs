﻿using Library.BL.Interface;
using Library.BL.ModelsDTO.User;
using Library.BL.ModelsDTO.UserDto.UserDto;
using Library.DAL.Models;
using Library.DAL.Repositories;

namespace Library.BL.Service
{
    public class UserService: IUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ISqlQueryUserService _sqlQueryProvider;

        public UserService(IRepositoryManager repositoryManager, ISqlQueryUserService sqlQueryProvider)
        {
            _repositoryManager = repositoryManager;
            _sqlQueryProvider = sqlQueryProvider;
        }

        #region IGetService

        public IEnumerable<User>? GetAllEntities() =>
            _repositoryManager.GetDataRepository.GetAllEntity<User>(_sqlQueryProvider.GetAllUsers);

        public User? GetSingleEntityByParam(Dictionary<string, object> param) =>
            _repositoryManager.GetDataRepository.GetSingleEntityByParam<User>(_sqlQueryProvider.GetUserByParam, param);

        public IEnumerable<User>? GetEntitiesByParam(Dictionary<string, object> param) =>
            _repositoryManager.GetDataRepository.GetEntitiesByParam<User>(_sqlQueryProvider.GetUserByParam, param);

        #endregion

        #region IAddEntity

        public bool AddEntity(UserAddDto userAddDto) =>
            _repositoryManager.ModificationRepository.AddEntity<UserAddDto>(_sqlQueryProvider.AddUser, userAddDto, true);

        #endregion

        #region IDeleteEntity

        public bool DeleteEntity(Dictionary<string, object> param) =>
            _repositoryManager.ModificationRepository.DeleteEntityDynamic(_sqlQueryProvider.NameUsersTable, param);

        public bool DeleteEntity(UserAddDto userAddDto) =>
            _repositoryManager.ModificationRepository.DeleteEntityDynamic(_sqlQueryProvider.NameUsersTable, userAddDto);

        #endregion

        #region IUpdateService

        public bool UpdateEntity(UserUpdatePersonalInfoDto entity) => UpdatePersonalInfo(entity);
        private bool UpdatePersonalInfo(UserUpdatePersonalInfoDto updatePersonalInfo)
        {
            var updateParams = new Dictionary<string, object>
            {
                ["id"] = updatePersonalInfo.Id
            };

            if (!string.IsNullOrWhiteSpace(updatePersonalInfo.Surname))
                updateParams["surname"] = updatePersonalInfo.Surname;

            if (!string.IsNullOrWhiteSpace(updatePersonalInfo.Name))
                updateParams["name"] = updatePersonalInfo.Name;

            if (!string.IsNullOrWhiteSpace(updatePersonalInfo.Patronymic))
                updateParams["patronymic"] = updatePersonalInfo.Patronymic;

            if (updateParams.Count <= 1) return false;

            return _repositoryManager.ModificationRepository.UpdateEntityDynamic(_sqlQueryProvider.NamePersonTable, updateParams);
        }


        public bool UpdateEntity(UserUpdateContactInfoDto entity) => UpdateContactInfo(entity);
        private bool UpdateContactInfo(UserUpdateContactInfoDto updateContactInfo)
        {
            var updateParams = new Dictionary<string, object>
            {
                ["id"] = updateContactInfo.Id
            };

            if (!string.IsNullOrWhiteSpace(updateContactInfo.Phone))
                updateParams["phone"] = updateContactInfo.Phone;

            if (!string.IsNullOrWhiteSpace(updateContactInfo.Email))
                updateParams["email"] = updateContactInfo.Email;

            if (updateParams.Count <= 1) return false;

            return _repositoryManager.ModificationRepository.UpdateEntityDynamic(_sqlQueryProvider.NameUsersTable, updateParams);
        }

        #endregion

    }
}
 