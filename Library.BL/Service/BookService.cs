using Dapper;
using Library.BL.Interface;
using Library.BL.ModelsDTO.BookDto;
using Library.DAL.Models;
using Library.DAL.Repositories;

namespace Library.BL.Service
{
    public class BookService : IBookService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ISqlQueryBookService _sqlQueryProvider;

        public BookService(IRepositoryManager repositoryManager, ISqlQueryBookService sqlQueryProvider)
        {
            _repositoryManager = repositoryManager;
            _sqlQueryProvider = sqlQueryProvider;
        }

        #region IGetService

        public IEnumerable<Book>? GetAllEntities() =>
            _repositoryManager.GetDataRepository.GetAllEntity<Book>(SqlQuery.GetBooks);

        public Book? GetSingleEntityByParam(Dictionary<string, object> param) =>
            _repositoryManager.GetDataRepository.GetSingleEntityByParam<Book>(SqlQuery.GetBookByParam, param);

        public IEnumerable<Book>? GetEntitiesByParam(Dictionary<string, object> param) =>
            _repositoryManager.GetDataRepository.GetEntitiesByParam<Book>(SqlQuery.GetBookByParam, param);

        #endregion

        #region IAddEntity

        public bool AddEntity(BookAddDto bookAddDto) =>
            _repositoryManager.ModificationRepository.AddEntity<BookAddDto>(SqlQuery.AddBook, bookAddDto, true);

        #endregion

        #region IUpdateService

        public bool UpdateEntity(BookUpdateInfoDto updateBookInfo)
        {
            // Получаем имена колонок таблицы
            var columnNames =
                _repositoryManager.GetDataRepository.GetColumnNames(_sqlQueryProvider.NameBookTable,
                    _sqlQueryProvider.GetColumnAndTypeTable);

            var updateParams = GetDynamicUpdateParams(updateBookInfo, columnNames!); 
            
            return _repositoryManager.ModificationRepository.UpdateEntityDynamic(_sqlQueryProvider.NameBookTable, updateParams);
        }

        #endregion

        private Dictionary<string, object> GetDynamicUpdateParams<T>(T entity, IEnumerable<string> columnNames) where T: class
        {
            var updateParams = new Dictionary<string, object>();
            // Получаем свойства объекта
            var properties = entity.GetType().GetProperties();

            foreach (var property in properties)
            {
                var columnName = columnNames.FirstOrDefault(c =>
                    string.Equals(c, property.Name, StringComparison.OrdinalIgnoreCase));

                if (property.GetValue(entity) != null && columnName != null)
                {
                    updateParams[columnName] = property.GetValue(entity)!;
                }
            }
            return updateParams;
        }

    }
    //Todo
    // Добавить остальные интерфейсы 
    // Добавить классы BookAddDto
    // Добавить комментарии в классы


}
