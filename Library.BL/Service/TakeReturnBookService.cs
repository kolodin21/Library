using Library.BL.Interface;
using Library.BL.ModelsDTO.TakeReturn;
using Library.DAL.Models;
using Library.DAL.Repositories;

namespace Library.BL.Service
{
    public class TakeReturnBookService : BaseService, ITakeReturnBookService
    {
        #region Constructor
        private readonly ISqlTakeReturnBookProvider _sqlProvider;
        public TakeReturnBookService(
            IRepositoryManager repositoryManager,
            ISqlTakeReturnBookProvider sqlProvider) : 
            base(repositoryManager)
        {
            _sqlProvider = sqlProvider;
        }
        #endregion

        #region GetService

        public IEnumerable<TakeReturnBooks>? GetAllEntities() =>
            RepositoryManager.GetDataRepository.GetAllEntity<TakeReturnBooks>(_sqlProvider.GetAll);

        public IEnumerable<TakeReturnBooks>? GetEntitiesByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetEntitiesByParam<TakeReturnBooks>(_sqlProvider.GetByParam, param);

        public TakeReturnBooks? GetSingleEntityByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetSingleEntityByParam<TakeReturnBooks>(_sqlProvider.GetByParam, param);

        public List<TakeReturnBookDto>? GetAllEntitiesDto()
        {
            List<TakeReturnBookDto> books = [];
            var takeReturn = GetAllEntities();

            books.AddRange(takeReturn.Select(book => new TakeReturnBookDto
            {
                Id = book.Id,
                PersonId = book.PersonId,
                NameAuthor = book.NameAuthor,
                TitleBook = book.TitleBook,
                DateIssuance = book.DateIssuance,
                DateReturn = book.DateReturn
            }));
            return books;
        }

        #endregion

        #region AddReturnService

        public bool AddEntity(TakeBookDto takeBook) =>
            RepositoryManager.ModificationRepository.AddEntity(_sqlProvider.Add, takeBook, true);

        public bool AddEntity(ReturnBookDto returnBook) =>
            RepositoryManager.ModificationRepository.AddEntity(_sqlProvider.ReturnBook, returnBook,true);


        #endregion

    }
}
