using Library.BL.Interface;
using Library.DAL.Models;
using Library.DAL.Repositories;

namespace Library.BL.Service
{
    public class BookService(RepositoryManager repositoryManager) : IGetService<Book>
    {
        private readonly RepositoryManager _repositoryManager = repositoryManager;

        public IEnumerable<Book>? GetAllEntities() =>
            _repositoryManager.GetDataRepository.GetAllEntity<Book>(SqlQuery.GetAllBooks);

        public Book? GetEntityByParam(Dictionary<string, object> param) =>
            _repositoryManager.GetDataRepository.GetEntityByParam<Book>(SqlQuery.GetBookByParam,param);

    }
}
