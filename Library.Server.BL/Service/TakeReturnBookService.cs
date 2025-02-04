using Library.Models;
using Library.Models.ModelsDTO;
using Library.Server.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Server.BL.Service
{
    public class TakeReturnBookService : BaseService, ITakeReturnBookService
    {
        #region Constructor
        private readonly ISqlTakeReturnBookProvider _sqlProvider;
        public TakeReturnBookService(
            IRepositoryManager repositoryManager,
            ISqlTakeReturnBookProvider sqlProvider,
            IMemoryCache cache) : 
            base(repositoryManager, cache)
        {
            _sqlProvider = sqlProvider;
        }
        #endregion

        #region GetService

        public async Task<IEnumerable<TakeReturnBooks>?> GetAllEntitiesAsync() => await 
            RepositoryManager.GetDataRepository.GetAllEntityAsync<TakeReturnBooks>(_sqlProvider.GetAll);

        public async Task<IEnumerable<TakeReturnBooks>?> GetEntitiesByParamAsync(Dictionary<string, object> param) => await 
            RepositoryManager.GetDataRepository.GetEntitiesByParamAsync<TakeReturnBooks>(_sqlProvider.GetByParam, param);

        public async Task<TakeReturnBooks?> GetSingleEntityByParamAsync(Dictionary<string, object> param) => await 
            RepositoryManager.GetDataRepository.GetSingleEntityByParamAsync<TakeReturnBooks>(_sqlProvider.GetByParam, param);

        public async Task<List<TakeReturnBookDto>> GetAllEntitiesDtoAsync()
        {
            // Инициализация списка
            var books = new List<TakeReturnBookDto>();

            // Асинхронное получение данных
            var takeReturn = await GetAllEntitiesAsync();

            // Заполнение списка
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

        public async Task<bool> AddEntityAsync(TakeBookDto takeBook) => await 
            RepositoryManager.ModificationRepository.AddEntityAsync(_sqlProvider.Add, takeBook, true);

        public async Task<bool> AddEntityAsync(ReturnBookDto returnBook) => await 
            RepositoryManager.ModificationRepository.AddEntityAsync(_sqlProvider.ReturnBook, returnBook,true);

        #endregion
    }
}
