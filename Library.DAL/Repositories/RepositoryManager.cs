using Library.DAL.Interface;

namespace Library.DAL.Repositories
{
    public interface IRepositoryManager
    {
        IGetRepository GetDataRepository { get; }
        IModificationRepository ModificationRepository { get; }
    }

    public class RepositoryManager : IRepositoryManager
    {
        // Репозитории
        public IGetRepository GetDataRepository { get; } 
        public IModificationRepository ModificationRepository { get; } 

        public RepositoryManager(IGetRepository getDataRepository, IModificationRepository modificationRepository)
        {
            GetDataRepository = getDataRepository;
            ModificationRepository = modificationRepository;
        }
    }
}
