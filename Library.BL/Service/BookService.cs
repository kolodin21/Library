using Dapper;
using Library.BL.Interface;
using Library.DAL.Models;
using Library.DAL.Repositories;

namespace Library.BL.Service
{
    public class BookService(RepositoryManager repositoryManager) :
        IGetService<Book>
    {
        private readonly RepositoryManager _repositoryManager = repositoryManager;

        #region IGetService

        public IEnumerable<Book>? GetAllEntities() =>
            _repositoryManager.GetDataRepository.GetAllEntity<Book>(SqlQuery.GetBooks);

        public Book? GetSingleEntityByParam(Dictionary<string, object> param) =>
            _repositoryManager.GetDataRepository.GetSingleEntityByParam<Book>(SqlQuery.GetBookByParam, param);

        public IEnumerable<Book>? GetEntitiesByParam(Dictionary<string, object> param) =>
            _repositoryManager.GetDataRepository.GetEntitiesByParam<Book>(SqlQuery.GetBookByParam, param);

        #endregion




    }
    //Todo
    // Добавить остальные интерфейсы 
    // Добавить классы BookDto
    // Добавить комментарии в классы
    

}
