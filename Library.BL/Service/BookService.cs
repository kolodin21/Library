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

        public IEnumerable<Book>? GetAllEntities()
        {
            return _repositoryManager.GetDataRepository.GetAllEntity<Book>(
                SqlQuery.GetAllBooks,
                multi =>
                {
                    var books =  MultiGetBooks(multi);
                    return books;
                });
        }

        public Book? GetEntityByParam(Dictionary<string, object> param) =>
            _repositoryManager.GetDataRepository.GetEntityByParam<Book>(SqlQuery.GetBook, param,
                multi =>
                {
                    var books = MultiGetBooks(multi);
                    return books.FirstOrDefault()!;
                });











        private static IEnumerable<Book> MultiGetBooks(SqlMapper.GridReader multi)
        {
            var books = multi.Read<Book, Author, Publisher, Condition, Book>(
                (book, author, publisher, condition) =>
                {
                    book.Author = author;
                    book.Publisher = publisher;
                    book.Condition = condition;
                    return book;
                },
                splitOn: "Author_Id,Publisher_Id,Condition_Id"); // Разделяем сущности по указанным столбцам);
            return books;
        }
    }

}
