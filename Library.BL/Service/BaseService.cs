using Library.DAL.Repositories;

namespace Library.BL.Service
{
    public class BaseService
    {
        protected readonly RepositoryManager _repositoryManager;

        public BaseService(RepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

    }
}
