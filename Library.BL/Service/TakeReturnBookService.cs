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

        #region IGetService

        public IEnumerable<TakeReturnBooks>? GetAllEntities() =>
            RepositoryManager.GetDataRepository.GetAllEntity<TakeReturnBooks>(_sqlProvider.GetAll);

        public IEnumerable<TakeReturnBooks>? GetEntitiesByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetEntitiesByParam<TakeReturnBooks>(_sqlProvider.GetByParam, param);

        public TakeReturnBooks? GetSingleEntityByParam(Dictionary<string, object> param) =>
            RepositoryManager.GetDataRepository.GetSingleEntityByParam<TakeReturnBooks>(_sqlProvider.GetByParam, param);

        #endregion
    }
}
