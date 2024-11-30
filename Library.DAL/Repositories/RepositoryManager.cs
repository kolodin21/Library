using Library.Common;
using Library.DAL.Interface;

namespace Library.DAL.Repositories
{
    public class RepositoryManager(IGetRepository getRepository, IModificationRepository modificationRepository)
    {
        // Репозитории
        public IGetRepository GetDataRepository { get; } = getRepository;
        public IModificationRepository ModificationRepository { get; } = modificationRepository;
    }
}
