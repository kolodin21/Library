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

        private readonly ISqlQueryBookService _sqlQueryProvider;

        public BookService(IRepositoryManager repositoryManager, ISqlQueryBookService sqlQueryProvider) 
            :base(repositoryManager)
        {
            _sqlQueryProvider = sqlQueryProvider;
        }
            
        #endregion

        #region IGetService

        public IEnumerable<Book>? GetAllEntities() =>
            RepositoryManager.GetDataRepository.GetAllEntity<Book>(_sqlQueryProvider.GetAll);

        public Book? GetSingleEntityByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetSingleEntityByParam<Book>(_sqlQueryProvider.GetByParam, param);

        public IEnumerable<Book>? GetEntitiesByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetEntitiesByParam<Book>(_sqlQueryProvider.GetByParam, param);

        #endregion

        #region IAddEntity

        public bool AddEntity(BookAddDto bookAddDto) =>
            RepositoryManager.ModificationRepository.AddEntity<BookAddDto>(_sqlQueryProvider.Add, bookAddDto, true);

        #endregion

        #region IDeleteEntity



        #endregion

        #region IUpdateService

        public bool UpdateEntity(BookUpdateInfoDto updateBookInfo)
        {
            // Получаем имена колонок таблицы
            var columnNames =
                RepositoryManager.GetDataRepository.GetColumnNames(_sqlQueryProvider.NameBookTable,
                    _sqlQueryProvider.GetColumnAndTypeTable);

            var updateParams = GetDynamicUpdateParams(updateBookInfo, columnNames!); 
            
            return RepositoryManager.ModificationRepository.UpdateEntityDynamic(_sqlQueryProvider.NameBookTable, updateParams);
        }

        #endregion
    }
}
