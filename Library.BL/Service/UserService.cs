﻿using Library.BL.Interface;
using Library.BL.ModelsDTO.User;
using Library.BL.ModelsDTO.UserDto.UserDto;
using Library.DAL.Models;
using Library.DAL.Repositories;

namespace Library.BL.Service
{
    public class UserService:BaseService,IUserService
    {

        #region Constructor

        private readonly ISqlQueryUserService _sqlQueryProvider;
        public UserService(IRepositoryManager repositoryManager, ISqlQueryUserService sqlQueryProvider): 
            base(repositoryManager)
        {
            _sqlQueryProvider = sqlQueryProvider;
        }
        
        #endregion

        #region IGetService

        public IEnumerable<User>? GetAllEntities() =>
            RepositoryManager.GetDataRepository.GetAllEntity<User>(_sqlQueryProvider.GetAll);

        public User? GetSingleEntityByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetSingleEntityByParam<User>(_sqlQueryProvider.GetByParam, param);

        public IEnumerable<User>? GetEntitiesByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetEntitiesByParam<User>(_sqlQueryProvider.GetByParam, param);

        #endregion

        #region IAddEntity

        public bool AddEntity(UserAddDto userAddDto) =>
            RepositoryManager.ModificationRepository.AddEntity<UserAddDto>(_sqlQueryProvider.Add, userAddDto, true);

        #endregion

        #region IDeleteEntity

        public bool DeleteEntity(Dictionary<string, object> param) =>
            RepositoryManager.ModificationRepository.DeleteEntityDynamic(_sqlQueryProvider.Delete, param);

        public bool DeleteEntity(UserAddDto userAddDto) =>
            RepositoryManager.ModificationRepository.DeleteEntityDynamic(_sqlQueryProvider.Delete, userAddDto);

        #endregion

        #region IUpdateService

        public bool UpdateEntity(UserUpdatePersonalInfoDto entity) => UpdatePersonalInfo(entity);
        private bool UpdatePersonalInfo(UserUpdatePersonalInfoDto updatePersonalInfo)
        {
            //var updateParams = new Dictionary<string, object>
            //{
            //    ["id"] = updatePersonalInfo.Id
            //};

            //if (!string.IsNullOrWhiteSpace(updatePersonalInfo.Surname))
            //    updateParams["surname"] = updatePersonalInfo.Surname;

            //if (!string.IsNullOrWhiteSpace(updatePersonalInfo.Name))
            //    updateParams["name"] = updatePersonalInfo.Name;

            //if (!string.IsNullOrWhiteSpace(updatePersonalInfo.Patronymic))
            //    updateParams["patronymic"] = updatePersonalInfo.Patronymic;

            //if (updateParams.Count <= 1) return false;

            // Получаем имена колонок таблицы
            var columnNames =
                RepositoryManager.GetDataRepository.GetColumnNames(_sqlQueryProvider.NamePersonTable,
                    _sqlQueryProvider.GetColumnAndTypeTable);

            if (columnNames is null)
                return false;

            var updateParams = GetDynamicUpdateParams(updatePersonalInfo, columnNames);

            return RepositoryManager.ModificationRepository.UpdateEntityDynamic(_sqlQueryProvider.NamePersonTable, updateParams!);
        }


        public bool UpdateEntity(UserUpdateContactInfoDto entity) => UpdateContactInfo(entity);
        private bool UpdateContactInfo(UserUpdateContactInfoDto updateContactInfo)
        {
            //var updateParams = new Dictionary<string, object>
            //{
            //    ["id"] = updateContactInfo.Id
            //};

            //if (!string.IsNullOrWhiteSpace(updateContactInfo.Phone))
            //    updateParams["phone"] = updateContactInfo.Phone;

            //if (!string.IsNullOrWhiteSpace(updateContactInfo.Email))
            //    updateParams["email"] = updateContactInfo.Email;

            //if (updateParams.Count <= 1) return false;


            // Получаем имена колонок таблицы
            var columnNames =
                RepositoryManager.GetDataRepository.GetColumnNames(_sqlQueryProvider.NameUserTable,
                    _sqlQueryProvider.GetColumnAndTypeTable);

            if (columnNames is null)
                return false;

            var updateParams = GetDynamicUpdateParams(updateContactInfo, columnNames);

            return RepositoryManager.ModificationRepository.UpdateEntityDynamic(_sqlQueryProvider.NameUserTable, updateParams!);
        }

        #endregion
    }
}
 