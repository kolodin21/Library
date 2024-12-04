using Dapper;
using Library.BL.Interface;
using Library.BL.ModelsDTO.BookDto;
using Library.DAL.Models;
using Library.DAL.Repositories;

namespace Library.BL.Service
{
    public class BookService : BaseService,IBookService
    {
        #region Constructor

        private readonly ISqlBookService _sqlProvider;

        public BookService(IRepositoryManager repositoryManager, ISqlBookService sqlProvider) 
            :base(repositoryManager)
        {
            _sqlProvider = sqlProvider;
        }
            
        #endregion

        #region IGetService

        public IEnumerable<Book>? GetAllEntities() =>
            RepositoryManager.GetDataRepository.GetAllEntity<Book>(_sqlProvider.GetAll);

        public IEnumerable<Book>? GetEntitiesByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetEntitiesByParam<Book>(_sqlProvider.GetByParam, param);

        public Book? GetSingleEntityByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetSingleEntityByParam<Book>(_sqlProvider.GetByParam, param);

        #endregion

        #region IAddService

        public bool AddEntity(BookAddDto bookAddDto) =>
            RepositoryManager.ModificationRepository.AddEntity<BookAddDto>(_sqlProvider.Add, bookAddDto, true);

        #endregion

        #region IDeleteService

        public bool DeleteEntity(int id) =>
            RepositoryManager.ModificationRepository.DeleteEntityByIdProcedure(_sqlProvider.Delete, id);
        
        #endregion

        #region IUpdateService

        public bool UpdateEntity(BookUpdateInfoDto updateBookInfo)
        {
            // Получаем имена колонок таблицы
            var columnNames =
                RepositoryManager.GetDataRepository.GetColumnNames(_sqlProvider.NameBookTable,
                    _sqlProvider.GetColumnAndTypeTable);

            var updateParams = GetDynamicUpdateParams(updateBookInfo, columnNames!); 
            
            return RepositoryManager.ModificationRepository.UpdateEntityDynamic(_sqlProvider.NameBookTable, updateParams);
        }

        #endregion
    }
}
